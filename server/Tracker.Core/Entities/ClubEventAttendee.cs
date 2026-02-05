using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class ClubEventAttendee
{
    public int Id { get; set; }

    [Required]
    public int EventId { get; set; }
    public ClubEvent Event { get; set; } = null!;

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Going"; // 'Going', 'Maybe', 'NotGoing'

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
