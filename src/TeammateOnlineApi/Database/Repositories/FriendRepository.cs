using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private TeammateOnlineContext context;

        public FriendRepository(TeammateOnlineContext newContext)
        {
            context = newContext;
        }

        public Friend Add(Friend friend)
        {
            context.Friends.Add(friend);
            context.SaveChanges();

            return friend;
        }

        public Friend FinBdyId(int id)
        {
            return context.Friends.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Friend> GetAllByUserProfileId(int userProfileId)
        {
            return context.Friends.Where(x => x.UserProfileId == userProfileId);
        }

        public void Remove(Friend friend)
        {
            context.Remove(friend);
            context.SaveChanges();
        }
    }
}
