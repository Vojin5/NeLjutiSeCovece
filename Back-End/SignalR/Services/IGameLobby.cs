using Back_End.SignalR.Models;


public interface IGameLobby
{
    public bool IsFull { get; }
    public List<PlayerInfo> Players { get; }
    public int Id { get; set; }
    public void AddPlayerToLobby(PlayerInfo player);
    public void UpdateLobby();
    public void EnsureThatPlayerIsNotInLobby(PlayerInfo player);
    public void RemovePlayerFromLobby(PlayerInfo player);
    public void Clear();
}

