using Microsoft.EntityFrameworkCore;
using Tracker.Core.Entities;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Data;

namespace Tracker.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TrackerDbContext _context;

    public UserRepository(TrackerDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
