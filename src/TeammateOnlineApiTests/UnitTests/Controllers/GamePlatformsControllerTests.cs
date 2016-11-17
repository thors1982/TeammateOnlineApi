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
    public class GamePlatformsControllerTests : IDisposable
    {
        private Mock<IGamePlatformRepository> mockGamePlatformRepository { get; set; }

        private GamePlatformsController controller { get; set; }

        private IEnumerable<GamePlatform> gamePlatforms { get; set; }

        private int idDoesNotExist { get; set; } = 7777;

        public GamePlatformsControllerTests()
        {
            mockGamePlatformRepository = new Mock<IGamePlatformRepository>();

            controller = new GamePlatformsController(mockGamePlatformRepository.Object);

            gamePlatforms = new List<GamePlatform>
                {
                    new GamePlatform() { Id = 1, Name = "Xbox Live", Url = "http://www.xbox.com/en-US/live", CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) },
                    new GamePlatform() { Id = 3, Name = "PlayStation Network", Url = "https://www.playstationnetwork.com/home", CreatedDate = new DateTime(2015, 12, 1), ModifiedDate = new DateTime(2016, 1, 1) },
                };
        }

        [Fact]
        public void GetCollectionReturnsAll()
        {
            mockGamePlatformRepository.Setup(repo => repo.GetAll()).Returns(gamePlatforms);

            var result = controller.GetCollection();
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(gamePlatforms, okResult.Value);
        }

        [Fact]
        public void PostReturnsCreateAtResult()
        {
            var testGamePlatform = gamePlatforms.First();

            mockGamePlatformRepository.Setup(repo => repo.Add(It.IsAny<GamePlatform>())).Returns(testGamePlatform);

            var result = controller.Post(testGamePlatform);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(testGamePlatform, createdResult.Value);

            Assert.Equal("GamePlatformDetail", createdResult.RouteName);
        }

        [Fact]
        public void GetDteailReturnsOkObjectResult()
        {
            var testGamePlatform = gamePlatforms.First();

            mockGamePlatformRepository.Setup(repo => repo.FindById(testGamePlatform.Id)).Returns(testGamePlatform);

            var result = controller.GetDetail(testGamePlatform.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testGamePlatform, okResult.Value);
        }

        [Fact]
        public void GetDetailReturnsNotFound()
        {
            mockGamePlatformRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns((GamePlatform)null);

            var result = controller.GetDetail(idDoesNotExist);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PutReturnsOkResult()
        {
            var testGamePlatform = gamePlatforms.First();

            mockGamePlatformRepository.Setup(repo => repo.FindById(testGamePlatform.Id)).Returns(testGamePlatform);
            mockGamePlatformRepository.Setup(repo => repo.Update(testGamePlatform));

            var updatedGamePlatform = new GamePlatform
            {
                Id = testGamePlatform.Id,
                Name = "Test Name",
                Url = "http://test.url/"
            };
            
            var result = controller.Put(testGamePlatform.Id, updatedGamePlatform);

            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void PutReturnsNotFound()
        {
            var testGamePlatform = gamePlatforms.First();
            
            mockGamePlatformRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns((GamePlatform)null);

            var updatedGamePlatform = new GamePlatform
            {
                Id = idDoesNotExist,
                Name = "Test Name",
                Url = "http://test.url/"
            };
            
            var result = controller.Put(idDoesNotExist, updatedGamePlatform);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteReturnsNoContentResult()
        {
            var testGamePlatform = gamePlatforms.First();

            mockGamePlatformRepository.Setup(repo => repo.FindById(testGamePlatform.Id)).Returns(testGamePlatform);
            mockGamePlatformRepository.Setup(repo => repo.Remove(testGamePlatform));

            var result = controller.Delete(testGamePlatform.Id);

            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteReturnsNotFound()
        {
            var testGamePlatform = gamePlatforms.First();

            mockGamePlatformRepository.Setup(repo => repo.FindById(idDoesNotExist)).Returns((GamePlatform)null);
            
            var result = controller.Delete(idDoesNotExist);

            var notFound = Assert.IsType<NotFoundResult>(result);
        }

        public void Dispose()
        {
        }
    }
}
