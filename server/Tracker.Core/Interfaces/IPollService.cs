using Tracker.Core.DTOs.Polls;

namespace Tracker.Core.Interfaces;

public interface IPollService
{
    Task<IEnumerable<PollDto>> GetActivePollsAsync(int? clubId);
    Task<PollDto> CreatePollAsync(int userId, CreatePollDto dto);
    Task<PollDto> VoteAsync(int userId, int pollId, VoteDto dto);
    Task<bool> DeletePollAsync(int userId, int pollId);
}
