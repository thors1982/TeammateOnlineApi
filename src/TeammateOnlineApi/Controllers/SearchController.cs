using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Authorize]
    public class SearchController : BaseController
    {
        public IUserProfileRepository UserProfileRepository;
        public IGameAccountRepository GameAccountRepository;

        public SearchController(IUserProfileRepository userProfileRepository, IGameAccountRepository gameAccountRepository)
        {
            UserProfileRepository = userProfileRepository;
            GameAccountRepository = gameAccountRepository;
        }

        [HttpGet]
        public Search GetCollection([FromQuery]string query = null)
        {
            var searchResponse = new Search();
            searchResponse.UserProfiles = new List<UserProfile>();
            searchResponse.GameAccounts = new List<GameAccount>();

            if (!string.IsNullOrEmpty(query))
            {
                // Find user profiles
                var userProfileController = new UserProfilesController(UserProfileRepository);
                searchResponse.UserProfiles.AddRange(userProfileController.GetCollection(query: query));

                // Find gameaccounts
                searchResponse.GameAccounts.AddRange(GameAccountRepository.Query(query));
            }

            return searchResponse;
        }
    }
}
