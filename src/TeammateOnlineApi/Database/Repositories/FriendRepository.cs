﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public Friend FindById(int id)
        {
            return context.Friends.Include(u => u.FriendUserProfile).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Friend> GetAllByUserProfileId(int userProfileId)
        {
            return context.Friends.Include(u => u.FriendUserProfile).Where(x => x.UserProfileId == userProfileId);
        }

        public Friend FindFriendOfAUser(int userProfileId, int friendUserProfileId)
        {
            return context.Friends.Include(u => u.FriendUserProfile).FirstOrDefault(x => x.UserProfileId == userProfileId && x.FriendUserProfileId == friendUserProfileId);
        }

        public bool IsUserAFriend(int userProfileId, int friendUserProfileId)
        {
            return context.Friends.Any(x => x.UserProfileId == userProfileId && x.FriendUserProfileId == friendUserProfileId);
        }

        public void Remove(Friend friend)
        {
            context.Remove(friend);
            context.SaveChanges();
        }
    }
}
