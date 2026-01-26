namespace Tracker.Core.DTOs.Admin;

public class SystemStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalMediaTracked { get; set; } // Sum of watchlist entries
    public int TotalClubs { get; set; }
    public int TotalDiscussions { get; set; }
    public int TotalPolls { get; set; }
}

public class UserAdminDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
