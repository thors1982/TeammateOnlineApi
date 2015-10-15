using Microsoft.Framework.DependencyInjection;
using System;
using System.Linq;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<TeammateOnlineContext>();
            
            if(!context.GameServices.Any())
            {
                var xbox = context.GameServices.Add(new GameService { Name = "Xbox Live", Url = "http://www.xbox.com/en-US/live" });
                var playstation = context.GameServices.Add(new GameService { Name = "PlatStation Network", Url = "https://www.playstationnetwork.com/home" });
                var steam = context.GameServices.Add(new GameService { Name = "Steam", Url = "http://store.steampowered.com" });
                var lol = context.GameServices.Add(new GameService { Name = "League of Legends", Url = "http://leagueoflegends.com" });
                var minecraft = context.GameServices.Add(new GameService { Name = "Minecraft", Url = "https://minecraft.net" });
                var origin = context.GameServices.Add(new GameService { Name = "Origin", Url = "https://www.origin.com/en-us/store" });
                var nintendo = context.GameServices.Add(new GameService { Name = "Nintendo Network", Url = "https://miiverse.nintendo.net" });

                context.SaveChanges();
            }
        }
    }
}
