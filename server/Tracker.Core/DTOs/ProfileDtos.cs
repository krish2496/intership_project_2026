using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.DTOs;

public class ProfileDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string ThemePreference { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}

public class UpdateProfileDto
{
    [MaxLength(50)]
    public string? Username { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    public string? ThemePreference { get; set; }
}
