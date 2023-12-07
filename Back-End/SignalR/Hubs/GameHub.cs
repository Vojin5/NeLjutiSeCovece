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

    //Metoda koja se poziva iz klijenta
    public void JoinGame(int playerId)
    {
        lock (_gameLock)
        {
            PlayerInfo player = _players.GetPlayerInfo(Context.ConnectionId);
            player.Id = playerId;
            _lobby.AddPlayerToLobby(player);
            Clients.All.SendAsync("ShowLobby", _lobby.Players);
            if (_lobby.IsFull)
            {
                player.GameId = _lobby.Id;
                _games.AddGame(_lobby.Id, _lobby.Players);
                Clients.All.SendAsync("ShowLobby", _lobby.Players);
                _lobby.CreateLobby();
            }
        }
    }



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
            PlayerInfo player = _players.GetPlayerInfo(Context.ConnectionId);
            if (player.InLobby)
            {
                _lobby.RemovePlayerFromLobby(player);
            }
            if (player.InGame)
            {
                _games.RemovePlayerFromGame(player);
            }
            _players.RemovePlayer(Context.ConnectionId);

        }
        return base.OnDisconnectedAsync(exception);
    }

}
