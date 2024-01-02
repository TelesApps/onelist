using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API;

public class AuthController : BaseApiController
{
    private readonly DataContext context;

    public AuthController(DataContext context)
    {
        this.context = context;
    }

    [HttpPost("register")] // api/auth/register
    public async Task<ActionResult<AppUser>> Register(string username, string password)
    {
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            PasswordSalt = hmac.Key
        };

        this.context.Users.Add(user);
        await this.context.SaveChangesAsync();

        return user;
    }

    
}
