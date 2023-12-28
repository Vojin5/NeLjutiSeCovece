namespace Back_End.Models;

public class MatchHistoryUser
{
    public int UserId { get; set; }
    public int Points { get; set; }

    public MatchHistoryUser()
    {
        
    }

    public MatchHistoryUser(int userId, int points)
    {
        UserId = userId;
        Points = points;
    }
}
