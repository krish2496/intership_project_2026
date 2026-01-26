using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Core.Interfaces;
using Tracker.Infrastructure.Auth;
using Tracker.Infrastructure.Data;
using Tracker.Infrastructure.Data.Repositories;
using Tracker.Infrastructure.External.Jikan;
using Tracker.Infrastructure.External.TMDB;

namespace Tracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<TrackerDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddHttpClient<IAnimeService, JikanService>();
        services.AddHttpClient<ITvSeriesService, TmdbService>();

        return services;
    }
}
