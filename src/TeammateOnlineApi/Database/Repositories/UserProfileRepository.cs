using System;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private TeammateOnlineContext context;

        public UserProfileRepository(TeammateOnlineContext newContext)
        {
            context = newContext;
        }

        public UserProfile Add(UserProfile userProfile)
        {
            context.UserProfiles.Add(userProfile);
            context.SaveChanges();

            return userProfile;
        }

        public UserProfile FindById(int id)
        {
            return context.UserProfiles.FirstOrDefault(x => x.Id == id);
        }

        public UserProfile FindByEmailAddress(string emailAddress)
        {
            return context.UserProfiles.FirstOrDefault(x => x.EmailAddress == emailAddress);
        }

        public UserProfile FindByGoogleId(string googleId)
        {
            return context.UserProfiles.FirstOrDefault(x => x.GoogleId == googleId);
        }

        public UserProfile FindByFacebookId(string facebookId)
        {
            return context.UserProfiles.FirstOrDefault(x => x.FacebookId == facebookId);
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return context.UserProfiles.ToList();
        }

        public void Update(UserProfile userProfile)
        {
            context.UserProfiles.Update(userProfile);
            context.SaveChanges();
        }

        public IEnumerable<UserProfile> Query(string query, int count = 10)
        {
            return context.UserProfiles.Where(x => x.EmailAddress.Contains(query)).Take(count);
        }
    }
}
