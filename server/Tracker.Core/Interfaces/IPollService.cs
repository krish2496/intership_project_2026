using Tracker.Core.DTOs.Polls;

namespace Tracker.Core.Interfaces;

public interface IPollService
{
    Task<IEnumerable<PollDto>> GetActivePollsWithUserAsync(int? clubId, int userId);
    Task<PollDto> CreatePollAsync(int userId, CreatePollDto dto);
    Task<PollDto> VoteAsync(int userId, int pollId, VoteDto dto);
    Task<bool> DeletePollAsync(int userId, int pollId);
}
