using Back_End.Models;

namespace Back_End.IRepository;

public interface IMatchRepository
{
    public Task AddMatch(Match match);
}
