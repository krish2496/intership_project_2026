using Tracker.Core.Enums;

namespace Tracker.Core.Entities;

public class Watchlist
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int MediaId { get; set; }
    public Media Media { get; set; } = null!;

    public WatchStatus Status { get; set; }

    public int Progress { get; set; } // Episodes watched

    public int? Rating { get; set; } // 1-10

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
