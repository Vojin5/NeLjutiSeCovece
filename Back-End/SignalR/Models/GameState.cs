namespace Back_End.SignalR.Models;

public class GameState
{
    //members
    private List<PlayerInfo> _players;
    private static int IdGenerator = 0;
    private int nextPlayer = 0;

    //props
    public List<PlayerInfo> Players { get => _players; set => _players = value; }
    public int Id { get; set; } = IdGenerator++;
    public int NextPlayer { get => _players[(nextPlayer++ % 4)].Id; }

    public GameState(List<PlayerInfo> players)
    {
        _players = players;
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Color = (Color)i;
        }
    }
}

public enum Color
{
    YELLOW,
    GREEN, 
    BLUE,
    RED
}

