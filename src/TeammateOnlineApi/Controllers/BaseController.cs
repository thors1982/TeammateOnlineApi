using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TeammateOnlineApi.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        public ILogger<GamePlatformsController> Logger { get; set; }
    }
}
