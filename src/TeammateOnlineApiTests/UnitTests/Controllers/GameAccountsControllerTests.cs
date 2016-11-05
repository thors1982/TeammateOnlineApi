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
    public class GameAccountsControllerTests : IDisposable
    {
        private Mock<IGameAccountRepository> mockGameAccountRepository { get; set; }

        private GameAccountsController controller { get; set; }

        private IEnumerable<GameAccount> gameAccounts { get; set; }

        private int idDoesNotExist { get; set; } = 7777;

        public GameAccountsControllerTests()
        {
            mockGameAccountRepository = new Mock<IGameAccountRepository>();

            controller = new GameAccountsController(mockGameAccountRepository.Object);

            gameAccounts = new List<GameAccount>
            {
                new GameAccount { Id = 1, GamePlatformId = 1, UserName = "IronManXbox", UserProfileId = 1, CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) },
                new GameAccount { Id = 2, GamePlatformId = 2, UserName = "IronManPlaystation", UserProfileId = 1, CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) },
            };
        }

        [Fact]
        public void GetCollection_ReturnsAll()
        {
            var userProfileId = gameAccounts.First().UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.GetAllByUserProfileId(userProfileId)).Returns(gameAccounts);

            var result = controller.GetCollection(userProfileId, null);

            Assert.Equal(gameAccounts, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetCollection_ByGamePlatformId(int gamePlatformId)
        {
            var userProfileId = gameAccounts.First().UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.GetAllByUserProfileId(userProfileId)).Returns(gameAccounts);

            var result = controller.GetCollection(userProfileId, gamePlatformId);

            Assert.Equal(gameAccounts.Where(g => g.GamePlatformId == gamePlatformId), result);
        }

        [Fact]
        public void Post_ReturnsCreateAtResult()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;
            
            mockGameAccountRepository.Setup(repo => repo.Add(It.IsAny<GameAccount>())).Returns(testGameAccount);

            var result = controller.Post(userProfileId, testGameAccount);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(testGameAccount, createdResult.Value);

            Assert.Equal("GameAccountDetail", createdResult.RouteName);
        }

        [Fact]
        public void GetDteail_ReturnsOkObjectResult()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(testGameAccount.Id)).Returns(testGameAccount);

            var result = controller.GetDetail(userProfileId, testGameAccount.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testGameAccount, okResult.Value);
        }

        [Fact]
        public void GetDetail_GameAccountsIdDoesNotExist_ReturnsNotFound()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns((GameAccount)null);

            var result = controller.GetDetail(userProfileId, idDoesNotExist);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetDetail_UserIdDoesNotExist_ReturnsNotFound()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(testGameAccount.Id)).Returns((GameAccount)null);

            var result = controller.GetDetail(idDoesNotExist, testGameAccount.Id);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Put_ReturnsOkResult()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(testGameAccount.Id)).Returns(testGameAccount);
            mockGameAccountRepository.Setup(repo => repo.Update(testGameAccount));

            var updatedGameAccount = new GameAccount
            {
                Id = testGameAccount.Id,
                GamePlatformId = testGameAccount.GamePlatformId,
                UserProfileId = testGameAccount.UserProfileId,
                UserName = "NewUserName",
            };

            var result = controller.Put(userProfileId, updatedGameAccount.GamePlatformId, updatedGameAccount);

            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Put_GameAccountsIdDoesNotExist_ReturnsNotFound()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(testGameAccount.Id)).Returns((GameAccount)null);

            var updatedGameAccount = new GameAccount
            {
                Id = testGameAccount.Id,
                GamePlatformId = testGameAccount.GamePlatformId,
                UserProfileId = testGameAccount.UserProfileId,
                UserName = "NewUserName",
            };

            var result = controller.Put(userProfileId, updatedGameAccount.GamePlatformId, updatedGameAccount);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Put_UserIdDoesNotExist_ReturnsNotFound()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            var wrongGameAccount = new GameAccount
            {
                Id = testGameAccount.Id,
                GamePlatformId = testGameAccount.GamePlatformId,
                UserProfileId = idDoesNotExist,
                UserName = testGameAccount.UserName
            };

            mockGameAccountRepository.Setup(repo => repo.FindById(testGameAccount.Id)).Returns(wrongGameAccount);

            var updatedGameAccount = new GameAccount
            {
                Id = testGameAccount.Id,
                GamePlatformId = testGameAccount.GamePlatformId,
                UserProfileId = testGameAccount.UserProfileId,
                UserName = "NewUserName",
            };

            var result = controller.Put(userProfileId, updatedGameAccount.GamePlatformId, updatedGameAccount);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNoContentResult()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(testGameAccount.Id)).Returns(testGameAccount);
            mockGameAccountRepository.Setup(repo => repo.Remove(testGameAccount));

            var result = controller.Delete(userProfileId, testGameAccount.Id);
            
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_GameAccountsIdDoesNotExist_ReturnsNotFound()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            mockGameAccountRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns((GameAccount)null);

            var result = controller.Delete(userProfileId, idDoesNotExist);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_UserIdDoesNotExist_ReturnsNotFound()
        {
            var testGameAccount = gameAccounts.First();
            var userProfileId = testGameAccount.UserProfileId;

            var wrongGameAccount = new GameAccount
            {
                Id = testGameAccount.Id,
                GamePlatformId = testGameAccount.GamePlatformId,
                UserProfileId = idDoesNotExist,
                UserName = testGameAccount.UserName
            };

            mockGameAccountRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns(wrongGameAccount);

            var result = controller.Delete(userProfileId, testGameAccount.Id);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        public void Dispose()
        {
            controller.Dispose();
        }
    }
}
