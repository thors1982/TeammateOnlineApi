using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Route("api/userprofiles/{userProfileId}/[controller]")]
    public class GameAccountsController : BaseController
    {
        [HttpGet]
        public IEnumerable<GameAccount> GetCollection(int userProfileId, [FromQuery]int? gamePlatformId)
        {
            var gameAccountList = TeammateOnlineContext.GameAccounts.Where(x => x.UserProfileId == userProfileId);
            if(gamePlatformId != null)
            {
                gameAccountList.Where(x => x.GamePlatformId == gamePlatformId);
            }

            return gameAccountList.ToList();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post(int userProfileId, [FromBody]GameAccount newGameAccount)
        {
            var result = TeammateOnlineContext.GameAccounts.Add(newGameAccount);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "GameAccountsController", gameAccountId = result.Entity.Id }, result.Entity);
        }

        [HttpGet("{gameAccountId}")]
        public IActionResult GetDetail(int userProfileId, int gameAccountId)
        {
            var gameAccount = TeammateOnlineContext.GameAccounts.FirstOrDefault(x => x.Id == gameAccountId && x.UserProfileId == userProfileId);

            if(gameAccount == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(gameAccount);
        }

        [HttpPut("{gameAccountId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, int gameAccountId, [FromBody]GameAccount newGameAccount)
        {
            var gameAccount = TeammateOnlineContext.GameAccounts.FirstOrDefault(x => x.Id == gameAccountId && x.UserProfileId == userProfileId);

            if (gameAccount == null)
            {
                return HttpNotFound();
            }

            gameAccount.UserProfileId = userProfileId;
            gameAccount.GamePlatformId = newGameAccount.GamePlatformId;
            gameAccount.UserName = newGameAccount.UserName;
            TeammateOnlineContext.GameAccounts.Update(gameAccount);
            TeammateOnlineContext.SaveChanges();

            return new HttpOkResult();
        }

        [HttpDelete("{gameAccountId}")]
        public IActionResult Delete(int userProfileId, int gameAccountId)
        {
            var gameAccount = TeammateOnlineContext.GameAccounts.FirstOrDefault(x => x.Id == gameAccountId && x.UserProfileId == userProfileId);

            if (gameAccount == null)
            {
                return HttpNotFound();
            }

            var result = TeammateOnlineContext.GameAccounts.Remove(gameAccount);
            TeammateOnlineContext.SaveChanges();

            return new NoContentResult();
        }
    }
}