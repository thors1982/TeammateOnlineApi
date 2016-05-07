using Microsoft.Data.Entity;
using System;
using System.Linq;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database
{
    public class TeammateOnlineContext : DbContext
    {
        public DbSet<GamePlatform> GamePlatforms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<GameAccount> GameAccounts { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public TeammateOnlineContext()
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
    }
}
