using Tracker.Core.Entities;

namespace Tracker.Core.Interfaces;

public interface IAnimeService
{
    Task<IEnumerable<Media>> SearchAnimeAsync(string query);
    Task<Media?> GetAnimeDetailsAsync(int id);
}
