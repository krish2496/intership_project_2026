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

    public async Task<IEnumerable<CommentDto>> GetDiscussionCommentsAsync(int discussionId)
    {
        var comments = await _context.Comments
            .Include(c => c.User)
            .Where(c => c.DiscussionId == discussionId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

        // Build tree structure
        var commentDtos = comments.Select(MapToDto).ToList();
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

        return MapToDto(comment);
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

    private static CommentDto MapToDto(Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            DiscussionId = comment.DiscussionId,
            UserId = comment.UserId,
            UserName = comment.User.Username,
            Content = comment.Content,
            IsSpoiler = comment.IsSpoiler,
            CreatedAt = comment.CreatedAt,
            ParentCommentId = comment.ParentCommentId
        };
    }
}
