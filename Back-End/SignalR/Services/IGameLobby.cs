using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services
{
    public interface IGameLobby
    {
        public bool IsFull { get; }
        public List<PlayerInfo> Players { get; }
        public int Id { get; set; }

        public void AddPlayerToLobby(PlayerInfo player, int playerId);
        public void UpdateLobby();
        public void AcknowledgePlayerLeft(string connectionId);
        public void EnsureThatPlayerIsNotInLobby(PlayerInfo player);
        public void RemovePlayerFromLobby(PlayerInfo player);
        public void CreateLobby();
    }
}
