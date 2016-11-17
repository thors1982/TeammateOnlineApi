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
        private IUserProfileRepository userProfileRepository;
        private IGameAccountRepository gameAccountRepository;

        public SearchController(IUserProfileRepository userProfileRepository, IGameAccountRepository gameAccountRepository)
        {
            this.userProfileRepository = userProfileRepository;
            this.gameAccountRepository = gameAccountRepository;
        }

        [HttpGet]
        public IActionResult GetCollection([FromQuery]string query = null)
        {
            var searchResponse = new Search();
            searchResponse.UserProfiles = new List<UserProfile>();
            searchResponse.GameAccounts = new List<GameAccount>();

            if (!string.IsNullOrEmpty(query))
            {
                // Find user profiles
                var userProfileController = new UserProfilesController(userProfileRepository);
                var foundUserProfiles = (List<UserProfile>)userProfileController.GetCollection(query: query);
                searchResponse.UserProfiles.AddRange(foundUserProfiles);

                // Find gameaccounts
                searchResponse.GameAccounts.AddRange(gameAccountRepository.Query(query));
            }

            return new OkObjectResult(searchResponse);
        }
    }
}
