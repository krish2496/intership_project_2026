namespace Tracker.Core.DTOs.Recommendation;

public class RecommendationDto
{
    public int MediaId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public decimal Score { get; set; } // 0-100 confidence score
    public string Reason { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? TotalEpisodes { get; set; }
}

public class SimilarAnimeDto
{
    public int MediaId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public decimal SimilarityScore { get; set; }
    public string? Description { get; set; }
}
