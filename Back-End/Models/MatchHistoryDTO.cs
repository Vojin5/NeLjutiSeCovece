namespace Back_End.Models;

public class MatchHistoryDTO
{
    public int MatchId { get; set; }
    public IEnumerable<UserMatchDTO> Users { get; set; }
    public MatchHistoryDTO()
    {
        
    }
    public MatchHistoryDTO(int matchId, IEnumerable<UserMatchDTO> users)
    {
        MatchId = matchId;
        Users = users;
    }
}
