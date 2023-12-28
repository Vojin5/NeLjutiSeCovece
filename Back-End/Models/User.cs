using System.ComponentModel.DataAnnotations;

namespace Back_End.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public int Elo { get; set; }
    public byte[] Image { get; set; }
    public List<MatchHistory>? Matches { get; set; }
    public User() { }

    public User(string username, string email, string password, string passwordSalt, byte[] image)
    {
        Username = username;
        Email = email;
        Password = password;
        PasswordSalt = passwordSalt;
        Elo = 0;
        Image = image;
    }

}
