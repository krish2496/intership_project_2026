using Tracker.Core.Entities;

namespace Tracker.Core.Interfaces;

public interface IRecommendationService
{
    Task<IEnumerable<Media>> GetRecommendationsForUserAsync(int userId);
}
