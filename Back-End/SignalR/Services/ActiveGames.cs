using System.Collections.Concurrent;
using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services;
public class ActiveGames : IActiveGames
{
    private Dictionary<int, List<PlayerInfo>> _activeGames = new();
    public void AddGame(int gameId, List<PlayerInfo> players)
    {
        players.ForEach(p =>
        {
            p.InGame = true;
            p.InLobby = false;
        });
        _activeGames.Add(gameId, players);
    }

    public void RemovePlayerFromGame(PlayerInfo player)
    {
        var game = _activeGames[player.GameId];
        game.Remove(player);
        player.InGame = false;
    }
}

