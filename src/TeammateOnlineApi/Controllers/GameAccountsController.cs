using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Authorize]
    [Route("api/userprofiles/{userProfileId}/[controller]")]
    public class GameAccountsController : BaseController
    {
        public IGameAccountRepository Repository;

        public GameAccountsController(IGameAccountRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public IEnumerable<GameAccount> GetCollection(int userProfileId, [FromQuery]int? gamePlatformId)
        {
            var gameAccountList = Repository.GetAllByUserProfileId(userProfileId);

            if(gamePlatformId != null)
            {
                gameAccountList.Where(x => x.GamePlatformId == gamePlatformId);
            }

            return gameAccountList;
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post(int userProfileId, [FromBody]GameAccount newGameAccount)
        {
            var result = Repository.Add(newGameAccount);

            return CreatedAtRoute("GetDetail", new { controller = "GameAccountsController", gameAccountId = result.Id }, result);
        }

        [HttpGet("{gameAccountId}")]
        public IActionResult GetDetail(int userProfileId, int gameAccountId)
        {
            var gameAccount = Repository.FinBdyId(gameAccountId);

            if(gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(gameAccount);
        }

        [HttpPut("{gameAccountId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, int gameAccountId, [FromBody]GameAccount newGameAccount)
        {
            var gameAccount = Repository.FinBdyId(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return HttpNotFound();
            }

            gameAccount.UserProfileId = userProfileId;
            gameAccount.GamePlatformId = newGameAccount.GamePlatformId;
            gameAccount.UserName = newGameAccount.UserName;

            Repository.Update(gameAccount);

            return new HttpOkResult();
        }

        [HttpDelete("{gameAccountId}")]
        public IActionResult Delete(int userProfileId, int gameAccountId)
        {
            var gameAccount = Repository.FinBdyId(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return HttpNotFound();
            }

            Repository.Remove(gameAccount);

            return new NoContentResult();
        }
    }
}
