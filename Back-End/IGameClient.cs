using Back_End.SignalR.Models;

namespace Back_End;
public interface IGameClient
{
    public void UpdateLobby(PlayerLobby info);
    public void DiceNumber();

}

