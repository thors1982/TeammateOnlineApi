using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private TeammateOnlineContext context;

        public UserGroupRepository(TeammateOnlineContext newContext)
        {
            context = newContext;
        }

        public UserGroup Add(UserGroup userGroup)
        {
            context.UserGroups.Add(userGroup);
            context.SaveChanges();

            return userGroup;
        }

        public IEnumerable<UserGroup> GetAllByGroupId(int groupId)
        {
            return context.UserGroups.Include(g => g.GroupId).Where(x => x.GroupId == groupId);
        }

        public IEnumerable<UserGroup> GetAllByUserProfileId(int userProfileId)
        {
            return context.UserGroups.Include(g => g.GroupId).Where(x => x.UserProfileId == userProfileId);
        }

        public UserGroup FindGroupOfAUser(int userProfileId, int groupId)
        {
            return context.UserGroups.Include(g => g.GroupId).FirstOrDefault(x => x.UserProfileId == userProfileId && x.GroupId == groupId);
        }

        public void Remove(UserGroup userGroup)
        {
            context.Remove(userGroup);
            context.SaveChanges();
        }

        public void Update(UserGroup userGroup)
        {
            context.UserGroups.Update(userGroup);
            context.SaveChanges();
        }
    }
}
