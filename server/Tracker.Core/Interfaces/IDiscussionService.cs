using Tracker.Core.DTOs.Discussions;

namespace Tracker.Core.Interfaces;

public interface IDiscussionService
{
    Task<IEnumerable<DiscussionDto>> GetClubDiscussionsAsync(int clubId);
    Task<DiscussionDto> GetDiscussionByIdAsync(int id);
    Task<DiscussionDto> CreateDiscussionAsync(int userId, int clubId, CreateDiscussionDto dto);
    Task<bool> DeleteDiscussionAsync(int userId, int discussionId);
}
