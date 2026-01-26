using Tracker.Core.Enums;

namespace Tracker.Core.DTOs;

public class MediaDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? ExternalId { get; set; }
    public MediaType Type { get; set; }
    public int? TotalEpisodes { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
}
