using Back_End.Models;

namespace Back_End.IRepository;

public interface IMatchHistoryRepository
{
    public Task<List<MatchHistoryDTO?>> GetUserMatchHistoryAsync(int userId);
    public Task AddMatchesAsync(List<MatchHistory> matchHistories);
}
