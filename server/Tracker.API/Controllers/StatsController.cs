using Microsoft.AspNetCore.Mvc;
using Tracker.Core.DTOs;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly IAdminService _adminService;

    public StatsController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("public")]
    public async Task<ActionResult<PublicStatsDto>> GetPublicStats()
    {
        var stats = await _adminService.GetPublicStatsAsync();
        return Ok(stats);
    }
}
