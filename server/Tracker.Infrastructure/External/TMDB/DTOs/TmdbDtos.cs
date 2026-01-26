using System.Text.Json.Serialization;

namespace Tracker.Infrastructure.External.TMDB.DTOs;

public class TmdbResponse<T>
{
    [JsonPropertyName("results")]
    public T Results { get; set; } = default!;
}

public class TmdbTvSeries
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("overview")]
    public string Overview { get; set; } = string.Empty;

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("number_of_episodes")]
    public int? NumberOfEpisodes { get; set; } // Only available in details endpoint
}
