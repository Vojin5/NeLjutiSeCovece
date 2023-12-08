namespace Back_End.SignalR.Models;
public class LobbyUpdate
{
    private List<PlayerLobby> _players = new(4);
    public List<PlayerLobby> Lobby { get; set; }
    public void AddPlayer(PlayerLobby player)
    {
        _players.Add(player);
    }
    public void Clear()
    {
        _players.Clear();
    }
}

