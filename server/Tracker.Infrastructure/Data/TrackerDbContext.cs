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
    
    // Comment Likes
    public DbSet<CommentLike> CommentLikes { get; set; } = null!;
    
    // Advanced Clubs
    public DbSet<ClubEvent> ClubEvents { get; set; } = null!;
    public DbSet<ClubEventAttendee> ClubEventAttendees { get; set; } = null!;
    public DbSet<ClubList> ClubLists { get; set; } = null!;
    public DbSet<ClubListItem> ClubListItems { get; set; } = null!;
    public DbSet<ClubInvite> ClubInvites { get; set; } = null!;

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

        // ClubEvent configurations
        modelBuilder.Entity<ClubEvent>()
            .HasOne(ce => ce.Club)
            .WithMany(c => c.Events)
            .HasForeignKey(ce => ce.ClubId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClubEvent>()
            .HasOne(ce => ce.Creator)
            .WithMany()
            .HasForeignKey(ce => ce.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ClubEventAttendee>()
            .HasOne(cea => cea.Event)
            .WithMany(ce => ce.Attendees)
            .HasForeignKey(cea => cea.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClubEventAttendee>()
            .HasOne(cea => cea.User)
            .WithMany()
            .HasForeignKey(cea => cea.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Ensure user can only RSVP once per event
        modelBuilder.Entity<ClubEventAttendee>()
            .HasIndex(cea => new { cea.EventId, cea.UserId })
            .IsUnique();

        // ClubList configurations
        modelBuilder.Entity<ClubList>()
            .HasOne(cl => cl.Club)
            .WithMany(c => c.Lists)
            .HasForeignKey(cl => cl.ClubId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClubList>()
            .HasOne(cl => cl.Creator)
            .WithMany()
            .HasForeignKey(cl => cl.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ClubListItem>()
            .HasOne(cli => cli.List)
            .WithMany(cl => cl.Items)
            .HasForeignKey(cli => cli.ListId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClubListItem>()
            .HasOne(cli => cli.AddedBy)
            .WithMany()
            .HasForeignKey(cli => cli.AddedById)
            .OnDelete(DeleteBehavior.NoAction);

        // ClubInvite configurations
        modelBuilder.Entity<ClubInvite>()
            .HasOne(ci => ci.Club)
            .WithMany(c => c.Invites)
            .HasForeignKey(ci => ci.ClubId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClubInvite>()
            .HasOne(ci => ci.Inviter)
            .WithMany()
            .HasForeignKey(ci => ci.InviterId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ClubInvite>()
            .HasOne(ci => ci.Invitee)
            .WithMany()
            .HasForeignKey(ci => ci.InviteeId)
            .OnDelete(DeleteBehavior.NoAction);

        // Ensure user can only be invited once per club
        modelBuilder.Entity<ClubInvite>()
            .HasIndex(ci => new { ci.ClubId, ci.InviteeId })
            .IsUnique();
    }
}
