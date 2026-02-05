using Microsoft.EntityFrameworkCore;
using Tracker.Core.DTOs.Comments;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class CommentService : ICommentService
{
    private readonly TrackerDbContext _context;

    public CommentService(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommentDto>> GetDiscussionCommentsAsync(int discussionId, int? userId = null)
    {
        var comments = await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Likes)
            .Where(c => c.DiscussionId == discussionId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

        // Build tree structure
        var commentDtos = comments.Select(c => MapToDto(c, userId)).ToList();
        var lookup = commentDtos.ToDictionary(c => c.Id);
        var rootComments = new List<CommentDto>();

        foreach (var dto in commentDtos)
        {
            if (dto.ParentCommentId.HasValue && lookup.TryGetValue(dto.ParentCommentId.Value, out var parent))
            {
                parent.Replies.Add(dto);
            }
            else
            {
                rootComments.Add(dto);
            }
        }

        return rootComments;
    }

    public async Task<CommentDto> CreateCommentAsync(int userId, int discussionId, CreateCommentDto dto)
    {
        var comment = new Comment
        {
            UserId = userId,
            DiscussionId = discussionId,
            Content = dto.Content,
            IsSpoiler = dto.IsSpoiler,
            ParentCommentId = dto.ParentCommentId
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        await _context.Entry(comment).Reference(c => c.User).LoadAsync();

        return MapToDto(comment, userId);
    }

    public async Task<bool> DeleteCommentAsync(int userId, int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null) return false;

        if (comment.UserId != userId) throw new UnauthorizedAccessException("Not the author");

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task LikeCommentAsync(int userId, int commentId)
    {
        await HandleVoteAsync(userId, commentId, true);
    }

    public async Task DislikeCommentAsync(int userId, int commentId)
    {
        await HandleVoteAsync(userId, commentId, false);
    }

    public async Task RemoveLikeAsync(int userId, int commentId)
    {
        var existing = await _context.CommentLikes
            .FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

        if (existing != null)
        {
            _context.CommentLikes.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }

    private async Task HandleVoteAsync(int userId, int commentId, bool isLike)
    {
        var existing = await _context.CommentLikes
            .FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

        if (existing == null)
        {
            _context.CommentLikes.Add(new CommentLike
            {
                CommentId = commentId,
                UserId = userId,
                IsLike = isLike
            });
        }
        else
        {
            if (existing.IsLike == isLike)
            {
                // Toggle off if same vote
                _context.CommentLikes.Remove(existing);
            }
            else
            {
                // Change vote
                existing.IsLike = isLike;
            }
        }

        await _context.SaveChangesAsync();
    }

    private static CommentDto MapToDto(Comment comment, int? userId)
    {
        var userLike = comment.Likes.FirstOrDefault(l => l.UserId == userId);
        
        return new CommentDto
        {
            Id = comment.Id,
            DiscussionId = comment.DiscussionId,
            UserId = comment.UserId,
            UserName = comment.User.Username,
            Content = comment.Content,
            IsSpoiler = comment.IsSpoiler,
            CreatedAt = comment.CreatedAt,
            ParentCommentId = comment.ParentCommentId,
            LikeCount = comment.Likes.Count(l => l.IsLike),
            DislikeCount = comment.Likes.Count(l => !l.IsLike),
            UserVote = userLike == null ? 0 : (userLike.IsLike ? 1 : -1)
        };
    }
}
