using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class ClubList
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

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsOfficial { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ClubListItem> Items { get; set; } = new List<ClubListItem>();
}
