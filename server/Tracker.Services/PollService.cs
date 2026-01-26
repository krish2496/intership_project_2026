using Microsoft.EntityFrameworkCore;
using Tracker.Core.DTOs.Polls;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class PollService : IPollService
{
    private readonly TrackerDbContext _context;

    public PollService(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PollDto>> GetActivePollsAsync(int? clubId)
    {
        var query = _context.Polls
            .Include(p => p.Creator)
            .Include(p => p.Options)
            .ThenInclude(o => o.Votes)
            .Include(p => p.Votes)
            .Where(p => p.IsActive);

        if (clubId.HasValue)
        {
            query = query.Where(p => p.ClubId == clubId);
        }
        else
        {
            query = query.Where(p => p.ClubId == null);
        }

        var polls = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        return polls.Select(p => MapToDto(p, null)); // User ID passed as null here, Controller will need to enrich or handle
    }
    
    // Overload for retrieving with user context (to see what they voted for) requires slightly different flow
    // For simplicity, we might just return all data and let frontend check user ID in votes if exposed, 
    // OR we modify the method signature to accept userId (optional). Let's modify the interface/method to accept userId for "Get".
    // Wait, the interface is "GetActivePollsAsync(int? clubId)". I'll keep it simple and handle user vote check logic here if I can pass userId.
    // For now, let's just return the DTO. The DTO has "UserVotedOptionId". We need userId to populate that.
    // I will refactor to accept userId logic in a bit or just return basic DTO.
    // Let's implement the basic one and maybe add a specific "GetPollsForUser" if needed.
    // Actually, let's just use the current DTO but populate UserVotedOptionId as null for now in the list view unless we pass userId.
    
    // Better approach: Let's assume we want to show if the user voted.
    // I'll stick to the interface defined.

    public async Task<PollDto> CreatePollAsync(int userId, CreatePollDto dto)
    {
        var poll = new Poll
        {
            CreatorId = userId,
            ClubId = dto.ClubId,
            Question = dto.Question,
            IsActive = true
        };

        foreach (var optText in dto.Options)
        {
            poll.Options.Add(new PollOption { Text = optText });
        }

        _context.Polls.Add(poll);
        await _context.SaveChangesAsync();

        await _context.Entry(poll).Reference(p => p.Creator).LoadAsync();

        return MapToDto(poll, userId);
    }

    public async Task<PollDto> VoteAsync(int userId, int pollId, VoteDto dto)
    {
        var poll = await _context.Polls
            .Include(p => p.Creator)
            .Include(p => p.Options)
            .ThenInclude(o => o.Votes)
            .Include(p => p.Votes)
            .FirstOrDefaultAsync(p => p.Id == pollId);

        if (poll == null) throw new Exception("Poll not found");
        if (!poll.IsActive) throw new Exception("Poll is closed");

        var existingVote = poll.Votes.FirstOrDefault(v => v.UserId == userId);
        if (existingVote != null) throw new Exception("Already voted");

        var option = poll.Options.FirstOrDefault(o => o.Id == dto.OptionId);
        if (option == null) throw new Exception("Invalid option");

        var vote = new PollVote
        {
            PollId = pollId,
            PollOptionId = dto.OptionId,
            UserId = userId
        };

        _context.PollVotes.Add(vote);
        await _context.SaveChangesAsync();

        // Refresh options/votes
        // Actually EF Core tracks them, but we added to the DbSet directly.
        // Let's just return the updated DTO.
        // We need to re-fetch or manual update for the DTO return.
        
        // Manual update of in-memory object for DTO construction
        poll.Votes.Add(vote);
        option.Votes.Add(vote);

        return MapToDto(poll, userId);
    }

    public async Task<bool> DeletePollAsync(int userId, int pollId)
    {
        var poll = await _context.Polls.FindAsync(pollId);
        if (poll == null) return false;

        if (poll.CreatorId != userId) throw new UnauthorizedAccessException("Not the creator");

        _context.Polls.Remove(poll);
        await _context.SaveChangesAsync();
        return true;
    }

    private static PollDto MapToDto(Poll poll, int? currentUserId)
    {
        var dto = new PollDto
        {
            Id = poll.Id,
            Question = poll.Question,
            CreatorName = poll.Creator.Username,
            IsActive = poll.IsActive,
            ClubId = poll.ClubId,
            CreatedAt = poll.CreatedAt,
            TotalVotes = poll.Votes.Count,
            Options = poll.Options.Select(o => new PollOptionDto
            {
                Id = o.Id,
                Text = o.Text,
                VoteCount = o.Votes.Count
            }).ToList()
        };

        if (currentUserId.HasValue)
        {
            var userVote = poll.Votes.FirstOrDefault(v => v.UserId == currentUserId);
            if (userVote != null)
            {
                dto.UserVotedOptionId = userVote.PollOptionId;
            }
        }

        return dto;
    }
    
    // Extension to filtering with userId
     public async Task<IEnumerable<PollDto>> GetActivePollsWithUserAsync(int? clubId, int userId)
    {
        var query = _context.Polls
            .Include(p => p.Creator)
            .Include(p => p.Options)
            .ThenInclude(o => o.Votes)
            .Include(p => p.Votes)
            .Where(p => p.IsActive);

        if (clubId.HasValue)
        {
            query = query.Where(p => p.ClubId == clubId);
        }
        else
        {
            query = query.Where(p => p.ClubId == null);
        }

        var polls = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        return polls.Select(p => MapToDto(p, userId));
    }
}
