using System.Text.Json;
using Tracker.Core.Entities;
using Tracker.Core.Enums;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.External.Jikan.DTOs;

namespace Tracker.Infrastructure.External.Jikan;

public class JikanService : IAnimeService
{
    private readonly HttpClient _httpClient;

    public JikanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.jikan.moe/v4/");
    }

    public async Task<IEnumerable<Media>> SearchAnimeAsync(string query)
    {
        var response = await _httpClient.GetAsync($"anime?q={query}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JikanResponse<List<JikanAnime>>>(content);

        if (result?.Data == null) return Enumerable.Empty<Media>();

        return result.Data.Select(MapToMedia);
    }

    public async Task<Media?> GetAnimeDetailsAsync(int id)
    {
        var response = await _httpClient.GetAsync($"anime/{id}");
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JikanResponse<JikanAnime>>(content);

        return result?.Data != null ? MapToMedia(result.Data) : null;
    }

    private static Media MapToMedia(JikanAnime anime)
    {
        return new Media
        {
            Title = anime.Title,
            Type = MediaType.Anime, 
            ExternalId = anime.MalId,
            TotalEpisodes = anime.Episodes,
            Description = anime.Synopsis,
            CoverImageUrl = anime.Images.Jpg.LargeImageUrl ?? anime.Images.Jpg.ImageUrl
        };
    }
}
