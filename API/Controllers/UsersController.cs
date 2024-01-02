using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private readonly DataContext context;

    public UsersController(DataContext context)
    {
        this.context = context;
    }

    [HttpGet] // api/users
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await this.context.Users.ToListAsync();
        return users;
    }

    [HttpGet("{id}")] // api/users/${id}
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await this.context.Users.FindAsync(id);
#pragma warning disable CS8604 // Possible null reference argument.
        return user;
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
