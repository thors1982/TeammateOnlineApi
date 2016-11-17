using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Annotations;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class GamePlatformsController : BaseController
    {
        private IGamePlatformRepository gamePlatformRepository;

        public GamePlatformsController(IGamePlatformRepository gamePlatformRepository)
        {
            this.gamePlatformRepository = gamePlatformRepository;
        }

        [HttpGet]
        public IActionResult GetCollection()
        {
            return new OkObjectResult(gamePlatformRepository.GetAll());
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(GamePlatform))]
        public IActionResult Post([FromBody]GamePlatform newGamePlatform)
        {
            var result = gamePlatformRepository.Add(newGamePlatform);

            return CreatedAtRoute("GamePlatformDetail", new { controller = "GamePlatformsController", gamePlatformId = result.Id }, result);
        }

        [HttpGet("{gamePlatformId}", Name = "GamePlatformDetail")]
        public IActionResult GetDetail(int gamePlatformId)
        {
            var gamePlatform = gamePlatformRepository.FindById(gamePlatformId);

            if (gamePlatform == null)
            {
                return NotFound();
            }

            return new OkObjectResult(gamePlatform);
        }

        [HttpPut("{gamePlatformId}")]
        [ValidateModelState]
        public IActionResult Put(int gamePlatformId, [FromBody]GamePlatform newGamePlatform)
        {
            var gamePlatform = gamePlatformRepository.FindById(gamePlatformId);

            if (gamePlatform == null)
            {
                return NotFound();
            }

            gamePlatform.Name = newGamePlatform.Name;
            gamePlatform.Url = newGamePlatform.Url;

            gamePlatformRepository.Update(gamePlatform);

            return new OkResult();
        }

        [HttpDelete("{gamePlatformId}")]
        public IActionResult Delete(int gamePlatformId)
        {
            var gamePlatform = gamePlatformRepository.FindById(gamePlatformId);

            if (gamePlatform == null)
            {
                return NotFound();
            }

            gamePlatformRepository.Remove(gamePlatform);

            return new NoContentResult();
        }
    }
}
