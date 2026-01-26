using Tracker.Core.DTOs.Clubs;

namespace Tracker.Core.Interfaces;

public interface IClubService
{
    Task<IEnumerable<ClubDto>> GetAllClubsAsync();
    Task<ClubDto> GetClubByIdAsync(int id);
    Task<ClubDto> CreateClubAsync(int userId, CreateClubDto dto);
    Task<bool> DeleteClubAsync(int userId, int clubId);
}
