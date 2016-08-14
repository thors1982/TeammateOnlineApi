using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IFriendRepository
    {
        Friend Add(Friend friend);

        Friend FindById(int id);

        IEnumerable<Friend> GetAllByUserProfileId(int userProfileId);

        Friend FindFriendOfAUser(int userProfileId, int friendUserProfileId);

        void Remove(Friend friend);
    }
}
