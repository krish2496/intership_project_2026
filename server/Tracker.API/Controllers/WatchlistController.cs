using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Core.DTOs.Watchlist;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WatchlistController : ControllerBase
{
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(IWatchlistService watchlistService)
    {
        _watchlistService = watchlistService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("UserId");
        if (claim == null) throw new UnauthorizedAccessException("User ID not found in token");
        return int.Parse(claim.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WatchlistDto>>> GetMyWatchlist()
    {
        var userId = GetUserId();
        var list = await _watchlistService.GetUserWatchlistAsync(userId);
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<WatchlistDto>> Add(AddToWatchlistDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _watchlistService.AddToWatchlistAsync(userId, dto);
            return CreatedAtAction(nameof(GetMyWatchlist), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WatchlistDto>> Update(int id, UpdateWatchlistDto dto)
    {
        var userId = GetUserId();
        var result = await _watchlistService.UpdateWatchlistAsync(userId, id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var success = await _watchlistService.RemoveFromWatchlistAsync(userId, id);
        if (!success) return NotFound();
        return NoContent();
    }
}
