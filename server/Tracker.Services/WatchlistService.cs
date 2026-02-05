using Microsoft.EntityFrameworkCore;
using Tracker.Core.DTOs;
using Tracker.Core.DTOs.Watchlist;
using Tracker.Core.Entities;
using Tracker.Core.Enums;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class WatchlistService : IWatchlistService
{
    private readonly TrackerDbContext _context;

    public WatchlistService(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task<WatchlistDto> AddToWatchlistAsync(int userId, AddToWatchlistDto dto)
    {
        // 1. Check if Media exists in local DB
        var media = await _context.Media
            .FirstOrDefaultAsync(m => m.ExternalId == dto.MediaExternalId && m.Type == dto.MediaType);

        if (media == null)
        {
            // Create new Media entry
            media = new Media
            {
                Title = dto.Title,
                Type = dto.MediaType,
                ExternalId = dto.MediaExternalId,
                CoverImageUrl = dto.CoverImageUrl,
                TotalEpisodes = dto.TotalEpisodes,
                Description = dto.Description
            };
            _context.Media.Add(media);
            await _context.SaveChangesAsync();
        }

        // 2. Check if already in watchlist
        var existingEntry = await _context.Watchlists
            .FirstOrDefaultAsync(w => w.UserId == userId && w.MediaId == media.Id);

        if (existingEntry != null)
        {
            throw new Exception("Media already in watchlist");
        }

        // 3. Create Watchlist entry
        var watchlistEntry = new Watchlist
        {
            UserId = userId,
            MediaId = media.Id,
            Status = dto.Status,
            Progress = 0,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Watchlists.Add(watchlistEntry);
        await _context.SaveChangesAsync();

        return MapToDto(watchlistEntry, media);
    }

    public async Task<WatchlistDto?> UpdateWatchlistAsync(int userId, int watchlistId, UpdateWatchlistDto dto)
    {
        var entry = await _context.Watchlists
            .Include(w => w.Media)
            .FirstOrDefaultAsync(w => w.Id == watchlistId && w.UserId == userId);

        if (entry == null) return null;

        var oldStatus = entry.Status;
        var oldRating = entry.Rating;

        if (dto.Status.HasValue) entry.Status = dto.Status.Value;
        if (dto.Progress.HasValue) entry.Progress = dto.Progress.Value;
        if (dto.Rating.HasValue) entry.Rating = dto.Rating.Value;

        entry.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(entry, entry.Media);
    }

    public async Task<bool> RemoveFromWatchlistAsync(int userId, int watchlistId)
    {
        var entry = await _context.Watchlists
            .FirstOrDefaultAsync(w => w.Id == watchlistId && w.UserId == userId);

        if (entry == null) return false;

        _context.Watchlists.Remove(entry);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<WatchlistDto>> GetUserWatchlistAsync(int userId)
    {
        var list = await _context.Watchlists
            .Include(w => w.Media)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.UpdatedAt)
            .ToListAsync();

        return list.Select(w => MapToDto(w, w.Media));
    }

    private static WatchlistDto MapToDto(Watchlist watchlist, Media media)
    {
        return new WatchlistDto
        {
            Id = watchlist.Id,
            MediaId = media.Id,
            Media = new MediaDto
            {
                Id = media.Id,
                Title = media.Title,
                ExternalId = media.ExternalId,
                Type = media.Type,
                TotalEpisodes = media.TotalEpisodes,
                Description = media.Description,
                CoverImageUrl = media.CoverImageUrl
            },
            Status = watchlist.Status,
            Progress = watchlist.Progress,
            Rating = watchlist.Rating
        };
    }
}
