using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tracker.Core.Entities;
using Tracker.Core.Enums;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class RecommendationService : IRecommendationService
{
    private readonly TrackerDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly string _tmdbApiKey;

    public RecommendationService(TrackerDbContext context, IConfiguration config, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
        _tmdbApiKey = config["Tmdb:ApiKey"] ?? "";
    }

    public async Task<IEnumerable<Media>> GetRecommendationsForUserAsync(int userId)
    {
        // 1. Get user's highly rated or watched items
        var seeds = await _context.Watchlists
            .Include(w => w.Media)
            .Where(w => w.UserId == userId && (w.Status == WatchStatus.Completed || w.Status == WatchStatus.Watching))
            .OrderByDescending(w => w.UpdatedAt)
            .Take(5)
            .ToListAsync();

        if (!seeds.Any()) return Enumerable.Empty<Media>();

        // 2. Pick a random seed
        var random = new Random();
        var seed = seeds[random.Next(seeds.Count)];

        // 3. Fetch from External API
        if (!seed.Media.ExternalId.HasValue) return Enumerable.Empty<Media>();

        if (seed.Media.Type == MediaType.Anime)
        {
            return await GetJikanRecommendations(seed.Media.ExternalId.Value);
        }
        else
        {
            return await GetTmdbRecommendations(seed.Media.ExternalId.Value);
        }
    }

    private async Task<IEnumerable<Media>> GetJikanRecommendations(int malId)
    {
        try
        {
            // Jikan Recommendation Endpoint: v4/anime/{id}/recommendations
            var response = await _httpClient.GetAsync($"https://api.jikan.moe/v4/anime/{malId}/recommendations");
            if (!response.IsSuccessStatusCode) return Enumerable.Empty<Media>();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JikanRecResponse>(content);
            
            return result?.Data.Select(item => new Media
            {
                Title = item.Entry.Title,
                ExternalId = item.Entry.MalId,
                Type = MediaType.Anime,
                CoverImageUrl = item.Entry.Images.Jpg.LargeImageUrl,
                Description = "Recommended based on your history."
            }).Take(6) ?? Enumerable.Empty<Media>();
        }
        catch (Exception)
        {
            return Enumerable.Empty<Media>();
        }
    }

    private async Task<IEnumerable<Media>> GetTmdbRecommendations(int tmdbId)
    {
        // Placeholder for TMDB implementation using similar pattern
        return Enumerable.Empty<Media>(); 
    }
}

// Internal DTOs for Jikan Response parsing just for this service
class JikanRecResponse
{
    [JsonPropertyName("data")]
    public List<JikanRecItem> Data { get; set; } = new();
}

class JikanRecItem
{
    [JsonPropertyName("entry")]
    public JikanEntry Entry { get; set; } = new();
}

class JikanEntry
{
    [JsonPropertyName("mal_id")]
    public int MalId { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";
    [JsonPropertyName("images")]
    public JikanImages Images { get; set; } = new();
}

class JikanImages
{
    [JsonPropertyName("jpg")]
    public JikanImageFormat Jpg { get; set; } = new();
}

class JikanImageFormat
{
    [JsonPropertyName("large_image_url")]
    public string LargeImageUrl { get; set; } = "";
}
