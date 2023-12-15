namespace Back_End.SignalR.Models;

public class GameStartInfo
{
    public int GameId { get; set; }
    public List<PlayerGameInfo> Players { get; set; } = new();

    public void AddPlayersInfo(int gameId, List<PlayerInfo> players)
    {
        GameId = gameId;
        players.ForEach(p => Players.Add(new(p.Username, p.Avatar, p.Color)));
    }
}
