using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public class GameAccountRepository : IGameAccountRepository
    {
        private TeammateOnlineContext context;

        public GameAccountRepository(TeammateOnlineContext newContext)
        {
            context = newContext;
        }

        public GameAccount Add(GameAccount gameAccount)
        {
            context.GameAccounts.Add(gameAccount);
            context.SaveChanges();

            return gameAccount;
        }

        public GameAccount FindById(int id)
        {
            return context.GameAccounts.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<GameAccount> GetAllByUserProfileId(int userProfileId)
        {
            return context.GameAccounts.Where(x => x.UserProfileId == userProfileId);
        }

        public void Remove(GameAccount gameAccount)
        {
            context.Remove(gameAccount);
            context.SaveChanges();
        }

        public void Update(GameAccount gameAccount)
        {
            context.GameAccounts.Update(gameAccount);
            context.SaveChanges();
        }

        public IEnumerable<GameAccount> Query(string query, int count = 10)
        {
            return context.GameAccounts.Where(x => x.UserName.Contains(query)).Take(count);
        }
    }
}
