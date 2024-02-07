using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services;

public interface IActiveGames
{
    public void StartGame(IGameLobby lobby);
    public void EnsureThatPlayerIsNotInGame(PlayerInfo player);
    public void DiceThrown(PlayerInfo player);
    public void MovePlayed(PlayerInfo player, PlayerMove move);
    public Task ReCreateGame(string id, List<PlayerInfo> players);
}

