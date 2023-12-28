namespace Back_End.Models;

public class Match
{
    public int Id { get; set; }
    public List<MatchHistory>? Players { get; set; }
}
