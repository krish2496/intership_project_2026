using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FeedController : ControllerBase
{
    private readonly IActivityService _activityService;

    public FeedController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("UserId");
        if (claim == null) throw new UnauthorizedAccessException("User ID not found in token");
        return int.Parse(claim.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyFeed()
    {
        var userId = GetUserId();
        var feed = await _activityService.GetFeedAsync(userId);
        return Ok(feed);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserActivity(int userId)
    {
        var activities = await _activityService.GetUserActivityAsync(userId);
        return Ok(activities);
    }
}
