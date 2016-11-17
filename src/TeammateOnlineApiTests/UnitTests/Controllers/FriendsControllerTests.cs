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
    public class FriendsControllerTests : IDisposable
    {
        private Mock<IFriendRepository> mockFriendRepository { get; set; }

        private FriendsController controller { get; set; }

        private IEnumerable<Friend> friends { get; set; }

        private int idDoesNotExist { get; set; } = 7777;

        public FriendsControllerTests()
        {
            mockFriendRepository = new Mock<IFriendRepository>();

            controller = new FriendsController(mockFriendRepository.Object);

            friends = new List<Friend>
            {
                new Friend { Id = 1, UserProfileId = 1, FriendUserProfile = new UserProfile { }, FriendUserProfileId = 2, CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) },
                new Friend { Id = 2, UserProfileId = 1, FriendUserProfile = new UserProfile { }, FriendUserProfileId = 3, CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) }
            };
        }

        [Fact]
        public void GetCollectionReturnsAll()
        {
            var userProfileId = friends.First().UserProfileId;

            mockFriendRepository.Setup(repo => repo.GetAllByUserProfileId(userProfileId)).Returns(friends);

            var result = controller.GetCollection(userProfileId);
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(friends, okResult.Value);
        }

        [Fact]
        public void PostReturnsCreateAtResult()
        {
            var testFriend = friends.First();
            var userProfileId = testFriend.UserProfileId;

            mockFriendRepository.Setup(repo => repo.FindFriendOfAUser(userProfileId, testFriend.FriendUserProfileId)).Returns((Friend)null);
            mockFriendRepository.Setup(repo => repo.Add(It.IsAny<Friend>())).Returns(testFriend);

            var result = controller.Post(userProfileId, testFriend);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(testFriend, createdResult.Value);

            Assert.Equal("FriendDetail", createdResult.RouteName);
        }

        [Fact]
        public void PostReturnsAlreadyExists()
        {
            var testFriend = friends.First();
            var userProfileId = testFriend.UserProfileId;

            mockFriendRepository.Setup(repo => repo.FindFriendOfAUser(userProfileId, testFriend.FriendUserProfileId)).Returns(testFriend);
            
            var result = controller.Post(userProfileId, testFriend);

            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetDteailReturnsOkObjectResult()
        {
            var testFriend = friends.First();
            var userProfileId = testFriend.UserProfileId;

            mockFriendRepository.Setup(repo => repo.FindById(testFriend.Id)).Returns(testFriend);

            var result = controller.GetDetail(userProfileId, testFriend.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testFriend, okResult.Value);
        }

        [Fact]
        public void GetDetailFriendIdDoesNotExist_ReturnsNotFound()
        {
            var testFriend = friends.First();
            var userProfileId = testFriend.UserProfileId;

            mockFriendRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns((Friend)null);

            var result = controller.GetDetail(userProfileId, idDoesNotExist);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetDetailUserIdDoesNotExistReturnsNotFound()
        {
            var testFriend = friends.First();
            var userProfileId = testFriend.UserProfileId;

            mockFriendRepository.Setup(repo => repo.FindById(testFriend.Id)).Returns(testFriend);

            var result = controller.GetDetail(idDoesNotExist, testFriend.Id);

            //var notFound = Assert.IsType<NotFoundResult>(result);
            // This is what details return... but collections return 404 lets figure this out
        }

        [Fact]
        public void DeleteReturnsNoContentResult()
        {
            var testFriend = friends.First();
            var userProfileId = testFriend.UserProfileId;

            mockFriendRepository.Setup(repo => repo.FindById(testFriend.Id)).Returns(testFriend);
            mockFriendRepository.Setup(repo => repo.Remove(testFriend));

            var result = controller.Delete(userProfileId, testFriend.Id);

            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteFriendIdDoesNotExistReturnsNotFound()
        {
            var testFriend = friends.First();
            var userProfileId = testFriend.UserProfileId;

            mockFriendRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns((Friend)null);

            var result = controller.Delete(userProfileId, idDoesNotExist);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteUserIdDoesNotExistReturnsNotFound()
        {
            var testFriend = friends.First();
            var userProfileId = testFriend.UserProfileId;

            var wrongFriend = new Friend
            {
                Id = testFriend.Id,
                UserProfileId = testFriend.UserProfileId,
                FriendUserProfile = testFriend.FriendUserProfile,
                FriendUserProfileId = idDoesNotExist
            };

            mockFriendRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns(wrongFriend);

            var result = controller.Delete(userProfileId, testFriend.Id);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        public void Dispose()
        {
            controller.Dispose();
        }
    }
}
