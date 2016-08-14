using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IFriendRequestRepository
    {
        FriendRequest Add(FriendRequest friendRequest);

        FriendRequest FindById(int id);

        IEnumerable<FriendRequest> GetAllByUserProfileId(int userProfileId);

        IEnumerable<FriendRequest> GetAllIncomingAndOutgoingRequests(int userProfileId);

        FriendRequest FindFriendRequestOfAUser(int userProfileId, int friendUserProfileId);

        void Remove(FriendRequest friendRequest);

        void Update(FriendRequest friendRequest);
    }
}
