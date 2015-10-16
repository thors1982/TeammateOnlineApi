using Microsoft.Data.Entity;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database
{
    public class TeammateOnlineContext : DbContext
    {
        public DbSet<GamePlatform> GamePlatforms { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}