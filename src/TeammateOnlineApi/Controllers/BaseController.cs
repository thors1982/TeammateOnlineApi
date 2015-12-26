using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        [FromServices]
        public TeammateOnlineContext TeammateOnlineContext { get; set; }

        [FromServices]
        public ILogger<GamePlatformsController> Logger { get; set; }
    }
}
