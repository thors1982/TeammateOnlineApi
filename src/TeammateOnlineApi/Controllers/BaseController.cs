using Microsoft.AspNetCore.Mvc;
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
        public ILogger<GamePlatformsController> Logger { get; set; }
    }
}
