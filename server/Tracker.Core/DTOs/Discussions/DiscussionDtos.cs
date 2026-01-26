using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.DTOs.Discussions;

public class CreateDiscussionDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public bool IsSpoiler { get; set; }
}

public class DiscussionDto
{
    public int Id { get; set; }
    public int ClubId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsSpoiler { get; set; }
    public DateTime CreatedAt { get; set; }
}
