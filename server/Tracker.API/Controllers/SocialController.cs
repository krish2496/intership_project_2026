using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SocialController : ControllerBase
{
    private readonly ISocialService _socialService;

    public SocialController(ISocialService socialService)
    {
        _socialService = socialService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("UserId");
        if (claim == null) throw new UnauthorizedAccessException("User ID not found in token");
        return int.Parse(claim.Value);
    }

    [HttpPost("follow/{followingId}")]
    public async Task<IActionResult> Follow(int followingId)
    {
        var followerId = GetUserId();
        var result = await _socialService.FollowUserAsync(followerId, followingId);
        if (!result) return BadRequest("Unable to follow user (already following or invalid).");
        return Ok(new { message = "Followed successfully" });
    }

    [HttpPost("unfollow/{followingId}")]
    public async Task<IActionResult> Unfollow(int followingId)
    {
        var followerId = GetUserId();
        var result = await _socialService.UnfollowUserAsync(followerId, followingId);
        if (!result) return BadRequest("Unable to unfollow user.");
        return Ok(new { message = "Unfollowed successfully" });
    }

    [HttpGet("followers/{userId}")]
    public async Task<IActionResult> GetFollowers(int userId)
    {
        var followers = await _socialService.GetFollowersAsync(userId);
        return Ok(followers);
    }

    [HttpGet("following/{userId}")]
    public async Task<IActionResult> GetFollowing(int userId)
    {
        var following = await _socialService.GetFollowingAsync(userId);
        return Ok(following);
    }

    [HttpGet("profile/{userId}")]
    public async Task<IActionResult> GetPublicProfile(int userId)
    {
        try
        {
            var profile = await _socialService.GetPublicProfileAsync(userId);
            if (profile == null) 
            {
                Console.WriteLine($"User with ID {userId} not found");
                return NotFound("User not found");
            }
            return Ok(profile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching profile for user {userId}: {ex.Message}");
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
