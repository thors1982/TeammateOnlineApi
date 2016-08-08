using Microsoft.Extensions.DependencyInjection;
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
            
            if(!context.GamePlatforms.Any())
            {
                var xbox = context.GamePlatforms.Add(new GamePlatform { Name = "Xbox Live", Url = "http://www.xbox.com/en-US/live" });
                var playstation = context.GamePlatforms.Add(new GamePlatform { Name = "PlatStation Network", Url = "https://www.playstationnetwork.com/home" });
                var steam = context.GamePlatforms.Add(new GamePlatform { Name = "Steam", Url = "http://store.steampowered.com" });
                var lol = context.GamePlatforms.Add(new GamePlatform { Name = "League of Legends", Url = "http://leagueoflegends.com" });
                var minecraft = context.GamePlatforms.Add(new GamePlatform { Name = "Minecraft", Url = "https://minecraft.net" });
                var origin = context.GamePlatforms.Add(new GamePlatform { Name = "Origin", Url = "https://www.origin.com/en-us/store" });
                var nintendo = context.GamePlatforms.Add(new GamePlatform { Name = "Nintendo Network", Url = "https://miiverse.nintendo.net" });
                var battlenet = context.GamePlatforms.Add(new GamePlatform { Name = "Battle.net", Url = "http://battle.net/en/" });
                var uplay = context.GamePlatforms.Add(new GamePlatform { Name = "Uplay", Url = "https://uplay.ubi.com" });
                var hirez = context.GamePlatforms.Add(new GamePlatform { Name = "Hi-Rez", Url = "https://www.hirezstudios.com/" });
                var mojang = context.GamePlatforms.Add(new GamePlatform { Name = "Mojang", Url = "https://mojang.com/" });

                context.SaveChanges();
            }
        }
    }
}
