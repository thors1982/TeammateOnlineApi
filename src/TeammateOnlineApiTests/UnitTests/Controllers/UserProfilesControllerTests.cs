using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TeammateOnlineApi.Controllers;
using TeammateOnlineApi.Models;
using TeammateOnlineApi.Database.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TeammateOnlineApiTests.UnitTests.Controllers
{
    public class UserProfilesControllerTests : IDisposable
    {
        private Mock<IUserProfileRepository> mockUserProfileRepository { get; set; }

        private UserProfilesController controller { get; set; }

        private IEnumerable<UserProfile> userProfiles { get; set; }

        private int idDoesNotExist { get; set; } = 7777;

        public UserProfilesControllerTests()
        {
            mockUserProfileRepository = new Mock<IUserProfileRepository>();

            controller = new UserProfilesController(mockUserProfileRepository.Object);

            userProfiles = new List<UserProfile>
            {
                new UserProfile { Id = 1, FirstName = "Tony", LastName = "Stark", EmailAddress = "tony.stark@ironman.com", FacebookId = "tony.stark.ironman.facebookid", GoogleId = "tony.stark.ironman.googleid", CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1)}
            };
        }

        [Fact]
        public void GetCollectionReturnsAll()
        {
            mockUserProfileRepository.Setup(repo => repo.GetAll()).Returns(userProfiles);

            var result = controller.GetCollection();
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(userProfiles, okResult.Value);
        }

        [Fact]
        public void PostReturnsCreateAtResult()
        {
            var testUserProfile = userProfiles.First();

            mockUserProfileRepository.Setup(repo => repo.Add(It.IsAny<UserProfile>())).Returns(testUserProfile);

            var result = controller.Post(testUserProfile);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(testUserProfile, createdResult.Value);

            Assert.Equal("UserProfileDetail", createdResult.RouteName);
        }

        [Fact]
        public void PostReturnsEmailAddressAlreadyTaken()
        {
            var testUserProfile = userProfiles.First();

            mockUserProfileRepository.Setup(repo => repo.Add(It.IsAny<UserProfile>())).Returns(testUserProfile);
            mockUserProfileRepository.Setup(repo => repo.FindByEmailAddress(testUserProfile.EmailAddress)).Returns(testUserProfile);

            var result = controller.Post(testUserProfile);

            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetDteailReturnsOkObjectResult()
        {
            var testUserProfile = userProfiles.First();
            mockUserProfileRepository.Setup(repo => repo.FindById(testUserProfile.Id)).Returns(testUserProfile);

            var result = controller.GetDetail(testUserProfile.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testUserProfile, okResult.Value);
        }

        [Fact]
        public void GetDetailReturnsNotFound()
        {
            mockUserProfileRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns((UserProfile)null);

            var result = controller.GetDetail(idDoesNotExist);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PutReturnsOkResult()
        {
            var testUserProfile = userProfiles.First();

            mockUserProfileRepository.Setup(repo => repo.FindById(testUserProfile.Id)).Returns(testUserProfile);
            mockUserProfileRepository.Setup(repo => repo.Update(testUserProfile));

            var updatedUserProfile = new UserProfile
                {
                    Id = testUserProfile.Id,
                    GoogleId = testUserProfile.GoogleId,
                    FacebookId = testUserProfile.FacebookId,
                    EmailAddress = "test@testing.com",
                    FirstName = "First",
                    LastName = "Last"
                };

            var result = controller.Put(testUserProfile.Id, updatedUserProfile);

            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void PutReturnsNotFound()
        {
            var testUserProfile = userProfiles.First();

            mockUserProfileRepository.Setup(repo => repo.FindById(testUserProfile.Id)).Returns((UserProfile)null);

            var updatedUserProfile = new UserProfile
            {
                GoogleId = testUserProfile.GoogleId,
                FacebookId = testUserProfile.FacebookId,
                EmailAddress = "test@testing.com",
                FirstName = "First",
                LastName = "Last",
                Id = idDoesNotExist
            };

            var result = controller.Put(testUserProfile.Id, updatedUserProfile);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PutReturnsEmailAddressAlreadyTaken()
        {
            var testUserProfile = userProfiles.First();

            var updatedUserProfile = new UserProfile
                {
                    Id = testUserProfile.Id,
                    GoogleId = testUserProfile.GoogleId,
                    FacebookId = testUserProfile.FacebookId,
                    EmailAddress = "test@testing.com",
                    FirstName = "First",
                    LastName = "Last"
                };

            mockUserProfileRepository.Setup(repo => repo.FindById(testUserProfile.Id)).Returns(testUserProfile);
            mockUserProfileRepository.Setup(repo => repo.FindByEmailAddress(updatedUserProfile.EmailAddress)).Returns(updatedUserProfile);

            var result = controller.Put(testUserProfile.Id, updatedUserProfile);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        public void Dispose()
        {
            controller.Dispose();
        }
    }
}
