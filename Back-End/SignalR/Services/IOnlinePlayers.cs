using Back_End.SignalR.Models;
public interface IOnlinePlayers
{
    public void AddPlayer(string connectionId);
    public PlayerInfo RemovePlayer(string connectionId);
    public PlayerInfo GetPlayerInfo(string connectionId);
}

