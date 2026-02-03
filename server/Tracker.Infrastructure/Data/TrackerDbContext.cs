using Microsoft.EntityFrameworkCore;
using Tracker.Core.Entities;

namespace Tracker.Infrastructure.Data;

public class TrackerDbContext : DbContext
{
    public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Media> Media { get; set; } = null!;
    public DbSet<Watchlist> Watchlists { get; set; } = null!;
    public DbSet<Club> Clubs { get; set; } = null!;
    public DbSet<Discussion> Discussions { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Poll> Polls { get; set; } = null!;
    public DbSet<PollOption> PollOptions { get; set; } = null!;
    public DbSet<PollVote> PollVotes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User - Username/Email unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Watchlist - User/Media relationship
        modelBuilder.Entity<Watchlist>()
            .HasOne(w => w.User)
            .WithMany(u => u.Watchlist)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Watchlist>()
            .HasOne(w => w.Media)
            .WithMany(m => m.WatchlistEntries)
            .HasForeignKey(w => w.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Club - Owner relationship
        modelBuilder.Entity<Club>()
            .HasOne(c => c.Owner)
            .WithMany(u => u.OwnedClubs)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Discussion - Club/User
        modelBuilder.Entity<Discussion>()
            .HasOne(d => d.Club)
            .WithMany(c => c.Discussions)
            .HasForeignKey(d => d.ClubId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Discussion>()
            .HasOne(d => d.User)
            .WithMany(u => u.Discussions)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Comment - Discussion/User/Parent
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Discussion)
            .WithMany(d => d.Comments)
            .HasForeignKey(c => c.DiscussionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting parent deletes replies

        // Polls configuration
        modelBuilder.Entity<Poll>()
            .HasOne(p => p.Creator)
            .WithMany()
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PollOption>()
            .HasOne(o => o.Poll)
            .WithMany(p => p.Options)
            .HasForeignKey(o => o.PollId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PollVote>()
            .HasOne(v => v.Poll)
            .WithMany(p => p.Votes)
            .HasForeignKey(v => v.PollId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PollVote>()
            .HasOne(v => v.PollOption)
            .WithMany(o => o.Votes)
            .HasForeignKey(v => v.PollOptionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PollVote>()
            .HasOne(v => v.User)
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure user can only vote once per poll
        modelBuilder.Entity<PollVote>()
            .HasIndex(v => new { v.PollId, v.UserId })
            .IsUnique();
    }
}
