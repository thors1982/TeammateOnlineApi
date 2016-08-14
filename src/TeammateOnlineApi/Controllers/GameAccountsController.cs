using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Annotations;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Authorize]
    [Route("api/UserProfiles/{userProfileId}/[controller]")]
    public class GameAccountsController : BaseController
    {
        public IGameAccountRepository GameAccountRepository;

        public GameAccountsController(IGameAccountRepository gameAccountRepository)
        {
            GameAccountRepository = gameAccountRepository;
        }

        [HttpGet]
        public IEnumerable<GameAccount> GetCollection(int userProfileId, [FromQuery]int? gamePlatformId)
        {
            var gameAccountList = GameAccountRepository.GetAllByUserProfileId(userProfileId);

            if (gamePlatformId != null)
            {
                gameAccountList.Where(x => x.GamePlatformId == gamePlatformId);
            }

            return gameAccountList;
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(GameAccount))]
        public IActionResult Post(int userProfileId, [FromBody]GameAccount newGameAccount)
        {
            var result = GameAccountRepository.Add(newGameAccount);

            return CreatedAtRoute("GameAccountDetail", new { controller = "GameAccountsController", gameAccountId = result.Id }, result);
        }

        [HttpGet("{gameAccountId}", Name = "GameAccountDetail")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, "Game account", typeof(GameAccount))]
        public IActionResult GetDetail(int userProfileId, int gameAccountId)
        {
            var gameAccount = GameAccountRepository.FindById(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            return new OkObjectResult(gameAccount);
        }

        [HttpPut("{gameAccountId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, int gameAccountId, [FromBody]GameAccount newGameAccount)
        {
            var gameAccount = GameAccountRepository.FindById(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            gameAccount.UserProfileId = userProfileId;
            gameAccount.GamePlatformId = newGameAccount.GamePlatformId;
            gameAccount.UserName = newGameAccount.UserName;

            GameAccountRepository.Update(gameAccount);

            return new OkResult();
        }

        [HttpDelete("{gameAccountId}")]
        public IActionResult Delete(int userProfileId, int gameAccountId)
        {
            var gameAccount = GameAccountRepository.FindById(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            GameAccountRepository.Remove(gameAccount);

            return new NoContentResult();
        }
    }
}
