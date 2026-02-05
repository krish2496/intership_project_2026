using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.DTOs.Comments;

public class CreateCommentDto
{
    [Required]
    public string Content { get; set; } = string.Empty;

    public bool IsSpoiler { get; set; }
    
    public int? ParentCommentId { get; set; }
}

public class CommentDto
{
    public int Id { get; set; }
    public int DiscussionId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsSpoiler { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int? ParentCommentId { get; set; }
    public List<CommentDto> Replies { get; set; } = new();

    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
    public int? UserVote { get; set; } // 1 = like, -1 = dislike, 0/null = none
}
