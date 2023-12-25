using Back_End.SignalR.Models;
namespace Back_End.SignalR;
public interface IGameClient
{
    public void UpdateLobby(LobbyUpdate update);
}

