using System.Linq;
using System.Security.Claims;
using TeammateOnlineApi.Database.Repositories;

namespace TeammateOnlineApi.Filters
{
    public class AuthorizationFilter
    {
        public bool IsUserAuthorizedForUser(ClaimsPrincipal currentUser, int userId)
        {
            var currentUserId = currentUser.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            return int.Parse(currentUserId) == userId;
        }

        public bool IsUserAuthorizedForFriend(ClaimsPrincipal currentUser, int friendId, IFriendRepository friendRepository)
        {
            var currentUserId = currentUser.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            return friendRepository.IsUserAFriend(int.Parse(currentUserId), friendId);
        }

        public bool IsUserAuthorizedForUserOrFriend(ClaimsPrincipal currentUser, int otherUserId, IFriendRepository friendRepository)
        {
            return IsUserAuthorizedForUser(currentUser, otherUserId) || IsUserAuthorizedForFriend(currentUser, otherUserId, friendRepository);
        }
    }
}
