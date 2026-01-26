using Tracker.Core.Entities;

namespace Tracker.Core.Interfaces;

public interface ITvSeriesService
{
    Task<IEnumerable<Media>> SearchTvSeriesAsync(string query);
    Task<Media?> GetTvSeriesDetailsAsync(int id);
}
