using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class ClubEvent
{
    public int Id { get; set; }

    [Required]
    public int ClubId { get; set; }
    public Club Club { get; set; } = null!;

    [Required]
    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string EventType { get; set; } = string.Empty; // 'WatchParty', 'Discussion', 'Contest'

    [Required]
    public DateTime EventDate { get; set; }

    public int Duration { get; set; } // in minutes

    public bool IsRecurring { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ClubEventAttendee> Attendees { get; set; } = new List<ClubEventAttendee>();
}
