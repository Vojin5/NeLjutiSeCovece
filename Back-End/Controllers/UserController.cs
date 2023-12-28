using Back_End.IRepository;
using Back_End.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public IUserRepository UserRepository { get; set; }
    public UserController(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UserRegisterModel user)
    {
        var existingUser = await UserRepository.GetUserByUsernameAsync(user.Username);

        if (existingUser != null)
        {
            return BadRequest();
        }

        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

        User newUser = new User()
        {
            Username = user.Username,
            Email = user.Email,
            Password = passwordHash,
            PasswordSalt = salt,
            Elo = 0,
            Image = Convert.FromBase64String(user.ImageBase64Encoded)
        };

        await UserRepository.AddUserAsync(newUser);

        return Ok();

    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUser([FromBody] UserLoginModel user)
    {
        var expectedUser = await UserRepository.GetUserByUsernameAsync(user.Username);

        if (expectedUser == null)
        {
            return NotFound();
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, expectedUser.PasswordSalt);
        if (hashedPassword != expectedUser.Password)
        {
            return BadRequest();
        }

        return Ok(new UserLoginDTO
        {
            Id = expectedUser.Id,
            Username = expectedUser.Username,
            Elo = expectedUser.Elo,
            Image = expectedUser.Image
        });
    }
}