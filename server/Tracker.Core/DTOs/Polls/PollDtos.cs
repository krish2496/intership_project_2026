using System.ComponentModel.DataAnnotations;

namespace Tracker.Core.DTOs.Polls;

public class CreatePollDto
{
    [Required]
    public string Question { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public List<string> Options { get; set; } = new();

    public int? ClubId { get; set; }
}

public class PollDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string CreatorName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int? ClubId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<PollOptionDto> Options { get; set; } = new();
    public int? UserVotedOptionId { get; set; } // ID of option user voted for, null if none
    public int TotalVotes { get; set; }
}

public class PollOptionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int VoteCount { get; set; }
}

public class VoteDto
{
    public int OptionId { get; set; }
}
