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
    public class FriendRequestsController : BaseController
    {
        public IFriendRequestRepository Repository;
        public IFriendRepository FriendRepository;

        public FriendRequestsController(IFriendRequestRepository friendRequestRepository, IFriendRepository friendRepository)
        {
            Repository = friendRequestRepository;
            FriendRepository = friendRepository;
        }

        [HttpGet]
        public IEnumerable<FriendRequest> GetCollection(int userProfileId)
        {
            var requests = Repository.GetAllIncomingAndOutgoingRequests(userProfileId).Where(r => r.IsPending == true && r.IsAccepted == false);
            foreach(var r in requests)
            {
                if(r.FriendUserProfileId == userProfileId)
                {
                    r.IsIncomingRequest = true;
                }
            }
            return requests;
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(FriendRequest))]
        public IActionResult Post(int userProfileId, [FromBody]FriendRequest newFriendRequest)
        {
            // Make sure user's are not already friends
            if(FriendRepository.FindFriendOfAUser(userProfileId, newFriendRequest.FriendUserProfileId) != null)
            {
                ModelState.AddModelError("FriendUserProfileId", "Friend already exists.");
                return new BadRequestObjectResult(ModelState);
            }
            // Make sure friend request does not already exist
            if(Repository.FindFriendRequestOfAUser(userProfileId, newFriendRequest.FriendUserProfileId) != null)
            {
                ModelState.AddModelError("FriendUserProfileId", "Friend request already exists.");
                return new BadRequestObjectResult(ModelState);
            }

            newFriendRequest.IsPending = true;
            newFriendRequest.IsAccepted = false;

            var result = Repository.Add(newFriendRequest);

            return CreatedAtRoute("FriendRequestDetail", new { controller = "FriendRequestsController", friendRequestId = result.Id }, result);
        }

        [HttpGet("{friendRequestId}", Name = "FriendRequestDetail")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, "User profile", typeof(FriendRequest))]

        public IActionResult GetDetail(int userProfileId, int friendRequestId)
        {
            var friendRequest = Repository.FindById(friendRequestId);

            if (friendRequest == null || friendRequest.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            return new OkObjectResult(friendRequest);
        }

        [HttpPut("{friendRequestId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, int friendRequestId, [FromBody]FriendRequest newFriendRequest)
        {
            // Todo make sure someone is not accepting someone elses request

            var friendRequest = Repository.FindById(friendRequestId);

            if (friendRequest == null || friendRequest.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            friendRequest.IsPending = newFriendRequest.IsPending;
            friendRequest.IsAccepted = newFriendRequest.IsAccepted;

            if(friendRequest.IsAccepted == true)
            {
                var result = CreateFriends(userProfileId, newFriendRequest.FriendUserProfileId);
                if(result == false)
                {
                    friendRequest.IsAccepted = false;
                }
            }

            Repository.Update(friendRequest);

            return new OkResult();
        }

        [HttpDelete("{friendRequestId}")]
        public IActionResult Delete(int userProfileId, int friendRequestId)
        {
            var friendRequest = Repository.FindById(friendRequestId);

            if (friendRequest == null || friendRequest.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            Repository.Remove(friendRequest);

            return new NoContentResult();
        }

        private bool CreateFriends(int userId1, int userId2)
        {
            var friendController = new FriendsController(FriendRepository);

            // Create friend for first user
            friendController.Post(userId1, new Friend { UserProfileId = userId1, FriendUserProfileId = userId2 });
            // Create friend for second user
            friendController.Post(userId2, new Friend { UserProfileId = userId2, FriendUserProfileId = userId1 });

            return true;
        }
    }
}
