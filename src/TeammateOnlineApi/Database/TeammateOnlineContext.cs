using System;
using Microsoft.Data.Entity;
using TeammateOnlineApi.Models;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace TeammateOnlineApi.Database
{
    public class TeammateOnlineContext : DbContext
    {
        public DbSet<GamePlatform> GamePlatforms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<GameAccount> GameAccounts { get; set; }

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