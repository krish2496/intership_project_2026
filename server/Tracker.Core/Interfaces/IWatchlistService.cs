using Tracker.Core.DTOs.Watchlist;

namespace Tracker.Core.Interfaces;

public interface IWatchlistService
{
    Task<WatchlistDto> AddToWatchlistAsync(int userId, AddToWatchlistDto dto);
    Task<WatchlistDto?> UpdateWatchlistAsync(int userId, int watchlistId, UpdateWatchlistDto dto);
    Task<bool> RemoveFromWatchlistAsync(int userId, int watchlistId);
    Task<IEnumerable<WatchlistDto>> GetUserWatchlistAsync(int userId);
}
