using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tracker.Core.DTOs;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            throw new UnauthorizedAccessException("Invalid user ID");
        return userId;
    }

    [HttpGet]
    public async Task<ActionResult<ProfileDto>> GetProfile()
    {
        try
        {
            var userId = GetUserId();
            var profile = await _profileService.GetProfileAsync(userId);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("password")]
    public async Task<ActionResult> ChangePassword(ChangePasswordDto dto)
    {
        try
        {
            var userId = GetUserId();
            await _profileService.ChangePasswordAsync(userId, dto);
            return Ok(new { message = "Password changed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ProfileDto>> UpdateProfile(UpdateProfileDto dto)
    {
        try
        {
            var userId = GetUserId();
            var profile = await _profileService.UpdateProfileAsync(userId, dto);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
