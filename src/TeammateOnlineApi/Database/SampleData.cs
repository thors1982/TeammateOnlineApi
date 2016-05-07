using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database
{
    public static class SampleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<TeammateOnlineContext>();

            if (!context.UserProfiles.Any())
            {
                var ironman = context.UserProfiles.Add(new UserProfile { FirstName = "Tony", LastName = "Stark", EmailAddress = "tony.stark@ironman.com" });
                var ironmanXbox = context.GameAccounts.Add(new GameAccount { UserName = "IronmanXbox", UserProfileId = ironman.Entity.Id, GamePlatformId = 1 });
                var ironmanPlaystation = context.GameAccounts.Add(new GameAccount { UserName = "IronmanPlaystation", UserProfileId = ironman.Entity.Id, GamePlatformId = 2 });
                var ironmanSteam = context.GameAccounts.Add(new GameAccount { UserName = "IronmanSteam", UserProfileId = ironman.Entity.Id, GamePlatformId = 3 });

                var captainamerica = context.UserProfiles.Add(new UserProfile { FirstName = "Steve", LastName = "Rogers", EmailAddress = "steve.rogers@captainamerica.com" });

                var hulk = context.UserProfiles.Add(new UserProfile { FirstName = "Bruce", LastName = "Banner", EmailAddress = "bruce.banner@hulk.com" });

                var batman = context.UserProfiles.Add(new UserProfile { FirstName = "Bruce", LastName = "Wayne", EmailAddress = "brace.wayne@batman.com" });

                context.SaveChanges();
            }
        }
    }
}