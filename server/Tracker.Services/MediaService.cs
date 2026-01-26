using Tracker.Core.Entities;
using Tracker.Core.Enums;
using Tracker.Core.Interfaces;

namespace Tracker.Services;

public class MediaService : IMediaService
{
    private readonly IAnimeService _animeService;
    private readonly ITvSeriesService _tvSeriesService;

    public MediaService(IAnimeService animeService, ITvSeriesService tvSeriesService)
    {
        _animeService = animeService;
        _tvSeriesService = tvSeriesService;
    }

    public async Task<IEnumerable<Media>> SearchAsync(string query, MediaType type)
    {
        if (type == MediaType.Anime)
        {
            return await _animeService.SearchAnimeAsync(query);
        }
        else
        {
            return await _tvSeriesService.SearchTvSeriesAsync(query);
        }
    }

    public async Task<Media?> GetDetailsAsync(int id, MediaType type)
    {
        if (type == MediaType.Anime)
        {
            return await _animeService.GetAnimeDetailsAsync(id);
        }
        else
        {
            return await _tvSeriesService.GetTvSeriesDetailsAsync(id);
        }
    }
}
