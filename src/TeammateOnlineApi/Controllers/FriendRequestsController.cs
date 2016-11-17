using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Authorize]
    [Route("api/UserProfiles/{userProfileId}/[controller]")]
    public class FriendRequestsController : BaseController
    {
        private IFriendRequestRepository friendRequestRepository;
        private IFriendRepository friendRepository;

        public FriendRequestsController(IFriendRequestRepository friendRequestRepository, IFriendRepository friendRepository)
        {
            this.friendRequestRepository = friendRequestRepository;
            this.friendRepository = friendRepository;
        }

        [HttpGet]
        public IActionResult GetCollection(int userProfileId)
        {
            var friendRequestsFromDatabase = friendRequestRepository.GetAllIncomingAndOutgoingRequests(userProfileId).Where(r => r.IsPending == true && r.IsAccepted == false);
            var responseFriendRequests = new List<FriendRequest>();
            foreach (var request in friendRequestsFromDatabase)
            {
                responseFriendRequests.Add(TranslateFriendRequestForResponse(userProfileId, request));
            }

            return new OkObjectResult(responseFriendRequests);
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(FriendRequest))]
        public IActionResult Post(int userProfileId, [FromBody]FriendRequest newFriendRequest)
        {
            // Make sure user's are not already friends
            if (friendRepository.FindFriendOfAUser(userProfileId, newFriendRequest.FriendUserProfileId) != null)
            {
                ModelState.AddModelError("FriendUserProfileId", "Friend already exists.");
                return new BadRequestObjectResult(ModelState);
            }

            // Make sure friend request does not already exist
            if (friendRequestRepository.FindFriendRequestOfAUser(userProfileId, newFriendRequest.FriendUserProfileId) != null)
            {
                ModelState.AddModelError("FriendUserProfileId", "Friend request already exists.");
                return new BadRequestObjectResult(ModelState);
            }

            newFriendRequest.IsPending = true;
            newFriendRequest.IsAccepted = false;

            var result = friendRequestRepository.Add(newFriendRequest);

            return CreatedAtRoute("FriendRequestDetail", new { controller = "FriendRequestsController", friendRequestId = result.Id }, result);
        }

        [HttpGet("{friendRequestId}", Name = "FriendRequestDetail")]

        public IActionResult GetDetail(int userProfileId, int friendRequestId)
        {
            var friendRequest = friendRequestRepository.FindById(friendRequestId);

            if (friendRequest == null || (friendRequest.UserProfileId != userProfileId && friendRequest.FriendUserProfileId != userProfileId))
            {
                return NotFound();
            }

            return new OkObjectResult(TranslateFriendRequestForResponse(userProfileId, friendRequest));
        }

        [HttpPut("{friendRequestId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, int friendRequestId, [FromBody]FriendRequest newFriendRequest)
        {
            // Todo: make sure someone is not accepting someone elses request

            var friendRequest = friendRequestRepository.FindById(friendRequestId);

            if (friendRequest == null || (friendRequest.UserProfileId != userProfileId && friendRequest.FriendUserProfileId != userProfileId))
            {
                return NotFound();
            }

            friendRequest.Note = newFriendRequest.Note;
            friendRequest.IsPending = newFriendRequest.IsPending;
            friendRequest.IsAccepted = newFriendRequest.IsAccepted;

            if (friendRequest.IsAccepted == true)
            {
                var result = CreateFriends(userProfileId, newFriendRequest.FriendUserProfileId);
                if (result == false)
                {
                    friendRequest.IsAccepted = false;
                }
            }

            friendRequestRepository.Update(friendRequest);

            return new OkResult();
        }

        [HttpDelete("{friendRequestId}")]
        public IActionResult Delete(int userProfileId, int friendRequestId)
        {
            var friendRequest = friendRequestRepository.FindById(friendRequestId);

            if (friendRequest == null || friendRequest.UserProfileId != userProfileId)
            {
                return NotFound();
            }

            friendRequestRepository.Remove(friendRequest);

            return new NoContentResult();
        }

        private bool CreateFriends(int userId1, int userId2)
        {
            var friendController = new FriendsController(friendRepository);

            // Create friend for first user
            friendController.Post(userId1, new Friend { UserProfileId = userId1, FriendUserProfileId = userId2 });

            // Create friend for second user
            friendController.Post(userId2, new Friend { UserProfileId = userId2, FriendUserProfileId = userId1 });

            return true;
        }

        private FriendRequest TranslateFriendRequestForResponse(int userProfileId, FriendRequest friendRequest)
        {
            if (friendRequest.UserProfileId == userProfileId)
            {
                friendRequest.IsIncomingRequest = false;
                return friendRequest;
            }

            return new FriendRequest
            {
                Id = friendRequest.Id,
                UserProfileId = friendRequest.FriendUserProfileId,
                UserProfile = friendRequest.FriendUserProfile,
                FriendUserProfileId = friendRequest.UserProfileId,
                FriendUserProfile = friendRequest.UserProfile,
                Note = friendRequest.Note,
                IsAccepted = friendRequest.IsAccepted,
                IsPending = friendRequest.IsPending,
                IsIncomingRequest = true,
                CreatedDate = friendRequest.CreatedDate,
                ModifiedDate = friendRequest.ModifiedDate
            };
        }
    }
}
