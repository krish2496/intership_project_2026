using System.ComponentModel.DataAnnotations;
using Tracker.Core.Enums;

namespace Tracker.Core.Entities;

public class Media
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public int? ExternalId { get; set; } // For Jikan (MalID) or TMDB ID

    public MediaType Type { get; set; }

    public int? TotalEpisodes { get; set; }

    public string? Description { get; set; }

    public string? CoverImageUrl { get; set; }

    public ICollection<Watchlist> WatchlistEntries { get; set; } = new List<Watchlist>();
}
