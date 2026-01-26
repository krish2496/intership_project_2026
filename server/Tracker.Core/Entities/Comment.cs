using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class Comment
{
    public int Id { get; set; }

    public int DiscussionId { get; set; }
    public Discussion Discussion { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public string Content { get; set; } = string.Empty;

    public bool IsSpoiler { get; set; }

    public int? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
