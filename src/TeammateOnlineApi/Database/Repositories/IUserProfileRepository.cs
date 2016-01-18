using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile Add(UserProfile userProfile);
        UserProfile FinBdyId(int id);
        UserProfile FindByEmailAddress(string emailAddress);
        IEnumerable<UserProfile> GetAll();
        void Update(UserProfile userProfile);
    }
}
