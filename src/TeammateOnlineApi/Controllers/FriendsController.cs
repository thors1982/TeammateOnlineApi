using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Annotations;
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
        public IFriendRepository FriendRepository;

        public FriendsController(IFriendRepository friendRepository)
        {
            FriendRepository = friendRepository;
        }

        [HttpGet]
        public IEnumerable<Friend> GetCollection(int userProfileId)
        {
            return FriendRepository.GetAllByUserProfileId(userProfileId);
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(Friend))]
        public IActionResult Post(int userProfileId, [FromBody]Friend newFriend)
        {
            // Make sure friend doesn't already exist
            if (FriendRepository.FindFriendOfAUser(userProfileId, newFriend.FriendUserProfileId) != null)
            {
                ModelState.AddModelError("FriendUserProfileId", "Friend already exists.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = FriendRepository.Add(newFriend);

            return CreatedAtRoute("FriendDetail", new { controller = "FriendsController", friendId = result.Id }, result);
        }

        [HttpGet("{friendId}", Name = "FriendDetail")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, "User profile", typeof(Friend))]

        public IActionResult GetDetail(int userProfileId, int friendId)
        {
            var friend = FriendRepository.FindById(friendId);

            if (friend == null || friend.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            return new OkObjectResult(friend);
        }

        [HttpDelete("{friendId}")]
        public IActionResult Delete(int userProfileId, int friendId)
        {
            var friend = FriendRepository.FindById(friendId);

            if (friend == null || friend.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            FriendRepository.Remove(friend);

            return new NoContentResult();
        }
    }
}
