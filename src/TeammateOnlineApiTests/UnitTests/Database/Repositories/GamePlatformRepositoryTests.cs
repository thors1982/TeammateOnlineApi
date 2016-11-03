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

        [Fact]
        public void Add_Test()
        {
            var steam = new GamePlatform { Name = "Steam", Url = "http://store.steampowered.com" };

            var currentCount = context.GamePlatforms.Count();

            var addedGamePlatform = gamePlatformRepository.Add(steam);

            Assert.Equal(currentCount + 1, context.GamePlatforms.Count());
            var foundGamePlatform = context.GamePlatforms.FirstOrDefault(x => x.Id == addedGamePlatform.Id);
            Assert.Equal(steam.Name, foundGamePlatform.Name);
            Assert.Equal(steam.Url, foundGamePlatform.Url);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void FindById_Test(int position)
        {
            var testGamePlatform = GetAGamePlatform(position);

            var foundGamePlatform = gamePlatformRepository.FindById(testGamePlatform.Id);

            Assert.Equal(testGamePlatform, foundGamePlatform);
        }

        [Fact]
        public void GetAll_Test()
        {
            var allGamePlatforms = gamePlatformRepository.GetAll();

            Assert.Equal(context.GamePlatforms, allGamePlatforms);
        }

        [Fact]
        public void Remove_Test()
        {
            var testGamePlatform = GetAGamePlatform();

            var currentCount = context.GamePlatforms.Count();

            gamePlatformRepository.Remove(testGamePlatform);

            Assert.Equal(currentCount - 1, context.GamePlatforms.Count());
            Assert.Equal(null, context.GamePlatforms.FirstOrDefault(x => x.Id == testGamePlatform.Id));
        }

        [Fact]
        public void Update_Test()
        {
            var testGamePlatform = GetAGamePlatform();

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

        private GamePlatform GetAGamePlatform(int numberToSkip = 0)
        {
            return context.GamePlatforms.Skip(numberToSkip).First();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
