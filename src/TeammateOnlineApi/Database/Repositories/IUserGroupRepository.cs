using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IUserGroupRepository
    {
        UserGroup Add(UserGroup userGroup);

        IEnumerable<UserGroup> GetAllByUserProfileId(int userProfileId);

        IEnumerable<UserGroup> GetAllByGroupId(int groupId);

        UserGroup FindGroupOfAUser(int userProfileId, int groupId);

        void Remove(UserGroup userGroup);

        void Update(UserGroup userGroup);
    }
}
