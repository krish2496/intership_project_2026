using Tracker.Core.DTOs;

namespace Tracker.Core.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<UserDto> LoginAsync(LoginDto dto);
}
