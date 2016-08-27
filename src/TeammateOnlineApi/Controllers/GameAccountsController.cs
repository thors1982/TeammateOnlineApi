using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Annotations;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Authorize]
    [Route("api/UserProfiles/{userProfileId}/[controller]")]
    public class GameAccountsController : BaseController
    {
        private IGameAccountRepository gameAccountRepository;

        public GameAccountsController(IGameAccountRepository gameAccountRepository)
        {
            this.gameAccountRepository = gameAccountRepository;
        }

        [HttpGet]
        public IEnumerable<GameAccount> GetCollection(int userProfileId, [FromQuery]int? gamePlatformId)
        {
            var gameAccountList = gameAccountRepository.GetAllByUserProfileId(userProfileId);

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
            var result = gameAccountRepository.Add(newGameAccount);

            return CreatedAtRoute("GameAccountDetail", new { controller = "GameAccountsController", gameAccountId = result.Id }, result);
        }

        [HttpGet("{gameAccountId}", Name = "GameAccountDetail")]
        public IActionResult GetDetail(int userProfileId, int gameAccountId)
        {
            var gameAccount = gameAccountRepository.FindById(gameAccountId);

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
            var gameAccount = gameAccountRepository.FindById(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            gameAccount.UserProfileId = userProfileId;
            gameAccount.GamePlatformId = newGameAccount.GamePlatformId;
            gameAccount.UserName = newGameAccount.UserName;

            gameAccountRepository.Update(gameAccount);

            return new OkResult();
        }

        [HttpDelete("{gameAccountId}")]
        public IActionResult Delete(int userProfileId, int gameAccountId)
        {
            var gameAccount = gameAccountRepository.FindById(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            gameAccountRepository.Remove(gameAccount);

            return new NoContentResult();
        }
    }
}
