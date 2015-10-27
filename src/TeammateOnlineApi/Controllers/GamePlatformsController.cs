using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;
using TeammateOnlineApi.ViewModels;

namespace TeammateOnlineApi.Controllers
{
    public class GamePlatformsController : BaseController
    {
        [HttpGet]
        public IEnumerable<GamePlatformViewModel> GetCollection()
        {
            return Mapper.Map<IEnumerable<GamePlatformViewModel>>(TeammateOnlineContext.GamePlatforms);
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]GamePlatformViewModel request)
        {
            var newGamePlatform = Mapper.Map<GamePlatform>(request);

            TeammateOnlineContext.GamePlatforms.Add(newGamePlatform);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "GamePlatformsController", gamePlatformId = newGamePlatform.Id }, Mapper.Map<GamePlatformViewModel>(newGamePlatform));
        }

        [HttpGet("{gamePlatformId}", Name ="GetDetail")]
        public IActionResult GetDetail(int gamePlatformId)
        {
            var gamePlatform = TeammateOnlineContext.GamePlatforms.FirstOrDefault(x => x.Id == gamePlatformId);

            if(gamePlatform == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(Mapper.Map<GamePlatformViewModel>(gamePlatform));
        }

        [HttpPut("{gamePlatformId}")]
        [ValidateModelState]
        public IActionResult Put(int gamePlatformId, [FromBody]GamePlatformViewModel request)
        {
            var gamePlatform = TeammateOnlineContext.GamePlatforms.FirstOrDefault(x => x.Id == gamePlatformId);

            if (gamePlatform == null)
            {
                return HttpNotFound();
            }

            var newGamePlatform = Mapper.Map<GamePlatform>(request);
            gamePlatform.Name = newGamePlatform.Name;
            gamePlatform.Url = newGamePlatform.Url;
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
