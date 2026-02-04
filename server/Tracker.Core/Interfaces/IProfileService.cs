using Tracker.Core.DTOs;

namespace Tracker.Core.Interfaces;

public interface IProfileService
{
    Task<ProfileDto> GetProfileAsync(int userId);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    Task<ProfileDto> UpdateProfileAsync(int userId, UpdateProfileDto dto);
}
