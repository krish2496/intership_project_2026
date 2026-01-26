using Microsoft.EntityFrameworkCore;
using Tracker.Core.DTOs.Discussions;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class DiscussionService : IDiscussionService
{
    private readonly TrackerDbContext _context;

    public DiscussionService(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DiscussionDto>> GetClubDiscussionsAsync(int clubId)
    {
        var discussions = await _context.Discussions
            .Include(d => d.User)
            .Where(d => d.ClubId == clubId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();

        return discussions.Select(MapToDto);
    }

    public async Task<DiscussionDto> GetDiscussionByIdAsync(int id)
    {
        var discussion = await _context.Discussions
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (discussion == null) throw new Exception("Discussion not found");

        return MapToDto(discussion);
    }

    public async Task<DiscussionDto> CreateDiscussionAsync(int userId, int clubId, CreateDiscussionDto dto)
    {
        var discussion = new Discussion
        {
            UserId = userId,
            ClubId = clubId,
            Title = dto.Title,
            Content = dto.Content,
            IsSpoiler = dto.IsSpoiler
        };

        _context.Discussions.Add(discussion);
        await _context.SaveChangesAsync();

        await _context.Entry(discussion).Reference(d => d.User).LoadAsync();

        return MapToDto(discussion);
    }

    public async Task<bool> DeleteDiscussionAsync(int userId, int discussionId)
    {
        var discussion = await _context.Discussions.FindAsync(discussionId);
        if (discussion == null) return false;

        if (discussion.UserId != userId) throw new UnauthorizedAccessException("Not the author");

        _context.Discussions.Remove(discussion);
        await _context.SaveChangesAsync();
        return true;
    }

    private static DiscussionDto MapToDto(Discussion discussion)
    {
        return new DiscussionDto
        {
            Id = discussion.Id,
            ClubId = discussion.ClubId,
            UserId = discussion.UserId,
            UserName = discussion.User.Username,
            Title = discussion.Title,
            Content = discussion.Content,
            IsSpoiler = discussion.IsSpoiler,
            CreatedAt = discussion.CreatedAt
        };
    }
}
