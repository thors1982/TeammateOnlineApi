using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Helpers;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class GamePlatformsController : BaseController
    {
        [HttpGet]
        public IEnumerable<GamePlatform> GetCollection()
        {
            var gamePlatformList = TeammateOnlineContext.GamePlatforms;

            return gamePlatformList.ToList();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]GamePlatform newGamePlatform)
        {
            var result = TeammateOnlineContext.GamePlatforms.Add(newGamePlatform);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "GamePlatformsController", gamePlatformId = result.Entity.Id }, result.Entity);
        }

        [HttpGet("{gamePlatformId}")]
        public IActionResult GetDetail(int gamePlatformId)
        {
            var gamePlatform = TeammateOnlineContext.GamePlatforms.FirstOrDefault(x => x.Id == gamePlatformId);

            if(gamePlatform == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(gamePlatform);
        }

        [HttpPut("{gamePlatformId}")]
        [ValidateModelState]
        public IActionResult Put(int gamePlatformId, [FromBody]GamePlatform newGamePlatform)
        {
            var gamePlatform = TeammateOnlineContext.GamePlatforms.FirstOrDefault(x => x.Id == gamePlatformId);

            if (gamePlatform == null)
            {
                return HttpNotFound();
            }

            gamePlatform.Name = newGamePlatform.Name;
            gamePlatform.Url = newGamePlatform.Url;
            gamePlatform.Active = newGamePlatform.Active;
            TeammateOnlineContext.GamePlatforms.Update(gamePlatform);
            TeammateOnlineContext.SaveChanges();

            return new HttpOkResult();
        }

        [HttpDelete("{gamePlatformId}")]
        public IActionResult Delete(int gamePlatformId)
        {
            var gamePlatform = TeammateOnlineContext.GamePlatforms.FirstOrDefault(x => x.Id == gamePlatformId);

            if(gamePlatform == null)
            {
                return HttpNotFound();
            }

            var result = TeammateOnlineContext.GamePlatforms.Remove(gamePlatform);
            TeammateOnlineContext.SaveChanges();

            return new NoContentResult();
        }
    }
}
