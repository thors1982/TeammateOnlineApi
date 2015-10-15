using Microsoft.Data.Entity;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database
{
    public class TeammateOnlineContext : DbContext
    {
        public DbSet<GameService> GameServices { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}