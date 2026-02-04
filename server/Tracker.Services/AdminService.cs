using Microsoft.EntityFrameworkCore;
using Tracker.Core.DTOs;
using Tracker.Core.DTOs.Admin;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Services;

public class AdminService : IAdminService
{
    private readonly TrackerDbContext _context;

    public AdminService(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task<SystemStatsDto> GetSystemStatsAsync()
    {
        return new SystemStatsDto
        {
            TotalUsers = await _context.Users.CountAsync(),
            TotalMediaTracked = await _context.Watchlists.CountAsync(),
            TotalClubs = await _context.Clubs.CountAsync(),
            TotalDiscussions = await _context.Discussions.CountAsync(),
            TotalPolls = await _context.Polls.CountAsync()
        };
    }

    public async Task<PublicStatsDto> GetPublicStatsAsync()
    {
        return new PublicStatsDto
        {
            TotalUsers = await _context.Users.CountAsync(),
            TotalClubs = await _context.Clubs.CountAsync(),
            TotalAnime = 1000 // Can be made dynamic if needed
        };
    }

    public async Task<IEnumerable<UserAdminDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();

        return users.Select(u => new UserAdminDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role,
            CreatedAt = u.CreatedAt
        });
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        // Prevent deleting last admin or self if needed, but for now simple delete
        if (user.Role == "Admin")
        {
             // Optional: prevent deleting admins via API
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
