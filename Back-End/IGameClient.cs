using Back_End.SignalR.Models;

namespace Back_End;
public interface IGameClient
{
    public void UpdateLobby(LobbyUpdate update);
}

