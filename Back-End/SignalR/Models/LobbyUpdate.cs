namespace Back_End.SignalR.Models;
public class LobbyUpdate
{
    private LinkedList<PlayerLobby> _players = new LinkedList<PlayerLobby>();
    public LinkedList<PlayerLobby> Lobby { get => _players; }
    public void AddPlayer(PlayerLobby player)
    {
        _players.AddLast(player);
    }
    public void Clear()
    {
        _players.Clear();
    }
}

