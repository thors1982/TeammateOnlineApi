using System.Collections.Generic;
using TeammateOnlineApi.Models;


namespace TeammateOnlineApi.Database.Repositories
{
    public interface IGameAccountRepository
    {
        GameAccount Add(GameAccount gameAccount);
        GameAccount FinBdyId(int id);
        IEnumerable<GameAccount> GetAllByUserProfileId(int userProfileId);
        void Remove(GameAccount gameAccount);
        void Update(GameAccount gameAccount);
    }
}
