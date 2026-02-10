using Microsoft.EntityFrameworkCore;
using Tracker.Core.DTOs;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class ActivityService : IActivityService
{
    private readonly TrackerDbContext _context;

    public ActivityService(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task LogActivityAsync(int userId, ActivityType type, int? mediaId, string description)
    {
        var activity = new Activity
        {
            UserId = userId,
            Type = type,
            MediaId = mediaId,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ActivityDto>> GetFeedAsync(int userId)
    {
        // Get list of users I follow
        var followingIds = await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.FollowingId)
            .ToListAsync();

        // Include self (optional, but good for feed)
        followingIds.Add(userId);

        var activities = await _context.Activities
            .Where(a => followingIds.Contains(a.UserId))
            .Include(a => a.User)
            .Include(a => a.Media)
            .OrderByDescending(a => a.CreatedAt)
            .Take(50) // Pagination limit
            .ToListAsync();

        return activities.Select(a => new ActivityDto
        {
            Id = a.Id,
            User = new ActivityUserDto
            {
                Id = a.User.Id,
                Username = a.User.Username
            },
            Media = a.Media != null ? new ActivityMediaDto
            {
                Id = a.Media.Id,
                Title = a.Media.Title,
                CoverImageUrl = a.Media.CoverImageUrl,
                ExternalId = a.Media.ExternalId,
                Type = (int)a.Media.Type
            } : null,
            Type = (int)a.Type,
            Description = a.Description,
            CreatedAt = a.CreatedAt
        });
    }

    public async Task<IEnumerable<ActivityDto>> GetUserActivityAsync(int userId)
    {
        var activities = await _context.Activities
            .Where(a => a.UserId == userId)
            .Include(a => a.User)
            .Include(a => a.Media)
            .OrderByDescending(a => a.CreatedAt)
            .Take(20)
            .ToListAsync();

        return activities.Select(a => new ActivityDto
        {
            Id = a.Id,
            User = new ActivityUserDto
            {
                Id = a.User.Id,
                Username = a.User.Username
            },
            Media = a.Media != null ? new ActivityMediaDto
            {
                Id = a.Media.Id,
                Title = a.Media.Title,
                CoverImageUrl = a.Media.CoverImageUrl,
                ExternalId = a.Media.ExternalId,
                Type = (int)a.Media.Type
            } : null,
            Type = (int)a.Type,
            Description = a.Description,
            CreatedAt = a.CreatedAt
        });
    }
}
