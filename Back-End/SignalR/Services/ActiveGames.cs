using Back_End.SignalR.Hubs;
using Back_End.SignalR.Models;
using Microsoft.AspNetCore.SignalR;
using Action = Back_End.SignalR.Models.Action;

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
        _hubContext.Clients.Clients(connectionIds).SendAsync("handleGameStart", game.Id);
        _hubContext.Clients.Clients(game.Players[game.CurrentPlayerTurn % 4].ConnectionId).SendAsync("handleMyTurn");
        lobby.Clear();
    }

    public void RemovePlayerFromGame(PlayerInfo player)
    {
        //var game = _activeGames[player.GameId];
        //game.Remove(player);
        //player.InGame = false;
    }


    public void DiceThrown(int gameId, string connectionId)
    {
        
        GameState game = _activeGames[gameId];

        //Za slucaj da pokusa da baca neko ko nije na redu
        if (!game.CheckIfPlayerValid(connectionId)) return;

        List<string> connectionIds = new(4);
        game.Players.ForEach(p => connectionIds.Add(p.ConnectionId));

        _hubContext.Clients.Clients(connectionIds).SendAsync("handleStartDiceAnimation");

        //Izmeniti generisanje broja kockice kasnije

        int diceNum = new Random().Next(7);
        if (diceNum == 0)
        {
            diceNum = 1;
        }
        if (diceNum == 4 || diceNum == 5) diceNum = 6;

        diceNum = 6;
        _hubContext.Clients.Clients(connectionIds).SendAsync("handleDiceNumber", diceNum);
        

        List<PlayerMove> moves = game.GeneratePossiblePlayerMoves(diceNum);
        Console.WriteLine("BROJ MOGUCIH POTEZA JE " + moves.Count);
        if (moves.Count == 0)
        {
            _hubContext.Clients.Clients(game.Players[game.NextPlayerTurnId].ConnectionId).SendAsync("handleMyTurn");
            return;
        }

        _hubContext.Clients.Client(game.Players[game.CurrentPlayerTurn % 4].ConnectionId).SendAsync("handlePossibleMoves", moves);
    }

    public void MovePlayed(int gameId, PlayerMove move)
    {
        Console.WriteLine("ODIGRAVA SE POTEZ " + move.FigureId + " " + move.NewPosition);
        GameState game = _activeGames[gameId];

        List<string> connectionIds = new(4);
        game.Players.ForEach(p => connectionIds.Add(p.ConnectionId));

        Action action = game.UpdateGameState(move);
        _hubContext.Clients.Clients(connectionIds).SendAsync("handlePlayerMove", action);
        _hubContext.Clients.Clients(game.Players[game.NextPlayerTurnId].ConnectionId).SendAsync("handleMyTurn");

    }
}

