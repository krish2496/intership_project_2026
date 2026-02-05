using Microsoft.EntityFrameworkCore;
using Tracker.Core.Entities;
using Tracker.Core.Enums;
using Tracker.Infrastructure.Data;

namespace Tracker.API.Data;

public static class DbSeeder
{
    public static async Task SeedSampleDataAsync(TrackerDbContext context)
    {
        // Seed Admin User
        var adminEmail = "adminuser@tracker.com";
        if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
        {
            Console.WriteLine("Seeding Admin User...");
            var adminUser = new User
            {
                Username = "Admin",
                Email = adminEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123!"),
                Role = "Admin",
                ThemePreference = "Dark"
            };
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
            Console.WriteLine("Admin User seeded successfully!");
        }

        // Check if we already have anime
        if (await context.Media.AnyAsync())
        {
            Console.WriteLine("Database already has anime data. Skipping anime seed.");
            return;
        }

        Console.WriteLine("Seeding sample anime data...");
        // Add sample anime (rest of the file...)

        // Add sample anime
        var animeList = new List<Media>
        {
            new Media
            {
                Title = "Attack on Titan",
                Description = "Humanity fights for survival against giant humanoid creatures called Titans.",
                Type = MediaType.Anime,
                TotalEpisodes = 75,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/10/47347.jpg"
            },
            new Media
            {
                Title = "Death Note",
                Description = "A high school student discovers a supernatural notebook that allows him to kill anyone.",
                Type = MediaType.Anime,
                TotalEpisodes = 37,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/9/9453.jpg"
            },
            new Media
            {
                Title = "Fullmetal Alchemist: Brotherhood",
                Description = "Two brothers search for the Philosopher's Stone to restore their bodies.",
                Type = MediaType.Anime,
                TotalEpisodes = 64,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/1223/96541.jpg"
            },
            new Media
            {
                Title = "Steins;Gate",
                Description = "A group of friends discover time travel through a microwave.",
                Type = MediaType.Anime,
                TotalEpisodes = 24,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/5/73199.jpg"
            },
            new Media
            {
                Title = "One Punch Man",
                Description = "A hero who can defeat any opponent with a single punch seeks a worthy challenge.",
                Type = MediaType.Anime,
                TotalEpisodes = 24,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/12/76049.jpg"
            },
            new Media
            {
                Title = "Demon Slayer",
                Description = "A boy becomes a demon slayer to avenge his family and cure his sister.",
                Type = MediaType.Anime,
                TotalEpisodes = 26,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/1286/99889.jpg"
            },
            new Media
            {
                Title = "My Hero Academia",
                Description = "A boy born without superpowers dreams of becoming a hero.",
                Type = MediaType.Anime,
                TotalEpisodes = 113,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/10/78745.jpg"
            },
            new Media
            {
                Title = "Naruto",
                Description = "A young ninja seeks recognition and dreams of becoming the Hokage.",
                Type = MediaType.Anime,
                TotalEpisodes = 220,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/13/17405.jpg"
            },
            new Media
            {
                Title = "Sword Art Online",
                Description = "Players trapped in a virtual reality MMORPG must clear the game to escape.",
                Type = MediaType.Anime,
                TotalEpisodes = 25,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/11/39717.jpg"
            },
            new Media
            {
                Title = "Code Geass",
                Description = "An exiled prince gains the power to control minds and leads a rebellion.",
                Type = MediaType.Anime,
                TotalEpisodes = 50,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/5/50331.jpg"
            },
            new Media
            {
                Title = "Hunter x Hunter",
                Description = "A young boy searches for his father and becomes a Hunter.",
                Type = MediaType.Anime,
                TotalEpisodes = 148,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/11/33657.jpg"
            },
            new Media
            {
                Title = "Tokyo Ghoul",
                Description = "A college student becomes a half-ghoul and struggles to survive.",
                Type = MediaType.Anime,
                TotalEpisodes = 12,
                CoverImageUrl = "https://cdn.myanimelist.net/images/anime/5/64449.jpg"
            }
        };

        await context.Media.AddRangeAsync(animeList);
        await context.SaveChangesAsync();

        Console.WriteLine($"Successfully seeded {animeList.Count} anime!");
    }
}
