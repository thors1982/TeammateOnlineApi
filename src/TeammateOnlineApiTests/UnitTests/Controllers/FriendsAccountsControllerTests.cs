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
    public class FriendsAccountsControllerTests : IDisposable
    {
        private Mock<IFriendRepository> mockFriendRepository { get; set; }

        private Mock<IGameAccountRepository> mockGameAccountRepository { get; set; }

        private FriendsAccountsController controller { get; set; }

        private IEnumerable<GameAccount> friendsAccounts { get; set; }

        private IEnumerable<Friend> friends { get; set; }

        private int idDoesNotExist { get; set; } = 7777;

        public FriendsAccountsControllerTests()
        {
            mockFriendRepository = new Mock<IFriendRepository>();
            mockGameAccountRepository = new Mock<IGameAccountRepository>();

            controller = new FriendsAccountsController(mockFriendRepository.Object, mockGameAccountRepository.Object);

            friendsAccounts = new List<GameAccount>
            {
                new GameAccount { Id = 1, GamePlatformId = 1, UserName = "IronManXbox", UserProfileId = 1, CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) },
                new GameAccount { Id = 2, GamePlatformId = 2, UserName = "IronManPlaystation", UserProfileId = 1, CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) },
            };

            friends = new List<Friend>
            {
                new Friend { Id = 1, UserProfileId = 1, FriendUserProfile = new UserProfile { }, FriendUserProfileId = 2, CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) },
                new Friend { Id = 2, UserProfileId = 1, FriendUserProfile = new UserProfile { }, FriendUserProfileId = 3, CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) }
            };
        }

        /*[Fact]
        public void GetCollectionReturnsAll()
        {
            var userProfileId = friendsAccounts.First().UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(55)).Returns(new GameAccount { GamePlatformId = 1, UserProfileId = 2 });
            mockFriendRepository.Setup(repo => repo.GetAllByUserProfileId(userProfileId)).Returns(friends);

        }*/

        [Fact]
        public void GetCollectionGameAccountDoestNotExist()
        {
            var userProfileId = friendsAccounts.First().UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(55)).Returns((GameAccount)null);

            var result = controller.GetCollection(userProfileId, idDoesNotExist);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        public void Dispose()
        {
            controller.Dispose();
        }
    }
}
