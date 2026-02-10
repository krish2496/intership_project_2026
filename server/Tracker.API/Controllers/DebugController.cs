using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tracker.Infrastructure.Data;

namespace Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly TrackerDbContext _context;

    public DebugController(TrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users
            .Select(u => new 
            {
                u.Id,
                u.Username,
                u.Email,
                WatchlistCount = u.Watchlist.Count
            })
            .ToListAsync();

        return Ok(new 
        {
            TotalUsers = users.Count,
            Users = users
        });
    }

    [HttpGet("follows")]
    public async Task<IActionResult> GetAllFollows()
    {
        var follows = await _context.Follows
            .Include(f => f.Follower)
            .Include(f => f.Following)
            .Select(f => new 
            {
                FollowerId = f.FollowerId,
                FollowerName = f.Follower.Username,
                FollowingId = f.FollowingId,
                FollowingName = f.Following.Username
            })
            .ToListAsync();

        return Ok(new 
        {
            TotalFollows = follows.Count,
            Follows = follows
        });
    }
}
