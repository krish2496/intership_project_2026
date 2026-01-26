using Tracker.Core.DTOs.Admin;

namespace Tracker.Core.Interfaces;

public interface IAdminService
{
    Task<SystemStatsDto> GetSystemStatsAsync();
    Task<IEnumerable<UserAdminDto>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(int userId);
}
