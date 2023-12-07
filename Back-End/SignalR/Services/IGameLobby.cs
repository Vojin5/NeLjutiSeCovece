using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services
{
    public interface IGameLobby
    {
        public bool IsFull { get; }
        public List<PlayerInfo> Players { get; }
        public int Id { get; set; }

        public void AddPlayerToLobby(PlayerInfo player);
        public void RemovePlayerFromLobby(PlayerInfo player);
        public void CreateLobby();
    }
}
