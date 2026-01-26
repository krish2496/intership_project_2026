using Tracker.Core.Entities;

namespace Tracker.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}
