using Back_End.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public Context Context { get; set; }
        public UserController(Context context)
        {
            Context = context;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] User user)
        {
            var existingUser = Context.Users.Where(u => u.Username == user.Username).FirstOrDefault();

            if (existingUser != null)
            {
                return BadRequest();
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

            user.Password = passwordHash;
            user.PasswordSalt = salt;

            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();

            return Ok();

        }

        [HttpGet("Login")]
        public async Task<ActionResult> LoginUser([FromQuery]string username, [FromQuery]string password)
        {
            var expectedUser = await Context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();

            if (expectedUser == null)
            {
                return NotFound();
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, expectedUser.PasswordSalt);
            if (hashedPassword != expectedUser.Password)
            {
                return BadRequest();
            }

            return Ok(new
            {
                expectedUser.Id,
                expectedUser.Username
            });
        }



    }
}
