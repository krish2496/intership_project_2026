using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class ClubListItem
{
    public int Id { get; set; }

    [Required]
    public int ListId { get; set; }
    public ClubList List { get; set; } = null!;

    [Required]
    public int MediaId { get; set; }

    [Required]
    [MaxLength(255)]
    public string MediaTitle { get; set; } = string.Empty;

    [Required]
    public int AddedById { get; set; }
    public User AddedBy { get; set; } = null!;

    public int Position { get; set; } // For ordering

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
