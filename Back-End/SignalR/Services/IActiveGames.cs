using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services;

public interface IActiveGames
{
    public Task StartGame(IGameLobby lobby);
    public void EnsureThatPlayerIsNotInGame(PlayerInfo player);
    public Task DiceThrown(PlayerInfo player);
    public void MovePlayed(PlayerInfo player, PlayerMove move);
    public Task ReCreateGame(string id, List<PlayerInfo> players);
}

