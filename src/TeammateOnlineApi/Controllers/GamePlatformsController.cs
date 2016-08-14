using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Annotations;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class GamePlatformsController : BaseController
    {
        public IGamePlatformRepository GamePlatformRepository;

        public GamePlatformsController(IGamePlatformRepository gamePlatformRepository)
        {
            GamePlatformRepository = gamePlatformRepository;
        }

        [HttpGet]
        public IEnumerable<GamePlatform> GetCollection()
        {
            return GamePlatformRepository.GetAll();
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(GamePlatform))]
        public IActionResult Post([FromBody]GamePlatform newGamePlatform)
        {
            var result = GamePlatformRepository.Add(newGamePlatform);

            return CreatedAtRoute("GamePlatformDetail", new { controller = "GamePlatformsController", gamePlatformId = result.Id }, result);
        }

        [HttpGet("{gamePlatformId}", Name = "GamePlatformDetail")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, "Game platform", typeof(GamePlatform))]
        public IActionResult GetDetail(int gamePlatformId)
        {
            var gamePlatform = GamePlatformRepository.FindById(gamePlatformId);

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
            var gamePlatform = GamePlatformRepository.FindById(gamePlatformId);

            if (gamePlatform == null)
            {
                return NotFound();
            }

            gamePlatform.Name = newGamePlatform.Name;
            gamePlatform.Url = newGamePlatform.Url;

            GamePlatformRepository.Update(gamePlatform);

            return new OkResult();
        }

        [HttpDelete("{gamePlatformId}")]
        public IActionResult Delete(int gamePlatformId)
        {
            var gamePlatform = GamePlatformRepository.FindById(gamePlatformId);

            if(gamePlatform == null)
            {
                return NotFound();
            }

            GamePlatformRepository.Remove(gamePlatform);

            return new NoContentResult();
        }
    }
}
