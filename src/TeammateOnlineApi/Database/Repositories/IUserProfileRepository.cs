using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile Add(UserProfile userProfile);
        UserProfile FindById(int id);
        UserProfile FindByEmailAddress(string emailAddress);
        UserProfile FindByGoogleId(string googleId);
        UserProfile FindByFacebookId(string facebookId);
        IEnumerable<UserProfile> GetAll();
        void Update(UserProfile userProfile);
        IEnumerable<UserProfile> Query(string query, int count = 10);
    }
}
