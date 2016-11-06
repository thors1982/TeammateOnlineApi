using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
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
                // Some default user profiles
                var ironman = context.UserProfiles.Add(new UserProfile { FirstName = "Tony", LastName = "Stark", EmailAddress = "tony.stark@ironman.com", FacebookId = "tony.stark.ironman.facebookid", GoogleId = "tony.stark.ironman.googleid" });
                var captainAmerica = context.UserProfiles.Add(new UserProfile { FirstName = "Steve", LastName = "Rogers", EmailAddress = "steve.rogers@captainamerica.com", FacebookId = "steve.rogers.captainamerica.facebookid", GoogleId = "steve.rogers.captainamerica.googleid" });
                var hulk = context.UserProfiles.Add(new UserProfile { FirstName = "Bruce", LastName = "Banner", EmailAddress = "bruce.banner@hulk.com", FacebookId = "bruce.banner.hulk.facebookid", GoogleId = "bruce.banner.hulk.googleid" });
                var batman = context.UserProfiles.Add(new UserProfile { FirstName = "Bruce", LastName = "Wayne", EmailAddress = "brace.wayne@batman.com", FacebookId = "brace.wayne.batman.facbeookid", GoogleId = "brace.wayne.batman.googleid" });

                context.SaveChanges();

                // Some default game accounts
                var ironmanXbox = context.GameAccounts.Add(new GameAccount { UserName = "IronmanXbox", UserProfileId = ironman.Entity.Id, GamePlatformId = 1 });
                var ironmanPlaystation = context.GameAccounts.Add(new GameAccount { UserName = "IronmanPlaystation", UserProfileId = ironman.Entity.Id, GamePlatformId = 2 });
                var ironmanSteam = context.GameAccounts.Add(new GameAccount { UserName = "IronmanSteam", UserProfileId = ironman.Entity.Id, GamePlatformId = 3 });

                var captainAmericaXbox = context.GameAccounts.Add(new GameAccount { UserName = "CaptainAmericaXbox", UserProfileId = captainAmerica.Entity.Id, GamePlatformId = 1 });
                var captainAmericaPlaystation = context.GameAccounts.Add(new GameAccount { UserName = "CaptainAmericaPlaystation", UserProfileId = captainAmerica.Entity.Id, GamePlatformId = 2 });
                var captainAmericaSteam = context.GameAccounts.Add(new GameAccount { UserName = "CaptainAmericaSteam", UserProfileId = captainAmerica.Entity.Id, GamePlatformId = 3 });

                var hulkXbox = context.GameAccounts.Add(new GameAccount { UserName = "HulkXbox", UserProfileId = hulk.Entity.Id, GamePlatformId = 1 });
                var hulkPlaystation = context.GameAccounts.Add(new GameAccount { UserName = "HulkPlaystation", UserProfileId = hulk.Entity.Id, GamePlatformId = 2 });
                var hulkSteam = context.GameAccounts.Add(new GameAccount { UserName = "HulkSteam", UserProfileId = hulk.Entity.Id, GamePlatformId = 3 });

                var batmanXbox = context.GameAccounts.Add(new GameAccount { UserName = "BatmanXbox", UserProfileId = batman.Entity.Id, GamePlatformId = 1 });
                var batmanPlaystation = context.GameAccounts.Add(new GameAccount { UserName = "BatmanPlaystation", UserProfileId = batman.Entity.Id, GamePlatformId = 2 });
                var batmanSteam = context.GameAccounts.Add(new GameAccount { UserName = "BatmanSteam", UserProfileId = batman.Entity.Id, GamePlatformId = 3 });

                // Some default friends
                var ironmansFriends = context.Friends.Add(new Friend { UserProfileId = ironman.Entity.Id, FriendUserProfileId = captainAmerica.Entity.Id });
                var captainAmericaFriends = context.Friends.Add(new Friend { UserProfileId = captainAmerica.Entity.Id, FriendUserProfileId = ironman.Entity.Id });

                context.SaveChanges();
            }
        }
    }
}