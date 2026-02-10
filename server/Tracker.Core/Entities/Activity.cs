using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.Entities;

public enum ActivityType
{
    WatchedEpisode,
    CompletedShow,
    RatedShow,
    JoinedClub
}

public class Activity
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int? MediaId { get; set; }
    public Media? Media { get; set; }

    public ActivityType Type { get; set; }
    
    [MaxLength(500)]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
