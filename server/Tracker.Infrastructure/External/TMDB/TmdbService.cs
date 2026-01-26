using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Tracker.Core.Entities;
using Tracker.Core.Enums;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.External.TMDB.DTOs;

namespace Tracker.Infrastructure.External.TMDB;

public class TmdbService : ITvSeriesService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseImageUrl = "https://image.tmdb.org/t/p/w500";

    public TmdbService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
        _apiKey = config["Tmdb:ApiKey"] ?? string.Empty;
    }

    public async Task<IEnumerable<Media>> SearchTvSeriesAsync(string query)
    {
        var response = await _httpClient.GetAsync($"search/tv?api_key={_apiKey}&query={query}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TmdbResponse<List<TmdbTvSeries>>>(content);

        if (result?.Results == null) return Enumerable.Empty<Media>();

        return result.Results.Select(MapToMedia);
    }

    public async Task<Media?> GetTvSeriesDetailsAsync(int id)
    {
        var response = await _httpClient.GetAsync($"tv/{id}?api_key={_apiKey}");
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TmdbTvSeries>(content);

        return result != null ? MapToMedia(result) : null;
    }

    private static Media MapToMedia(TmdbTvSeries tv)
    {
        return new Media
        {
            Title = tv.Name,
            Type = MediaType.TVSeries,
            ExternalId = tv.Id,
            Description = tv.Overview,
            TotalEpisodes = tv.NumberOfEpisodes,
            CoverImageUrl = !string.IsNullOrEmpty(tv.PosterPath) ? $"{BaseImageUrl}{tv.PosterPath}" : null
        };
    }
}
