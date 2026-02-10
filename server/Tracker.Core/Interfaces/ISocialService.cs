using Tracker.Core.DTOs;

namespace Tracker.Core.Interfaces;

public interface ISocialService
{
    Task<bool> FollowUserAsync(int followerId, int followingId);
    Task<bool> UnfollowUserAsync(int followerId, int followingId);
    Task<IEnumerable<object>> GetFollowersAsync(int userId);
    Task<IEnumerable<object>> GetFollowingAsync(int userId);
    Task<object?> GetPublicProfileAsync(int userId);
}
