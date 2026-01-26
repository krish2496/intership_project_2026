using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.DTOs.Clubs;

public class CreateClubDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}

public class ClubDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public int MemberCount { get; set; } // Placeholder for future logic
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // We might want to add CreatedAt to Entity
}
