using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class IdentityController : Controller
    {
        public IUserProfileRepository Repository;

        public IdentityController(IUserProfileRepository repository)
        {
            Repository = repository;
        }

        /*[HttpGet("identity/login")]
        public async Task<IActionResult> Login()
        {
            var claims = new List<Claim>
            {
                new Claim("sub", "12345"),
                new Claim("name", "Tony Stark"),
                new Claim("email", "tony.stark@ironman.com")
            };

            var id = new ClaimsIdentity(claims, "test");

            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));

            return LocalRedirect("/");
        }*/

        [HttpGet("Identity/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("3rdPartyLogin");

            return LocalRedirect("/");
        }

        [HttpGet("Identity/Google")]
        public IActionResult Google()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/identity/google/callback"
            };
            
            return new ChallengeResult("Google", properties);
        }

        [HttpGet("Identity/Google/Callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var exteranlIdentity = await HttpContext.Authentication.AuthenticateAsync("3rdPartyLogin");
           
            var googleId = exteranlIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userProfileContoller = new UserProfilesController(Repository);
            var userProfileList = userProfileContoller.GetCollection(googleId) as List<UserProfile>;
            
            if(userProfileList.Any())
            {
                var createUser = new UserProfile
                {
                    GoogleId = googleId,
                    FirstName = exteranlIdentity.FindFirst(ClaimTypes.GivenName).Value,
                    LastName = exteranlIdentity.FindFirst(ClaimTypes.Surname).Value,
                    EmailAddress = exteranlIdentity.FindFirst(ClaimTypes.Email).Value,
                };

                var response = userProfileContoller.Post(createUser);

                userProfileList = userProfileContoller.GetCollection(googleId) as List<UserProfile>;
            }

            signInUser(userProfileList.First());

            return LocalRedirect("/");
        }

        [HttpGet("Identity/Facebook")]
        public IActionResult Facebook()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/identity/facebook/callback"
            };

            return new ChallengeResult("Facebook", properties);
        }

        [HttpGet("Identity/Facebook/Callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var exteranlIdentity = await HttpContext.Authentication.AuthenticateAsync("3rdPartyLogin");

            var facebookId = exteranlIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userProfileContoller = new UserProfilesController(Repository);
            var userProfileList = userProfileContoller.GetCollection(facebookId) as List<UserProfile>;

            if (userProfileList.Any())
            {
                var createUser = new UserProfile
                {
                    FacebookId = facebookId,
                    FirstName = exteranlIdentity.FindFirst(ClaimTypes.GivenName).Value,
                    LastName = exteranlIdentity.FindFirst(ClaimTypes.Surname).Value,
                    //Email
                };

                var response = userProfileContoller.Post(createUser);

                userProfileList = userProfileContoller.GetCollection(facebookId) as List<UserProfile>;
            }

            signInUser(userProfileList.First());

            return LocalRedirect("/");
        }

        private async void signInUser(UserProfile userProfile)
        {
            var claims = new List<Claim>
            {
                new Claim("sub", userProfile.Id.ToString()),
                new Claim("firstname", userProfile.FirstName),
                new Claim("lastname", userProfile.LastName),
                new Claim("email", userProfile.EmailAddress),
                new Claim("role", "default")
            };

            var id = new ClaimsIdentity(claims, "google");

            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
            await HttpContext.Authentication.SignOutAsync("3rdPartyLogin");
        }
    }
}
