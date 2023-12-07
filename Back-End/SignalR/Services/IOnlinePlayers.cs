using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services
{
    public interface IOnlinePlayers
    {
        public void AddPlayer(string connectionId);
        public PlayerInfo RemovePlayer(string connectionId);
        public PlayerInfo GetPlayerInfo(string connectionId);
    }
}
