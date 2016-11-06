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
        }

        public void Dispose()
        {
            controller.Dispose();
        }
    }
}
