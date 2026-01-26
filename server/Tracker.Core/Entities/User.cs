using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public string ThemePreference { get; set; } = "Light"; // "Light" or "Dark"

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Watchlist> Watchlist { get; set; } = new List<Watchlist>();
    public ICollection<Club> OwnedClubs { get; set; } = new List<Club>();
    public ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();
}
