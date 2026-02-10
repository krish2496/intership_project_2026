using Microsoft.Extensions.DependencyInjection;
using Tracker.Core.Interfaces;
using Tracker.Services;

namespace Tracker.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IWatchlistService, WatchlistService>();
        services.AddScoped<IClubService, ClubService>();
        services.AddScoped<IDiscussionService, DiscussionService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ISocialService, SocialService>();
        services.AddScoped<IActivityService, ActivityService>();
        return services;
    }
}
