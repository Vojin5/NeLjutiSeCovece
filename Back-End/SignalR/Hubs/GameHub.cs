using Back_End.SignalR.Models;
using Back_End.SignalR.Services;
using Microsoft.AspNetCore.SignalR;

namespace Back_End.SignalR.Hubs;

public class GameHub : Hub
{
    private IGameLobby _lobby;
    private IOnlinePlayers _players;
    private IActiveGames _games;
    private static object _gameLock = new();

    public GameHub(IGameLobby lobby, IOnlinePlayers players, IActiveGames games)
    {
        _lobby = lobby;
        _players = players;
        _games = games;
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

    //Kada igrac baci kockicu koju je dobio od servera
    public void DiceThrown(int gameId)
    {
        lock (_gameLock)
        {
            _games.DiceThrown(gameId);
        }
        
    }

    //Kada igrac odigra jedan od mogucih poteza
    public void MovePlayed(int gameId, PlayerMove move)
    {
        lock (_gameLock)
        {
            _games.MovePlayed(gameId, move);
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
            _lobby.UpdateLobby();
        }

        return base.OnDisconnectedAsync(exception);
    }

}
