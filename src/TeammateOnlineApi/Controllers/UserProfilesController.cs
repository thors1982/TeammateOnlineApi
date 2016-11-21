using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Annotations;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Authorize]
    public class UserProfilesController : BaseController
    {
        private IUserProfileRepository userProfileRepository;

        public UserProfilesController(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        [HttpGet]
        public IActionResult GetCollection([FromQuery]string emailAddress = null, [FromQuery]string googleId = null, [FromQuery]string facebookId = null, [FromQuery]string query = null)
        {
            var userProfileList = new List<UserProfile>();

            if (!string.IsNullOrEmpty(googleId))
            {
                userProfileList.Add(userProfileRepository.FindByGoogleId(googleId));
            }
            else if (!string.IsNullOrEmpty(facebookId))
            {
                userProfileList.Add(userProfileRepository.FindByFacebookId(facebookId));
            }
            else if (!string.IsNullOrEmpty(emailAddress))
            {
                userProfileList.Add(userProfileRepository.FindByEmailAddress(emailAddress));
            }
            else if (!string.IsNullOrEmpty(query))
            {
                userProfileList.AddRange(GetUserProfilesFromQuery(query));
            }
            else
            {
                userProfileList = userProfileRepository.GetAll().ToList();
            }

            return new OkObjectResult(userProfileList);
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(UserProfile))]
        public IActionResult Post([FromBody]UserProfile newUserProfile)
        {
            if (userProfileRepository.FindByEmailAddress(newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = userProfileRepository.Add(newUserProfile);

            return CreatedAtRoute("UserProfileDetail", new { controller = "UserProfilesController", userProfileId = result.Id }, result);
        }

        [HttpGet("{userProfileId}", Name = "UserProfileDetail")]
        public IActionResult GetDetail(int userProfileId)
        {
            var userProfile = userProfileRepository.FindById(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            return new OkObjectResult(userProfile);
        }

        [HttpPut("{userProfileId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, [FromBody]UserProfile newUserProfile)
        {
            var userProfile = userProfileRepository.FindById(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            if (userProfile.EmailAddress != newUserProfile.EmailAddress && userProfileRepository.FindByEmailAddress(newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            userProfile.FirstName = newUserProfile.FirstName;
            userProfile.LastName = newUserProfile.LastName;
            userProfile.EmailAddress = newUserProfile.EmailAddress;

            userProfileRepository.Update(userProfile);

            return new OkResult();
        }

        [NonAction]
        public IEnumerable<UserProfile> GetUserProfilesFromQuery(string query)
        {
            return userProfileRepository.Query(query);
        }
    }
}
