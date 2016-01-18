using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IFriendRepository
    {
        Friend Add(Friend friend);
        Friend FinBdyId(int id);
        IEnumerable<Friend> GetAll();
        void Remove(Friend friend);
    }
}
