using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services
{
    public interface IActiveGames
    {
        public void AddGame(int gameId, List<PlayerInfo> players);
        public void RemovePlayerFromGame(PlayerInfo player);
    }
}
