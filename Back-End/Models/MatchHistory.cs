namespace Back_End.Models;

public class MatchHistory
{
    public int Id { get; set; }

    public int Points { get; set; }
    public User User { get; set; }
    public Match Match { get; set; }
    public MatchHistory() { }

    public MatchHistory(User user, Match match, int points)
    {
        User = user;
        Match = match;
        Points = points;
    }
}
