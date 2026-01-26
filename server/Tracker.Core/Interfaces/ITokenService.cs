using Tracker.Core.Entities;

namespace Tracker.Core.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
