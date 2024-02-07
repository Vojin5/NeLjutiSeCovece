using Back_End.Models;
using Back_End.SignalR.Models;
using Back_End.SignalR.Services;
using Microsoft.AspNetCore.SignalR;

namespace Back_End.SignalR.Hubs;

public class GameHub : Hub
{
    private IGameLobby _lobby;
    private IOnlinePlayers _players;
    private IActiveGames _games;
    private IPendingGames _pendingGames;

    private static object _gameLock = new();
    private static SemaphoreSlim _gameSemaphore = new SemaphoreSlim(1, 1);

    public GameHub(IGameLobby lobby, IOnlinePlayers players, IActiveGames games, IPendingGames pendingGames)
    {
        _lobby = lobby;
        _players = players;
        _games = games;
        _pendingGames = pendingGames;
    }
    public void JoinLobby()
    {
        lock (_gameLock)
        {
            _lobby.AddPlayerToLobby(_players.GetPlayerInfo(Context.ConnectionId));
            _lobby.UpdateLobby();

            if (_lobby.IsFull)
            {
                _games.StartGame(_lobby);
            }
        }
    }

    public void LeaveLobby()
    {
        lock (_gameLock)
        {
            _lobby.RemovePlayerFromLobby(_players.GetPlayerInfo(Context.ConnectionId));
            _lobby.UpdateLobby();
        }
    }

    public void SendMyInfo(int playerId, string avatar, string username)
    {
        lock (_gameLock)
        {
            PlayerInfo player = _players.GetPlayerInfo(Context.ConnectionId);
            player.Id = playerId;
            player.Avatar = avatar;
            player.Username = username;
        }
    }

    public void DiceThrown()
    {
        lock (_gameLock)
        {
            _games.DiceThrown(_players.GetPlayerInfo(Context.ConnectionId));
        }
    }

    public void MovePlayed(PlayerMove move)
    {
        lock (_gameLock)
        {
            _games.MovePlayed(_players.GetPlayerInfo(Context.ConnectionId), move);
        }
    }

    public async Task ReJoinMatch(string gameKey)
    {
        await _gameSemaphore.WaitAsync();
        try
        {
            var pendingGame = _pendingGames.GetPendingGame(gameKey);
            if (pendingGame != null)
            {
                pendingGame.AddPlayer(_players.GetPlayerInfo(Context.ConnectionId));
                if (pendingGame.Full) await _games.ReCreateGame(gameKey, pendingGame.Players);
                return;
            }

            _pendingGames.Create(gameKey, _players.GetPlayerInfo(Context.ConnectionId));
        }
        finally
        {
            _gameSemaphore.Release();
        }

    }

    //izvrsavaju se kada klijent udje/izadje
    public override Task OnConnectedAsync()
    {
        lock (_gameLock)
        {
            _players.AddPlayer(Context.ConnectionId);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        lock (_gameLock)
        {
            PlayerInfo player = _players.RemovePlayer(Context.ConnectionId);
            _lobby.EnsureThatPlayerIsNotInLobby(player);
            _games.EnsureThatPlayerIsNotInGame(player);
            if (player.GameId != String.Empty)
                _pendingGames.Remove(player.GameId);
            
        }

        return base.OnDisconnectedAsync(exception);
    }

}
