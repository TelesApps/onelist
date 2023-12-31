﻿using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

public class AuthController : BaseApiController
{
    private readonly DataContext context;
    private readonly ITokenService tokenService;

    public AuthController(DataContext context, ITokenService tokenService)
    {
        this.context = context;
        this.tokenService = tokenService;
    }

    [HttpPost("register")] // api/auth/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await this.UserExists(registerDto.Username))
        {
            return BadRequest("Username is taken");
        }
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        this.context.Users.Add(user);
        await this.context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = this.tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")] // api/auth/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await this.context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == loginDto.Username.ToLower());

        if (user == null)
        {
            return Unauthorized("Invalid username");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = this.tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await this.context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }

}
