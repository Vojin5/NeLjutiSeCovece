using Back_End.SignalR.Models;

namespace Back_End.SignalR.Services;
public class OnlinePlayers : IOnlinePlayers
{
    private Dictionary<string, PlayerInfo> _onlinePlayers = new();
    public void AddPlayer(string connectionId)
    {
        _onlinePlayers.Add(connectionId, new PlayerInfo(connectionId));
    }

    public PlayerInfo RemovePlayer(string connectionId)
    {
        PlayerInfo player;
        _onlinePlayers.Remove(connectionId, out player);
        return player;
    }
    public PlayerInfo GetPlayerInfo(string connectionId)
    {
        return _onlinePlayers[connectionId];
    }
}



