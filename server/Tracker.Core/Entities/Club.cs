using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class Club
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public bool IsPrivate { get; set; } = false;
    public int ActivityScore { get; set; } = 0;
    public int Level { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();
    public ICollection<ClubEvent> Events { get; set; } = new List<ClubEvent>();
    public ICollection<ClubList> Lists { get; set; } = new List<ClubList>();
    public ICollection<ClubInvite> Invites { get; set; } = new List<ClubInvite>();
}

public class Discussion
{
    public int Id { get; set; }

    public int ClubId { get; set; }
    public Club Club { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public bool IsSpoiler { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
