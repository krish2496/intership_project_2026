using Microsoft.EntityFrameworkCore;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class SocialService : ISocialService
{
    private readonly TrackerDbContext _context;

    public SocialService(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> FollowUserAsync(int followerId, int followingId)
    {
        if (followerId == followingId) return false;

        var exists = await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        if (exists) return false;

        var follow = new Follow
        {
            FollowerId = followerId,
            FollowingId = followingId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnfollowUserAsync(int followerId, int followingId)
    {
        var follow = await _context.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

        if (follow == null) return false;

        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<object>> GetFollowersAsync(int userId)
    {
        return await _context.Follows
            .Where(f => f.FollowingId == userId)
            .Include(f => f.Follower)
            .Select(f => new 
            {
                f.Follower.Id,
                f.Follower.Username,
                f.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetFollowingAsync(int userId)
    {
        return await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Include(f => f.Following)
            .Select(f => new 
            {
                f.Following.Id,
                f.Following.Username,
                f.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<object?> GetPublicProfileAsync(int userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new 
            {
                u.Id,
                u.Username,
                u.CreatedAt,
                FollowersCount = u.Followers.Count(),
                FollowingCount = u.Following.Count()
            })
            .FirstOrDefaultAsync();

        return user;
    }
}
