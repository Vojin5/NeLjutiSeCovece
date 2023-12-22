namespace Back_End.SignalR.Models;

public class PlayerGameInfo
{
    public string Username { get; set; }
    public string Avatar { get; set; }
    public Color Color { get; set; }

    public PlayerGameInfo(string username, string avatar, Color color)
    {
        Username = username;
        Avatar = avatar;
        Color = color;
    }
}
