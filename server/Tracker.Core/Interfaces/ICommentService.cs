using Tracker.Core.DTOs.Comments;

namespace Tracker.Core.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetDiscussionCommentsAsync(int discussionId);
    Task<CommentDto> CreateCommentAsync(int userId, int discussionId, CreateCommentDto dto);
    Task<bool> DeleteCommentAsync(int userId, int commentId);
}
