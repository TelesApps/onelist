using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // api/users
public class UsersController : ControllerBase
{
    private readonly DataContext context;

    public UsersController(DataContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        var users = this.context.Users.ToList();
        return users;
    }

    [HttpGet("{id}")] // api/users/2
    public ActionResult<AppUser> GetUser(int id)
    {
        var user = this.context.Users.Find(id);
        return user;
    }
}
