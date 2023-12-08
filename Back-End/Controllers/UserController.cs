using Back_End.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public Context Context { get; set; }
    public UserController(Context context)
    {
        Context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UserRegisterModel user)
    {
        var existingUser = Context.Users
            .Where(u => u.Username == user.Username)
            .FirstOrDefault();

        if (existingUser != null)
        {
            return BadRequest();
        }

        User newUser = new User();
        newUser.Username = user.Username;
        newUser.Email = user.Email;
        newUser.Elo = 0;
        newUser.Image = Convert.FromBase64String(user.ImageBase64Encoded);

        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

        newUser.Password = passwordHash;
        newUser.PasswordSalt = salt;

        await Context.Users.AddAsync(newUser);
        await Context.SaveChangesAsync();

        return Ok();

    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(UserLoginModel user)
    {
        var expectedUser = await Context.Users
            .Where(u => u.Username == user.Username)
            .FirstOrDefaultAsync();

        if (expectedUser == null)
        {
            return NotFound();
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, expectedUser.PasswordSalt);
        if (hashedPassword != expectedUser.Password)
        {
            return BadRequest();
        }

        return Ok(new
        {
            expectedUser.Id,
            expectedUser.Username,
            expectedUser.Elo,
            expectedUser.Image
        });
    }
}