public class PlayerInfo
{
    public PlayerInfo()
    {
        InLobby = InGame = false;
    }
    public int Id { get; set; }
    public int GameId { get; set; }
    public bool InGame { get; set; }
    public bool InLobby { get; set; }

    public override bool Equals(object? obj)
    {
        PlayerInfo other = obj as PlayerInfo;
        return Id == other.Id;
    }
}

