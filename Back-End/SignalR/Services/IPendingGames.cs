using Back_End.Models;

namespace Back_End.SignalR.Services;

public interface IPendingGames
{
    PendingGame GetPendingGame(string gameKey);
    void Create(string gameKey, PlayerInfo player);
    void Remove(string gameKey);
}
