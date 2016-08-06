using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IFriendRepository
    {
        Friend Add(Friend friend);
        Friend FindById(int id);
        IEnumerable<Friend> GetAllByUserProfileId(int userProfileId);
        void Remove(Friend friend);
    }
}
