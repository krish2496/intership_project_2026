using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Core.DTOs.Polls;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PollController : ControllerBase
{
    private readonly IPollService _pollService;

    public PollController(IPollService pollService)
    {
        _pollService = pollService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("UserId");
        if (claim == null) throw new UnauthorizedAccessException("User ID not found in token");
        return int.Parse(claim.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PollDto>>> GetActivePolls([FromQuery] int? clubId)
    {
        try
        {
            var userId = GetUserId();
            var polls = await _pollService.GetActivePollsWithUserAsync(clubId, userId);
            return Ok(polls);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<PollDto>> Create(CreatePollDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _pollService.CreatePollAsync(userId, dto);
            return CreatedAtAction(nameof(GetActivePolls), null, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/vote")]
    public async Task<ActionResult<PollDto>> Vote(int id, VoteDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _pollService.VoteAsync(userId, id, dto);
            return Ok(result);
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
            var success = await _pollService.DeletePollAsync(userId, id);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
