﻿using Back_End.SignalR.Models;
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
    public void JoinLobby(int playerId)
    {
        lock (_gameLock)
        {
            _lobby.AddPlayerToLobby(_players.GetPlayerInfo(Context.ConnectionId), playerId);
            _lobby.UpdateLobby();
        }
    }

    public void LeaveLobby()
    {
        lock (_gameLock)
        {
            _lobby.RemovePlayerFromLobby(_players.GetPlayerInfo(Context.ConnectionId));
            _lobby.AcknowledgePlayerLeft(Context.ConnectionId);
            _lobby.UpdateLobby();
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
            PlayerInfo player = _players.RemovePlayer(Context.ConnectionId);
            _lobby.EnsureThatPlayerIsNotInLobby(player);
            _lobby.UpdateLobby();
        }

        return base.OnDisconnectedAsync(exception);
    }

}