using Tracker.Core.Entities;
using Tracker.Core.Enums;

namespace Tracker.Core.Interfaces;

public interface IMediaService
{
    Task<IEnumerable<Media>> SearchAsync(string query, MediaType type);
    Task<Media?> GetDetailsAsync(int id, MediaType type);
}
