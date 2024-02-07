using Back_End.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Controllers;

[ApiController]
[Route("[controller]")]
public class UnfinishedGameController : ControllerBase
{
    public Context Context { get; set; }

    public UnfinishedGameController(Context context)
    {
        Context = context;
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddUnfinishedGame([FromBody]UnfinishedGameInfo ugi)
    {
        var existingUG = await Context.UnfinishedGames
            .Include(ug => ug.GameState)
            .Where(ug => ug.GameState.GameKey == ugi.GameKey)
            .FirstOrDefaultAsync();

        if (existingUG != null)
        {
            existingUG.GameState.State = ugi.State;
            await Context.SaveChangesAsync();
            return Ok();
        }

        var users = await Context.Users
            .Where(u => ugi.PlayerIds.Contains(u.Id))
            .ToListAsync();

        GameState gameState = new GameState();
        gameState.GameKey = ugi.GameKey;
        gameState.State = ugi.State;
        Context.GameStates.Add(gameState);

        users.ForEach(u =>
        {
            var newUG = new UnfinishedGame();
            newUG.User = u;
            newUG.GameState = gameState;
            Context.UnfinishedGames.Add(newUG);
        });

        await Context.SaveChangesAsync();

        return Ok();
        
    }

    [HttpGet("my-games/{userId}")]
    public async Task<ActionResult> GetUserUnifishedGames([FromRoute]int userId)
    {
        var ugList = await Context.UnfinishedGames
            .Include(ug => ug.User)
            .Include(ug => ug.GameState)
            .Where(ug => ug.User.Id == userId)
            .Select(ug => new {ug.GameState.GameKey})
            .ToListAsync();

        return Ok(ugList);
    }

    [HttpGet("game-state/{gameKey}")]
    public async Task<ActionResult> GetGameState([FromRoute]string gameKey)
    {
        var gameState = await Context.GameStates.Where(gs => gs.GameKey == gameKey).Select(gs => gs.State).FirstOrDefaultAsync();
        return Ok(gameState);
    }

    [HttpDelete("remove/{gameKey}")]
    public async Task <ActionResult> RemoveUnfinishedGame([FromRoute]string gameKey)
    {
        var gameState = await Context.GameStates.Where(gs => gs.GameKey == gameKey).FirstOrDefaultAsync();

        var unfinishedGames = await Context.UnfinishedGames.Include(uf => uf.GameState).Where(uf => uf.GameState.GameKey == gameKey).ToListAsync();
       

        Context.UnfinishedGames.RemoveRange(unfinishedGames);
        Context.GameStates.Remove(gameState);

        await Context.SaveChangesAsync();

        return Ok();
    }
}
