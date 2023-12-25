using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services;

public interface IActiveGames
{
    public void StartGame(IGameLobby lobby);
    public void RemovePlayerFromGame(PlayerInfo player);
    public void DiceThrown(int gameId, string connectionId);
    public void MovePlayed(int gameId, PlayerMove move);
}

