using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;
using TeammateOnlineApi.ViewModels;

namespace TeammateOnlineApi.Controllers
{
    [Route("api/userprofiles/{userProfileId}/[controller]")]
    public class GameAccountsController : BaseController
    {
        [HttpGet]
        public IEnumerable<GameAccountViewModel> GetCollection(int userProfileId, [FromQuery]int? gamePlatformId)
        {
            var gameAccountList = TeammateOnlineContext.GameAccounts.Where(x => x.UserProfileId == userProfileId);
            if(gamePlatformId != null)
            {
                gameAccountList.Where(x => x.GamePlatformId == gamePlatformId);
            }

            return Mapper.Map<IEnumerable<GameAccountViewModel>>(gameAccountList.ToList());
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post(int userProfileId, [FromBody]GameAccountViewModel request)
        {
            var newGameAccount = Mapper.Map<GameAccount>(request);

            TeammateOnlineContext.GameAccounts.Add(newGameAccount);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "GameAccountsController", gameAccountId = newGameAccount.Id }, Mapper.Map<GameAccountViewModel>(newGameAccount));
        }

        [HttpGet("{gameAccountId}")]
        public IActionResult GetDetail(int userProfileId, int gameAccountId)
        {
            var gameAccount = TeammateOnlineContext.GameAccounts.FirstOrDefault(x => x.Id == gameAccountId && x.UserProfileId == userProfileId);

            if(gameAccount == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(Mapper.Map<GameAccountViewModel>(gameAccount));
        }

        [HttpPut("{gameAccountId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, int gameAccountId, [FromBody]GameAccountViewModel request)
        {
            var gameAccount = TeammateOnlineContext.GameAccounts.FirstOrDefault(x => x.Id == gameAccountId && x.UserProfileId == userProfileId);

            if (gameAccount == null)
            {
                return HttpNotFound();
            }

            var newGameAccount = Mapper.Map<GameAccount>(request);
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