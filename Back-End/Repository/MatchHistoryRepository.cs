using Back_End.IRepository;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Repository;

public class MatchHistoryRepository : IMatchHistoryRepository
{
    public Context Context { get; set; }
    public MatchHistoryRepository(Context context)
    {
        Context = context;
    }
    public async Task<List<MatchHistoryDTO?>> GetUserMatchHistoryAsync(int userId)
    {
        var matches = await Context.PlayedMatches
            .Include(pm => pm.Match)
            .Include(pm => pm.User)
            .GroupBy(pm => pm.Match.Id)
            .Where(group => group.Any(pm => pm.User.Id == userId))
            .Select(group => new MatchHistoryDTO
            {
                MatchId = group.Key,
                Users = group.Select(pm => new UserMatchDTO
                {
                    Username = pm.User.Username,
                    Image = pm.User.Image,
                    Points = pm.Points
                })
            })
            .ToListAsync();

        return matches;
    }

    public async Task AddMatchesAsync(List<MatchHistory> matchHistories)
    {
        await Context.PlayedMatches
            .AddRangeAsync(matchHistories);
        await Context.SaveChangesAsync();
    }
}
