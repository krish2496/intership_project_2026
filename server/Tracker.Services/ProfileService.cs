using BCrypt.Net;
using Tracker.Core.DTOs;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;

namespace Tracker.Services;

public class ProfileService : IProfileService
{
    private readonly IUserRepository _userRepository;

    public ProfileService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ProfileDto> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        return new ProfileDto
        {
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            ThemePreference = user.ThemePreference,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new Exception("Current password is incorrect");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);
    }

    public async Task<ProfileDto> UpdateProfileAsync(int userId, UpdateProfileDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        // Check if username is being changed and if it's already taken
        if (!string.IsNullOrEmpty(dto.Username) && dto.Username != user.Username)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new Exception("Username already in use");
            user.Username = dto.Username;
        }

        // Check if email is being changed and if it's already taken
        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already in use");
            user.Email = dto.Email;
        }

        // Update theme preference if provided
        if (!string.IsNullOrEmpty(dto.ThemePreference))
        {
            user.ThemePreference = dto.ThemePreference;
        }

        await _userRepository.UpdateAsync(user);

        return new ProfileDto
        {
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            ThemePreference = user.ThemePreference,
            CreatedAt = user.CreatedAt
        };
    }
}
