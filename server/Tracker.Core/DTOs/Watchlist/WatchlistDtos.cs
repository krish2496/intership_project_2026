using System.ComponentModel.DataAnnotations;
using Tracker.Core.Enums;

namespace Tracker.Core.DTOs.Watchlist;

public class AddToWatchlistDto
{
    [Required]
    public int MediaExternalId { get; set; } // Jikan ID or TMDB ID

    [Required]
    public MediaType MediaType { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }

    public int? TotalEpisodes { get; set; }
    
    public string? Description { get; set; }

    public WatchStatus Status { get; set; } = WatchStatus.PlanToWatch;
}

public class UpdateWatchlistDto
{
    public WatchStatus? Status { get; set; }
    public int? Progress { get; set; }
    public int? Rating { get; set; }
}

public class WatchlistDto
{
    public int Id { get; set; }
    public int MediaId { get; set; }
    public MediaDto Media { get; set; } = null!;
    public WatchStatus Status { get; set; }
    public int Progress { get; set; }
    public int? Rating { get; set; }
}
