namespace Back_End.Models;

public class UserMatchDTO
{
    public string Username { get; set; }
    public int Points { get; set; }
    public byte[] Image { get; set; }
    public UserMatchDTO()
    {
        
    }
    public UserMatchDTO(string username, int points, byte[] image)
    {
        Username = username;
        Points = points;
        Image = image;
    }
}
