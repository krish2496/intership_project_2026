using Tracker.Core.DTOs.Comments;

namespace Tracker.Core.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetDiscussionCommentsAsync(int discussionId, int? userId = null);
    Task<CommentDto> CreateCommentAsync(int userId, int discussionId, CreateCommentDto dto);
    Task<bool> DeleteCommentAsync(int userId, int commentId);
    Task LikeCommentAsync(int userId, int commentId);
    Task DislikeCommentAsync(int userId, int commentId);
    Task RemoveLikeAsync(int userId, int commentId);
}
