namespace Tracker.Core.DTOs.Activity;

public class ActivityDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string ActivityType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string? Metadata { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ActivityCommentDto> RecentComments { get; set; } = new();
}

public class CreateActivityDto
{
    public string ActivityType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string? Metadata { get; set; }
}

public class ActivityCommentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateCommentDto
{
    public string Content { get; set; } = string.Empty;
}

public class UserFollowDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsFollowing { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
}
