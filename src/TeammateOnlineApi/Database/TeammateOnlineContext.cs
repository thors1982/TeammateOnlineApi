using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database
{
    public class TeammateOnlineContext : DbContext
    {
        public DbSet<GamePlatform> GamePlatforms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<GameAccount> GameAccounts { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public TeammateOnlineContext(DbContextOptions<TeammateOnlineContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                    entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Creating indexes
            modelBuilder.Entity<UserProfile>().HasIndex(u => u.EmailAddress);
            modelBuilder.Entity<UserProfile>().HasIndex(u => u.FacebookId);
            modelBuilder.Entity<UserProfile>().HasIndex(u => u.GoogleId);

            modelBuilder.Entity<GameAccount>().HasIndex(g => g.UserProfileId);
            modelBuilder.Entity<GameAccount>().HasIndex(g => g.UserName);

            modelBuilder.Entity<Friend>().HasIndex(f => f.UserProfileId);

            modelBuilder.Entity<FriendRequest>().HasIndex(fr => fr.UserProfileId);
            modelBuilder.Entity<FriendRequest>().HasIndex(fr => fr.FriendUserProfileId);

            modelBuilder.Entity<Group>().HasIndex(g => g.Name);

            modelBuilder.Entity<UserGroup>().HasIndex(g => g.UserProfileId);
            modelBuilder.Entity<UserGroup>().HasIndex(g => g.GroupId);
        }
    }
}
