using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IGamePlatformRepository
    {
        GamePlatform Add(GamePlatform gamePlatform);
        GamePlatform FinBdyId(int id);
        IEnumerable<GamePlatform> GetAll();
        void Remove(GamePlatform gamePlatform);
        void Update(GamePlatform gamePlatform);
    }
}