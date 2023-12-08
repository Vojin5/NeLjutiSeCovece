using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services;
public class GameLobby : IGameLobby
{
    //members
    private List<PlayerInfo> _players = new(4);

    //props
    public bool IsFull { get => _players.Count == 4; }
    public List<PlayerInfo> Players { get => _players; }
    public int Id { get; set; }

    //funcs

    public void AddPlayerToLobby(PlayerInfo player)
    {
        _players.Add(player);
        player.InLobby = true;
    }

    public void RemovePlayerFromLobby(PlayerInfo player)
    {
        _players.Remove(player);
        player.InLobby = false;
    }
    public void CreateLobby()
    {
        _players = new List<PlayerInfo>();
        Id++;
    }
}
