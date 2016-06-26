using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Authorize]
    [Route("api/UserProfiles/{userProfileId}/[controller]")]
    public class FriendsController : BaseController
    {
        public IFriendRepository Repository;

        public FriendsController(IFriendRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public IEnumerable<Friend> GetCollection(int userProfileId)
        {
            return Repository.GetAllByUserProfileId(userProfileId);
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post(int userProfileId, [FromBody]Friend newFriend)
        {
            var result = Repository.Add(newFriend);

            return CreatedAtRoute("FriendDetail", new { controller = "FriendsController", friendId = result.Id }, result);
        }

        [HttpGet("{friendId}", Name = "FriendDetail")]
        public IActionResult GetDetail(int userProfileId, int friendId)
        {
            var friend = Repository.FinBdyId(friendId);

            if (friend == null || friend.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            return new OkObjectResult(friend);
        }

        [HttpDelete("{friendId}")]
        public IActionResult Delete(int userProfileId, int friendId)
        {
            var friend = Repository.FinBdyId(friendId);

            if (friend == null || friend.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            Repository.Remove(friend);

            return new NoContentResult();
        }
    }
}
