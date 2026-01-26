using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Core.DTOs.Discussions;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DiscussionController : ControllerBase
{
    private readonly IDiscussionService _discussionService;

    public DiscussionController(IDiscussionService discussionService)
    {
        _discussionService = discussionService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("UserId");
        if (claim == null) throw new UnauthorizedAccessException("User ID not found in token");
        return int.Parse(claim.Value);
    }

    [HttpGet("club/{clubId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<DiscussionDto>>> GetByClub(int clubId)
    {
        return Ok(await _discussionService.GetClubDiscussionsAsync(clubId));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<DiscussionDto>> Get(int id)
    {
        try
        {
            return Ok(await _discussionService.GetDiscussionByIdAsync(id));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("club/{clubId}")]
    public async Task<ActionResult<DiscussionDto>> Create(int clubId, CreateDiscussionDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _discussionService.CreateDiscussionAsync(userId, clubId, dto);
            // This is slightly awkward route for created at action but ok for now
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _discussionService.DeleteDiscussionAsync(userId, id);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
