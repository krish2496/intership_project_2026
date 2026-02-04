using Tracker.Core.DTOs;
using Tracker.Core.DTOs.Admin;

namespace Tracker.Core.Interfaces;

public interface IAdminService
{
    Task<SystemStatsDto> GetSystemStatsAsync();
    Task<PublicStatsDto> GetPublicStatsAsync();
    Task<IEnumerable<UserAdminDto>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(int userId);
}
