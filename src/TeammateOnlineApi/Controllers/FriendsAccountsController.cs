﻿using System.Collections.Generic;
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
    [Authorize]
    [Route("api/UserProfiles/{userProfileId}/GameAccounts/{gameAccountId}/[controller]")]
    public class FriendsAccountsController : BaseController
    {
        private IGameAccountRepository gameAccountRepository;

        private IFriendRepository friendRepository;

        public FriendsAccountsController(IFriendRepository friendRepository, IGameAccountRepository gameAccountRepository)
        {
            this.gameAccountRepository = gameAccountRepository;
            this.friendRepository = friendRepository;
        }

        [HttpGet]
        public IActionResult GetCollection(int userProfileId, int gameAccountId)
        {
            var gameAccountList = new List<GameAccount>();

            var gameAccount = gameAccountRepository.FindById(gameAccountId);

            if (gameAccount == null || gameAccount.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            var friends = friendRepository.GetAllByUserProfileId(userProfileId);
            foreach(var friend in friends)
            {
                var friendsGameAccounts = gameAccountRepository.GetAllByUserProfileId(friend.FriendUserProfileId).Where(ga => ga.GamePlatformId == gameAccount.GamePlatformId);
                gameAccountList.AddRange(friendsGameAccounts);
            }

            return new OkObjectResult(gameAccountList);
        }
    }
}
