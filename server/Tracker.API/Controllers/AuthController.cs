using Microsoft.AspNetCore.Mvc;
using Tracker.Core.DTOs;
using Tracker.Core.Interfaces;

namespace Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        try
        {
            var user = await _authService.RegisterAsync(registerDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        try
        {
            var user = await _authService.LoginAsync(loginDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
