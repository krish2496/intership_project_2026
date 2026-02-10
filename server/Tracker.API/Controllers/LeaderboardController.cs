using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tracker.Infrastructure.Data;

namespace Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly TrackerDbContext _context;

    public LeaderboardController(TrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet("top-watchers")]
    public async Task<IActionResult> GetTopWatchers()
    {
        var leaderboard = await _context.Users
            .Select(u => new 
            {
                u.Id,
                u.Username,
                TotalEpisodes = u.Watchlist.Sum(w => w.Progress),
                AnimeCompleted = u.Watchlist.Count(w => w.Status == Tracker.Core.Enums.WatchStatus.Completed)
            })
            .OrderByDescending(x => x.TotalEpisodes)
            .Take(50)
            .ToListAsync();

        return Ok(leaderboard);
    }
}
