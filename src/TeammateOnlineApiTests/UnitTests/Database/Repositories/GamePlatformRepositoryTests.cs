using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Database.Repositories;
using Xunit;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApiTests.UnitTests.Database.Repositories
{
    public class GamePlatformRepositoryTests : IDisposable
    {
        private TeammateOnlineContext context;

        private GamePlatformRepository gamePlatformRepository;
        
        public GamePlatformRepositoryTests()
        {
            var db = new DbContextOptionsBuilder<TeammateOnlineContext>();
            db.UseInMemoryDatabase();
            context = new TeammateOnlineContext(db.Options);

            gamePlatformRepository = new GamePlatformRepository(context);

            SeedData();
        }

        [Fact(DisplayName = "GamePlatformRepository - Adding Game Platform")]
        public void Add()
        {
            var steam = new GamePlatform { Name = "Steam", Url = "http://store.steampowered.com" };

            var currentCount = context.GamePlatforms.Count();

            var addedGamePlatform = gamePlatformRepository.Add(steam);

            Assert.Equal(currentCount + 1, context.GamePlatforms.Count());
            var foundGamePlatform = context.GamePlatforms.FirstOrDefault(x => x.Id == addedGamePlatform.Id);
            Assert.Equal(steam.Name, foundGamePlatform.Name);
            Assert.Equal(steam.Url, foundGamePlatform.Url);
        }

        [Theory(DisplayName = "GamePlatformRepository - Find Game Platform by Id")]
        [InlineData(0)]
        [InlineData(1)]
        public void FindById(int numberToSkip)
        {
            var testGamePlatform = GetGamePlatform(numberToSkip);

            var foundGamePlatform = gamePlatformRepository.FindById(testGamePlatform.Id);

            Assert.Equal(testGamePlatform, foundGamePlatform);
        }

        [Fact(DisplayName = "GamePlatformRepository - Get all Game Platforms")]
        public void GetAll()
        {
            var allGamePlatforms = gamePlatformRepository.GetAll();

            Assert.Equal(context.GamePlatforms, allGamePlatforms);
        }

        [Fact(DisplayName = "GamePlatformRepository - Removing a Game Platform")]
        public void Remove()
        {
            var testGamePlatform = GetGamePlatform();

            var currentCount = context.GamePlatforms.Count();

            gamePlatformRepository.Remove(testGamePlatform);

            Assert.Equal(currentCount - 1, context.GamePlatforms.Count());
            Assert.Equal(null, context.GamePlatforms.FirstOrDefault(x => x.Id == testGamePlatform.Id));
        }

        [Fact(DisplayName = "GamePlatformRepository - Update a Game Platform")]
        public void Update()
        {
            var testGamePlatform = GetGamePlatform();

            testGamePlatform.Name = "UpdatedName";
            testGamePlatform.Url = "http://updated.url";

            gamePlatformRepository.Update(testGamePlatform);

            Assert.Equal(testGamePlatform, context.GamePlatforms.FirstOrDefault(x => x.Id == testGamePlatform.Id));
        }

        private void SeedData()
        {
            if (!context.GamePlatforms.Any())
            {
                // Seed data
                context.GamePlatforms.Add(new GamePlatform { Name = "Xbox Live", Url = "http://www.xbox.com/en-US/live" });
                context.GamePlatforms.Add(new GamePlatform { Name = "PlayStation Network", Url = "https://www.playstationnetwork.com/home" });
                context.SaveChanges();
            }
        }

        private GamePlatform GetGamePlatform(int numberToSkip = 0)
        {
            return context.GamePlatforms.Skip(numberToSkip).First();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
