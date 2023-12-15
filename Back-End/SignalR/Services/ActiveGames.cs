using System.Collections.Concurrent;
using Back_End.SignalR.Hubs;
using Back_End.SignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace Back_End.SignalR.Services;
public class ActiveGames : IActiveGames
{
    private Dictionary<int, GameState> _activeGames = new();
    private readonly IHubContext<GameHub> _hubContext;

    public ActiveGames(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext; 
    }
    public void StartGame(IGameLobby lobby)
    {
        GameState game = new(lobby.Players);
        _activeGames.Add(game.Id, game);

        
        List<string> connectionIds = new(4);
        lobby.Players.ForEach(p => connectionIds.Add(p.ConnectionId));
        _hubContext.Clients.Clients(connectionIds).SendAsync("GameStart", game.Id);
        _hubContext.Clients.Clients(connectionIds).SendAsync("NextPlayer", game.NextPlayer);
        lobby.Clear();
    }

    public void RemovePlayerFromGame(PlayerInfo player)
    {
        //var game = _activeGames[player.GameId];
        //game.Remove(player);
        //player.InGame = false;
    }

    public void DiceThrown(int gameId)
    {
        //Izmeniti generisanje broja kockice kasnije
        GameState game = _activeGames[gameId];
        int diceNum = new Random().Next(7);
        if (diceNum == 0)
        {
            diceNum = 1;
        }

        List<string> connectionIds = new(4);
        game.Players.ForEach(p => connectionIds.Add(p.ConnectionId));
        Console.WriteLine(game.Players.Count.ToString());
        _hubContext.Clients.Clients(connectionIds).SendAsync("DiceArrived", diceNum);
        _hubContext.Clients.Clients(connectionIds).SendAsync("NextPlayer", game.NextPlayer);
    }
}

