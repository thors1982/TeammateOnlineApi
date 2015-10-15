using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Helpers;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class GameServicesController : BaseController
    {
        [HttpGet]
        public IEnumerable<GameService> GetCollection()
        {
            var gameServiceList = TeammateOnlineContext.GameServices;

            return gameServiceList.ToList();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]GameService newGameService)
        {
            var result = TeammateOnlineContext.GameServices.Add(newGameService);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "GameServiceController", gameServiceId = result.Entity.GameServiceId }, result.Entity);
        }

        [HttpGet("{gameServiceId}")]
        public IActionResult GetDetail(int gameServiceId)
        {
            var gameService = TeammateOnlineContext.GameServices.First(x => x.GameServiceId == gameServiceId);

            return new HttpOkObjectResult(gameService);
        }

        [HttpPut("{gameServiceId}")]
        [ValidateModelState]
        public IActionResult Put(int gameServiceId, [FromBody]GameService newGameService)
        {
            var gameService = TeammateOnlineContext.GameServices.First(x => x.GameServiceId == gameServiceId);

            if (gameService == null)
            {
                return HttpNotFound();
            }

            gameService.Name = newGameService.Name;
            gameService.Url = newGameService.Url;
            TeammateOnlineContext.GameServices.Update(gameService);
            TeammateOnlineContext.SaveChanges();

            return new HttpOkResult();
        }

        [HttpDelete("{gameServiceId}")]
        public IActionResult Delete(int gameServiceId)
        {
            var gameService = TeammateOnlineContext.GameServices.First(x => x.GameServiceId == gameServiceId);

            if(gameService == null)
            {
                return HttpNotFound();
            }

            var result = TeammateOnlineContext.GameServices.Remove(gameService);
            TeammateOnlineContext.SaveChanges();

            return new NoContentResult();
        }
    }
}
