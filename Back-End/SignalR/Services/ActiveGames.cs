using Back_End.Controllers;
using Back_End.SignalR.Hubs;
using Back_End.SignalR.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = Back_End.SignalR.Models.Action;

namespace Back_End.SignalR.Services;
public class ActiveGames : IActiveGames
{
    private Dictionary<string, GameState> _activeGames = new();
    private readonly IHubContext<GameHub> _hubContext;

    public ActiveGames(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task StartGame(IGameLobby lobby)
    {
        string gameId = Guid.NewGuid().ToString();
        GameState game = new(lobby.Players, gameId);
        _activeGames.Add(gameId, game);


        List<string> connectionIds = new(4);
        lobby.Players.ForEach(p => connectionIds.Add(p.ConnectionId));
        await _hubContext.Clients.Clients(connectionIds).SendAsync("handleGameStart", game.Id);
        await _hubContext.Clients.Clients(game.Players[game.CurrentPlayerTurn].ConnectionId).SendAsync("handleMyTurn");
        lobby.Clear();
    }
    
    public void EnsureThatPlayerIsNotInGame(PlayerInfo player)
    {
        if (!player.InGame) return;
        _activeGames[player.GameId].HandlePlayerLeaving();
        _activeGames[player.GameId].Players.ForEach(p => p.InGame = false);
        _activeGames.Remove(player.GameId);
    }

    public async Task DiceThrown(PlayerInfo player)
    {
        GameState game = _activeGames[player.GameId];

        if (!game.CheckIfPlayerValid(player.ConnectionId)) return;

        List<string> connectionIds = new(4);
        game.Players.ForEach(p => connectionIds.Add(p.ConnectionId));

        await _hubContext.Clients.Clients(connectionIds).SendAsync("handleStartDiceAnimation");


        int diceNum = new Random().Next(7);
        if (diceNum == 0)
        {
            diceNum = 1;
        }

        diceNum = 6;
        await _hubContext.Clients.Clients(connectionIds).SendAsync("handleDiceNumber", diceNum);


        List<PlayerMove> moves = game.GeneratePossiblePlayerMoves(diceNum);

        if (moves.Count == 0)
        {
            await _hubContext.Clients.Clients(game.Players[game.NextPlayerTurnId].ConnectionId).SendAsync("handleMyTurn");
            return;
        }

        await _hubContext.Clients.Client(game.Players[game.CurrentPlayerTurn % 4].ConnectionId).SendAsync("handlePossibleMoves", moves);
    }

    public async void MovePlayed(PlayerInfo player, PlayerMove move)
    {
        GameState game = _activeGames[player.GameId];

        List<string> connectionIds = new(4);
        game.Players.ForEach(p => connectionIds.Add(p.ConnectionId));

        Action action = game.UpdateGameState(move);
        if (game.IsGameOver())
        {
            game.GameOverNotifyPlayers(_hubContext);
            HttpClient client = new();
            await client.DeleteAsync($"http://localhost:5295/UnfinishedGame/remove/{player.GameId}");
            return;
        }
        await _hubContext.Clients.Clients(connectionIds).SendAsync("handlePlayerMove", action);
        await _hubContext.Clients.Clients(game.Players[game.NextPlayerTurnId].ConnectionId).SendAsync("handleMyTurn");

    }

    public async Task ReCreateGame(string gameKey, List<PlayerInfo> players)
    {
        var client = new HttpClient();
        var res = await client.GetAsync($"http://127.0.0.1:5295/UnfinishedGame/game-state/{gameKey}");
        var content = await res.Content.ReadAsStringAsync();
        var contentJSON = JsonConvert.DeserializeObject<JObject>(content);

        GameState game = new();
        game.Id = gameKey;
        players.ForEach(p =>
        {
            p.GameId = gameKey;
            p.InGame = true;
        });


        game.CurrentPlayerTurn = (int)contentJSON!["currentPlayerTurnId"]!;
        var gameState = game.ReCreateState(contentJSON, players);
        _activeGames.Add(gameKey, game);
        

        List<string> connectionIds = players.Select(p => p.ConnectionId).ToList();
        await _hubContext.Clients
            .Clients(connectionIds)
            .SendAsync("handleReCreationOfGameState", gameKey, gameState, _activeGames[gameKey].Players.Select(p => new { p.Avatar, p.Username }));
        await _hubContext.Clients.Client(players[game.CurrentPlayerTurn].ConnectionId).SendAsync("handleMyTurn");
    }
}

