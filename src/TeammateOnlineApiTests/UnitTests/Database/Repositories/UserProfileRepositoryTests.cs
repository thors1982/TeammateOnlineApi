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
    public class UserProfileRepositoryTests : IDisposable
    {
        private TeammateOnlineContext context;

        private UserProfileRepository userProfileRepository;

        public UserProfileRepositoryTests()
        {
            var db = new DbContextOptionsBuilder<TeammateOnlineContext>();
            db.UseInMemoryDatabase();
            context = new TeammateOnlineContext(db.Options);

            userProfileRepository = new UserProfileRepository(context);

            SeedData();
        }

        [Fact(DisplayName = "UserProfileRepository - Adding User Profile")]
        public void Add()
        {
            var hulk = new UserProfile { FirstName = "Bruce", LastName = "Banner", EmailAddress = "bruce.banner@hulk.com", FacebookId = "bruce.banner.hulk.facebookid", GoogleId = "bruce.banner.hulk.googleid" };

            var currentCount = context.UserProfiles.Count();

            var addUserProfile = userProfileRepository.Add(hulk);

            Assert.Equal(currentCount + 1, context.UserProfiles.Count());
            var foundUserProfile = context.UserProfiles.FirstOrDefault(x => x.Id == addUserProfile.Id);
            Assert.Equal(hulk.FirstName, foundUserProfile.FirstName);
            Assert.Equal(hulk.LastName, foundUserProfile.LastName);
            Assert.Equal(hulk.EmailAddress, foundUserProfile.EmailAddress);
            Assert.Equal(hulk.FacebookId, foundUserProfile.FacebookId);
            Assert.Equal(hulk.GoogleId, foundUserProfile.GoogleId);
        }

        [Theory(DisplayName = "UserProfileRepository - Find User Profile by Id")]
        [InlineData(0)]
        [InlineData(1)]
        public void FindById(int numberToSkip)
        {
            var testUserProfile = GetUserProfile(numberToSkip);

            var foundGamePlatform = userProfileRepository.FindById(testUserProfile.Id);

            Assert.Equal(testUserProfile, foundGamePlatform);
        }

        [Fact(DisplayName = "UserProfileRepository - Find User Profile by Email Address")]
        public void FindByEmailAddress()
        {
            var testUserProfile = GetUserProfile();
            
            var foundGamePlatform = userProfileRepository.FindByEmailAddress(testUserProfile.EmailAddress);

            Assert.Equal(testUserProfile, foundGamePlatform);
        }

        [Fact(DisplayName = "UserProfileRepository - Find User Profile by Google Id")]
        public void FindByGoogleId()
        {
            var testUserProfile = GetUserProfile();

            var foundGamePlatform = userProfileRepository.FindByGoogleId(testUserProfile.GoogleId);

            Assert.Equal(testUserProfile, foundGamePlatform);
        }

        [Fact(DisplayName = "UserProfileRepository - Find User Profile by Facebook Id")]
        public void FindByFacebookId()
        {
            var testUserProfile = GetUserProfile();

            var foundGamePlatform = userProfileRepository.FindByFacebookId(testUserProfile.FacebookId);

            Assert.Equal(testUserProfile, foundGamePlatform);
        }

        [Fact(DisplayName = "UserProfileRepository - Get all User Profiles")]
        public void GetAll()
        {
            var allUserProfiles = userProfileRepository.GetAll();

            Assert.Equal(context.UserProfiles, allUserProfiles);
        }

        [Fact(DisplayName = "UserProfileRepository - Update a User Profile")]
        public void Update()
        {
            var testUserProfile = GetUserProfile();

            testUserProfile.FirstName = "UpdatedFirstName";
            testUserProfile.LastName = "UpdatedLastName";
            testUserProfile.EmailAddress = "updated@emailaddress.com";
            testUserProfile.GoogleId = "updated.googleid";
            testUserProfile.FacebookId = "updated.facebookeid";

            userProfileRepository.Update(testUserProfile);

            Assert.Equal(testUserProfile, context.UserProfiles.FirstOrDefault(x => x.Id == testUserProfile.Id));
        }

        [Fact(DisplayName = "UserProfileRepository - Query User Profiles")]
        public void Query()
        {
            var testUserProfile = GetUserProfile();

            var query = testUserProfile.EmailAddress.Substring(2, 10);

            var foundResults = userProfileRepository.Query(query);

            Assert.Equal(testUserProfile, foundResults.First());

        }

        private void SeedData()
        {
            if (!context.UserProfiles.Any())
            {
                // Seed data
                context.UserProfiles.Add(new UserProfile { FirstName = "Tony", LastName = "Stark", EmailAddress = "tony.stark@ironman.com", FacebookId = "tony.stark.ironman.facebookid", GoogleId = "tony.stark.ironman.googleid", CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) });
                context.UserProfiles.Add(new UserProfile { FirstName = "Steve", LastName = "Rogers", EmailAddress = "steve.rogers@captainamerica.com", FacebookId = "steve.rogers.captainameria.facebookid", GoogleId = "steve.rogers.captainameria.googleid", CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) });
                context.SaveChanges();
            }
        }

        private UserProfile GetUserProfile(int numberToSkip = 0)
        {
            return context.UserProfiles.Skip(numberToSkip).First();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
