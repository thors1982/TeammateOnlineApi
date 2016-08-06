using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public class GamePlatformRepository : IGamePlatformRepository
    {
        private TeammateOnlineContext context;

        public GamePlatformRepository(TeammateOnlineContext newContext)
        {
            context = newContext;
        }

        public GamePlatform Add(GamePlatform gamePlatform)
        {
            context.GamePlatforms.Add(gamePlatform);
            context.SaveChanges();

            return gamePlatform;
        }

        public GamePlatform FindById(int id)
        {
            return context.GamePlatforms.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<GamePlatform> GetAll()
        {
            return context.GamePlatforms.ToList();
        }

        public void Remove(GamePlatform gamePlatform)
        {
            context.Remove(gamePlatform);
            context.SaveChanges();
        }

        public void Update(GamePlatform gamePlatform)
        {
            context.GamePlatforms.Update(gamePlatform);
            context.SaveChanges();
        }
    }
}
