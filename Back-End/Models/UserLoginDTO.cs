namespace Back_End.Models;

public class UserLoginDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int Elo { get; set; }
    public byte[] Image { get; set; }
    public UserLoginDTO()
    {
        
    }
    public UserLoginDTO(int id, string username, int elo, byte[] image)
    {
        Id = id;
        Username = username;
        Elo = elo;
        Image = image;
    }

}
