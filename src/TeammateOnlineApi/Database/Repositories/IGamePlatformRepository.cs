using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IGamePlatformRepository
    {
        GamePlatform Add(GamePlatform gamePlatform);

        GamePlatform FindById(int id);

        IEnumerable<GamePlatform> GetAll();

        void Remove(GamePlatform gamePlatform);

        void Update(GamePlatform gamePlatform);
    }
}