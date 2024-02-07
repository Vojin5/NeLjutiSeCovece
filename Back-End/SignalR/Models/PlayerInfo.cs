using Back_End.SignalR.Models;

public class PlayerInfo
{
    public PlayerInfo()
    {
        InLobby = InGame = false;
    }
    public PlayerInfo(string connectionId)
    {
        ConnectionId = connectionId;
    }
    public int Id { get; set; }
    public string ConnectionId { get; set; }
    public string GameId { get; set; } = "";
    public bool InGame { get; set; }
    public bool InLobby { get; set; }
    public string Avatar { get; set; }
    public string Username { get; set; }

    public Color Color { get; set; }

    public override bool Equals(object? obj)
    {
        PlayerInfo other = obj as PlayerInfo;
        return ConnectionId == other.ConnectionId;
    }
}

