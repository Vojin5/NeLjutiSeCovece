namespace Back_End.SignalR.Models;
public class PlayerLobby
{
    public PlayerLobby(string username, string avatar)
    {
        Username = username;
        Avatar = avatar;
    }   
    public string Username { get; set; }
    public string Avatar { get; set; }
}
