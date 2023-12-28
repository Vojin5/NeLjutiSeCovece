using Back_End.IRepository;
using Back_End.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchHistoryController : ControllerBase
{
    public IMatchHistoryRepository MatchHistoryRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IMatchRepository MatchRepository { get; set; }
    public MatchHistoryController(
        IMatchHistoryRepository matchHistoryRepository,
        IUserRepository userRepository,
        IMatchRepository matchRepository)
    {
        MatchHistoryRepository = matchHistoryRepository;
        UserRepository = userRepository;
        MatchRepository = matchRepository;

    }

    [HttpPost]
    public async Task<ActionResult> AddMatch([FromBody] List<MatchHistoryUser> users)
    {
        List<User?> usersToFind = new List<User?>();
        for (int i = 0; i < users.Count; i++)
        {
            usersToFind.Add(await UserRepository.GetUserByIdAsync(users.ElementAt(i).UserId));
        }

        List<MatchHistory> matchHistories = new List<MatchHistory>();

        Match match = new Match();

        await MatchRepository.AddMatch(match);

        for (int i = 0; i < usersToFind.Count; i++)
        {
            matchHistories.Add(new MatchHistory(usersToFind.ElementAt(i), match, users.ElementAt(i).Points));
        }

        await MatchHistoryRepository.AddMatchesAsync(matchHistories);

        return Ok();
    }

    [HttpGet("mymatchhistory/{userId}")]
    public async Task<ActionResult> GetUserMatchHistory([FromRoute] int userId)
    {
        return Ok(await MatchHistoryRepository.GetUserMatchHistoryAsync(userId));
    }

}
