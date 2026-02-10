using Tracker.Core.DTOs;
using Tracker.Core.Entities;

namespace Tracker.Core.Interfaces;

public interface IActivityService
{
    Task LogActivityAsync(int userId, ActivityType type, int? mediaId, string description);
    Task<IEnumerable<ActivityDto>> GetFeedAsync(int userId);
    Task<IEnumerable<ActivityDto>> GetUserActivityAsync(int userId);
}
