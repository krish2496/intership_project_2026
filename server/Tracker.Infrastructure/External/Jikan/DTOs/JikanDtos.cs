using System.Text.Json.Serialization;

namespace Tracker.Infrastructure.External.Jikan.DTOs;

public class JikanResponse<T>
{
    [JsonPropertyName("data")]
    public T Data { get; set; } = default!;
}

public class JikanAnime
{
    [JsonPropertyName("mal_id")]
    public int MalId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("episodes")]
    public int? Episodes { get; set; }

    [JsonPropertyName("synopsis")]
    public string Synopsis { get; set; } = string.Empty;

    [JsonPropertyName("images")]
    public JikanImages Images { get; set; } = new();
}

public class JikanImages
{
    [JsonPropertyName("jpg")]
    public JikanImageFormat Jpg { get; set; } = new();
}

public class JikanImageFormat
{
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = string.Empty;
    
    [JsonPropertyName("large_image_url")]
    public string LargeImageUrl { get; set; } = string.Empty;
}
