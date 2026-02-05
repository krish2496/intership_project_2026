using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class ClubInvite
{
    public int Id { get; set; }

    [Required]
    public int ClubId { get; set; }
    public Club Club { get; set; } = null!;

    [Required]
    public int InviterId { get; set; }
    public User Inviter { get; set; } = null!;

    [Required]
    public int InviteeId { get; set; }
    public User Invitee { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // 'Pending', 'Accepted', 'Declined'

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
