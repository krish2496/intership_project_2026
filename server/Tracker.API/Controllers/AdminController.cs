using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tracker.Core.DTOs.Admin;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<SystemStatsDto>> GetStats()
    {
        return Ok(await _adminService.GetSystemStatsAsync());
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserAdminDto>>> GetUsers()
    {
        return Ok(await _adminService.GetAllUsersAsync());
    }

    [HttpDelete("users/{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var success = await _adminService.DeleteUserAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
