using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class GamePlatformsController : BaseController
    {
        public IGamePlatformRepository Repository;

        public GamePlatformsController(IGamePlatformRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public IEnumerable<GamePlatform> GetCollection()
        {
            return Repository.GetAll();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]GamePlatform newGamePlatform)
        {
            var result = Repository.Add(newGamePlatform);

            return CreatedAtRoute("GamePlatformDetail", new { controller = "GamePlatformsController", gamePlatformId = result.Id }, result);
        }

        [HttpGet("{gamePlatformId}", Name = "GamePlatformDetail")]
        public IActionResult GetDetail(int gamePlatformId)
        {
            var gamePlatform = Repository.FinBdyId(gamePlatformId);

            if(gamePlatform == null)
            {
                return NotFound();
            }

            return new OkObjectResult(gamePlatform);
        }

        [HttpPut("{gamePlatformId}")]
        [ValidateModelState]
        public IActionResult Put(int gamePlatformId, [FromBody]GamePlatform newGamePlatform)
        {
            var gamePlatform = Repository.FinBdyId(gamePlatformId);

            if (gamePlatform == null)
            {
                return NotFound();
            }

            gamePlatform.Name = newGamePlatform.Name;
            gamePlatform.Url = newGamePlatform.Url;

            Repository.Update(gamePlatform);

            return new OkResult();
        }

        [HttpDelete("{gamePlatformId}")]
        public IActionResult Delete(int gamePlatformId)
        {
            var gamePlatform = Repository.FinBdyId(gamePlatformId);

            if(gamePlatform == null)
            {
                return NotFound();
            }

            Repository.Remove(gamePlatform);

            return new NoContentResult();
        }
    }
}
