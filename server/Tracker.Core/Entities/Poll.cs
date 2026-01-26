using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class Poll
{
    public int Id { get; set; }

    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;

    public int? ClubId { get; set; } // Optional: Polls can be global or club-specific
    public Club? Club { get; set; }

    [Required]
    public string Question { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
    public ICollection<PollVote> Votes { get; set; } = new List<PollVote>();
}

public class PollOption
{
    public int Id { get; set; }

    public int PollId { get; set; }
    public Poll Poll { get; set; } = null!;

    [Required]
    public string Text { get; set; } = string.Empty;

    public ICollection<PollVote> Votes { get; set; } = new List<PollVote>();
}

public class PollVote
{
    public int Id { get; set; }

    public int PollId { get; set; }
    public Poll Poll { get; set; } = null!;

    public int PollOptionId { get; set; }
    public PollOption PollOption { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
