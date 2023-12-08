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
    public int GameId { get; set; }
    public bool InGame { get; set; }
    public bool InLobby { get; set; }
    public string Avatar { get; set; }
    public string Username { get; set; }

    public override bool Equals(object? obj)
    {
        PlayerInfo other = obj as PlayerInfo;
        return Id == other.Id;
    }
}

