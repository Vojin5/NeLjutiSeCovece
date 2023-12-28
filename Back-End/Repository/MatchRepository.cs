using Back_End.IRepository;
using Back_End.Models;

namespace Back_End.Repository;

public class MatchRepository : IMatchRepository
{
    public Context Context { get; set; }
    public MatchRepository(Context context)
    {
        Context = context;
    }
    public async Task AddMatch(Match match)
    {
        await Context.Matches
            .AddAsync(match);
        await Context.SaveChangesAsync();
    }
}
