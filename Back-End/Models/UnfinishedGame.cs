namespace Back_End.Models;

public class UnfinishedGame
{
    public int Id { get; set; }
    public User? User { get; set; }
    public GameState? GameState { get; set; }

    public UnfinishedGame()
    {
        
    }
}

public class GameState
{
    public int Id { get; set; }
    public string GameKey { get; set; }
    public string State { get; set; }

    //navigacije
    public List<UnfinishedGame>? UnfinishedGames { get; set; }

    public GameState()
    {
        
    }
}

public class UnfinishedGameInfo
{
    public string GameKey { get; set; }
    public string State { get; set; }
    public List<int> PlayerIds { get; set; }
    public UnfinishedGameInfo() { }
}
