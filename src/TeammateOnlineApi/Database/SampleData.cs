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
                var captainamerica = context.UserProfiles.Add(new UserProfile { FirstName = "Steve", LastName = "Rogers", EmailAddress = "steve.rogers@captainamerica.com" });
                var hulk = context.UserProfiles.Add(new UserProfile { FirstName = "Bruce", LastName = "Banner", EmailAddress = "bruce.banner@hulk.com" });

                context.SaveChanges();
            }
        }
    }
}