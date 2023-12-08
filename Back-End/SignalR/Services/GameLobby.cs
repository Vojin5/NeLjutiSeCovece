using Back_End.SignalR.Hubs;
using Back_End.SignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace Back_End.SignalR.Services;
public class GameLobby : IGameLobby
{
    //members
    private List<PlayerInfo> _players = new(4);
    private LobbyUpdate _update = new();
    List<string> _playerConnectionIds = new(4);

    //props
    public bool IsFull { get => _players.Count == 4; }
    public List<PlayerInfo> Players { get => _players; }
    public int Id { get; set; }

    private readonly IHubContext<GameHub> _hubContext;

    //funcs

    public GameLobby(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public void UpdateLobby()
    {
        _players.ForEach(p =>
        {
            _playerConnectionIds.Add(p.ConnectionId);
            _update.AddPlayer(new PlayerLobby(p.Username, p.Avatar));
        });

        _hubContext.Clients.Clients(_playerConnectionIds).SendAsync("UpdateLobby", _update);
        _update.Clear();
        _playerConnectionIds.Clear();
    }
    public void AcknowledgePlayerLeft(string connectionId)
    {
        _hubContext.Clients.Client(connectionId).SendAsync("UpdateLobbyAfterLeaving");
    }

    public void EnsureThatPlayerIsNotInLobby(PlayerInfo player)
    {
        if (player.InLobby)
        {
            _players.Remove(player);
        }
    }

    public void AddPlayerToLobby(PlayerInfo player, int playerId)
    {
        player.Id = playerId;
        player.InLobby = true;
        _players.Add(player);
    }

    public void RemovePlayerFromLobby(PlayerInfo player)
    {
        _players.Remove(player);
        player.InLobby = false;
    }
}
