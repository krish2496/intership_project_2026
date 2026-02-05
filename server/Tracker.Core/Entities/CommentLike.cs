using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class CommentLike
{
    public int Id { get; set; }

    [Required]
    public int CommentId { get; set; }
    public Comment Comment { get; set; } = null!;

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public bool IsLike { get; set; } // true = like, false = dislike

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
