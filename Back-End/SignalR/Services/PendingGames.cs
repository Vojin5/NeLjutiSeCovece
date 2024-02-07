using Back_End.Models;

namespace Back_End.SignalR.Services;

public class PendingGames : IPendingGames
{
    private Dictionary<string, PendingGame> _pendingGames = new();
    public PendingGame GetPendingGame(string gameKey)
    {
        PendingGame pendingGame;
        _pendingGames.TryGetValue(gameKey, out pendingGame);
        return pendingGame;
    }

    public void Create(string gameKey, PlayerInfo player)
    {
        var pendingGame = new PendingGame();
        pendingGame.AddPlayer(player);

        _pendingGames.Add(gameKey, pendingGame);
    }

    public void Remove(string gameKey)
    {
        _pendingGames.Remove(gameKey);
    }

}

public class PendingGame 
{
    public string GameKey { get; set; }
    public string State { get; set; }
    public List<PlayerInfo> Players { get; set; }

    public bool Full { get => Players.Count == 4; }

    public void AddPlayer(PlayerInfo player)
    {
        Players.Add(player);
    }

    public PendingGame()
    {
        Players = new();
    }
}

