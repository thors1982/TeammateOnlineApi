using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private TeammateOnlineContext context;

        public FriendRequestRepository(TeammateOnlineContext newContext)
        {
            context = newContext;
        }

        public FriendRequest Add(FriendRequest friendRequest)
        {
            context.FriendRequests.Add(friendRequest);
            context.SaveChanges();

            return friendRequest;
        }

        public FriendRequest FindById(int id)
        {
            return context.FriendRequests.Include(u => u.FriendUserProfile).Include(u => u.UserProfile).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<FriendRequest> GetAllByUserProfileId(int userProfileId)
        {
            return context.FriendRequests.Include(u => u.FriendUserProfile).Include(u => u.UserProfile).Where(x => x.UserProfileId == userProfileId);
        }

        public IEnumerable<FriendRequest> GetAllIncomingAndOutgoingRequests(int userProfileId)
        {
            return context.FriendRequests.Include(u => u.FriendUserProfile).Include(u => u.UserProfile).Where(x => x.UserProfileId == userProfileId || x.FriendUserProfileId == userProfileId);
        }

        public FriendRequest FindFriendRequestOfAUser(int userProfileId, int friendUserProfileId)
        {
            return context.FriendRequests.Include(u => u.FriendUserProfile).Include(u => u.UserProfile).FirstOrDefault(x => (x.UserProfileId == userProfileId && x.FriendUserProfileId == friendUserProfileId) || (x.UserProfileId == friendUserProfileId && x.FriendUserProfileId == userProfileId));
        }

        public void Remove(FriendRequest friendRequest)
        {
            context.Remove(friendRequest);
            context.SaveChanges();
        }

        public void Update(FriendRequest friendRequest)
        {
            context.FriendRequests.Update(friendRequest);
            context.SaveChanges();
        }
    }
}
