using Microsoft.EntityFrameworkCore;
using Tracker.Core.DTOs.Clubs;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class ClubService : IClubService
{
    private readonly TrackerDbContext _context;

    public ClubService(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClubDto>> GetAllClubsAsync()
    {
        var clubs = await _context.Clubs
            .Include(c => c.Owner)
            .ToListAsync();

        return clubs.Select(MapToDto);
    }

    public async Task<ClubDto> GetClubByIdAsync(int id)
    {
        var club = await _context.Clubs
            .Include(c => c.Owner)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (club == null) throw new Exception("Club not found");

        return MapToDto(club);
    }

    public async Task<ClubDto> CreateClubAsync(int userId, CreateClubDto dto)
    {
        var club = new Club
        {
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = userId
        };

        _context.Clubs.Add(club);
        await _context.SaveChangesAsync();

        // Load owner for DTO
        await _context.Entry(club).Reference(c => c.Owner).LoadAsync();

        return MapToDto(club);
    }

    public async Task<bool> DeleteClubAsync(int userId, int clubId)
    {
        var club = await _context.Clubs.FindAsync(clubId);
        if (club == null) return false;

        if (club.OwnerId != userId) throw new UnauthorizedAccessException("Not the owner");

        _context.Clubs.Remove(club);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ClubDto MapToDto(Club club)
    {
        return new ClubDto
        {
            Id = club.Id,
            Name = club.Name,
            Description = club.Description,
            OwnerId = club.OwnerId,
            OwnerName = club.Owner.Username,
            MemberCount = 1 // Basic implementation: only owner initially
        };
    }
}
