namespace Tracker.Core.DTOs;

public class ActivityDto
{
    public int Id { get; set; }
    public ActivityUserDto User { get; set; }
    public ActivityMediaDto? Media { get; set; }
    public int Type { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ActivityUserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
}

public class ActivityMediaDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CoverImageUrl { get; set; }
    public int? ExternalId { get; set; }
    public int Type { get; set; }
}
