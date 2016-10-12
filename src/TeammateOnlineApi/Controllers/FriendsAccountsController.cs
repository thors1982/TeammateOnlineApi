using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Annotations;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;
using Microsoft.AspNetCore.Http;

namespace TeammateOnlineApi.Controllers
{
    [Route("api/UserProfiles/{userProfileId}/GameAccounts/{gameAccountId}/[controller]")]
    public class FriendsAccountsController : BaseController
    {
        private IGameAccountRepository gameAccountRepository;

        private IFriendRepository friendRepository;

        public FriendsAccountsController(IGameAccountRepository gameAccountRepository, IFriendRepository friendRepository)
        {
            this.gameAccountRepository = gameAccountRepository;
            this.friendRepository = friendRepository;
        }

        [HttpGet]
        public IEnumerable<GameAccount> GetCollection(int userProfileId, int gameAccountId)
        {
            var gameAccountList = new List<GameAccount>();

            var gameAccount = gameAccountRepository.FindById(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return null;
            }

            var friends = friendRepository.GetAllByUserProfileId(userProfileId);
            foreach(var friend in friends)
            {
                var friendsGameAccounts = gameAccountRepository.GetAllByUserProfileId(friend.FriendUserProfileId).Where(ga => ga.GamePlatformId == gameAccount.GamePlatformId);
                gameAccountList.AddRange(friendsGameAccounts);
            }

            return gameAccountList;
        }
    }
}
