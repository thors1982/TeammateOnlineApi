using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class IdentityController : Controller
    {
        [HttpGet("identity/login")]
        public async Task<IActionResult> Login()
        {
            var claims = new List<Claim>
            {
                new Claim("sub", "12345"),
                new Claim("name", "Nick Stark"),
                new Claim("email", "thors1982@gmail.com")
            };

            var id = new ClaimsIdentity(claims, "test");

            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));

            return LocalRedirect("/");
        }

        [HttpGet("identity/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("3rdPartyLogin");

            return LocalRedirect("/");
        }

        [HttpGet("identity/google")]
        public IActionResult Google()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/identity/google/callback"
            };
            
            return new ChallengeResult("Google", properties);
        }

        [HttpGet("identity/google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            return await ThirdPartyLogin("google");
        }

        [HttpGet("identity/facebook")]
        public IActionResult Facebook()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/identity/facebook/callback"
            };

            return new ChallengeResult("Facebook", properties);
        }

        [HttpGet("identity/facebook/callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            return await ThirdPartyLogin("facebook");
        }

        private async Task<IActionResult> ThirdPartyLogin(string type)
        {
            var exteranlIdentity = await HttpContext.Authentication.AuthenticateAsync("3rdPartyLogin");

            //TODO: Lookup in database for this user id

            var claims = new List<Claim>
            {
                new Claim("sub", "12345"),
                new Claim("name", exteranlIdentity.FindFirst(ClaimTypes.Name).Value),
            };

            // This logic incase facebook doesn't return email address
            if(exteranlIdentity.FindFirst(ClaimTypes.Email) != null)
            {
                claims.Add(new Claim("email", exteranlIdentity.FindFirst(ClaimTypes.Email).Value));
            }

            var id = new ClaimsIdentity(claims, type);

            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
            await HttpContext.Authentication.SignOutAsync("3rdPartyLogin");

            return LocalRedirect("/");
        }
    }
}
