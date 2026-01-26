using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Core.Entities;
using Tracker.Core.Enums;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Media>>> Search([FromQuery] string query, [FromQuery] MediaType type)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query cannot be empty");

        var results = await _mediaService.SearchAsync(query, type);
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Media>> GetDetails(int id, [FromQuery] MediaType type)
    {
        var media = await _mediaService.GetDetailsAsync(id, type);
        if (media == null) return NotFound();

        return Ok(media);
    }
}
