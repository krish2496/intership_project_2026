using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Core.DTOs.Clubs;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClubController : ControllerBase
{
    private readonly IClubService _clubService;

    public ClubController(IClubService clubService)
    {
        _clubService = clubService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("UserId");
        if (claim == null) throw new UnauthorizedAccessException("User ID not found in token");
        return int.Parse(claim.Value);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ClubDto>>> GetAll()
    {
        return Ok(await _clubService.GetAllClubsAsync());
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ClubDto>> Get(int id)
    {
        try
        {
            return Ok(await _clubService.GetClubByIdAsync(id));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ClubDto>> Create(CreateClubDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = await _clubService.CreateClubAsync(userId, dto);
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
            var success = await _clubService.DeleteClubAsync(userId, id);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
