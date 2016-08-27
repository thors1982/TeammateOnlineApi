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
    public class FriendsController : BaseController
    {
        private IFriendRepository friendRepository;

        public FriendsController(IFriendRepository friendRepository)
        {
            this.friendRepository = friendRepository;
        }

        [HttpGet]
        public IEnumerable<Friend> GetCollection(int userProfileId)
        {
            return friendRepository.GetAllByUserProfileId(userProfileId);
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(Friend))]
        public IActionResult Post(int userProfileId, [FromBody]Friend newFriend)
        {
            // Make sure friend doesn't already exist
            if (friendRepository.FindFriendOfAUser(userProfileId, newFriend.FriendUserProfileId) != null)
            {
                ModelState.AddModelError("FriendUserProfileId", "Friend already exists.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = friendRepository.Add(newFriend);

            return CreatedAtRoute("FriendDetail", new { controller = "FriendsController", friendId = result.Id }, result);
        }

        [HttpGet("{friendId}", Name = "FriendDetail")]

        public IActionResult GetDetail(int userProfileId, int friendId)
        {
            var friend = friendRepository.FindById(friendId);

            if (friend == null || friend.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            return new OkObjectResult(friend);
        }

        [HttpDelete("{friendId}")]
        public IActionResult Delete(int userProfileId, int friendId)
        {
            var friend = friendRepository.FindById(friendId);

            if (friend == null || friend.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            friendRepository.Remove(friend);

            return new NoContentResult();
        }
    }
}
