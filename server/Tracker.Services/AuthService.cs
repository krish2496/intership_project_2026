using BCrypt.Net;
using Tracker.Core.DTOs;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;

namespace Tracker.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepository.GetByEmailAsync(dto.Email) != null)
            throw new Exception("Email already in use");

        if (await _userRepository.GetByUsernameAsync(dto.Username) != null)
            throw new Exception("Username already in use");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _userRepository.AddAsync(user);

        return new UserDto
        {
            Username = user.Username,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        };
    }

    public async Task<UserDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
            throw new Exception("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        return new UserDto
        {
            Username = user.Username,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        };
    }
}
