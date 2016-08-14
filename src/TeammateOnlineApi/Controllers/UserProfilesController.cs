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
        public IUserProfileRepository UserProfileRepository;

        public UserProfilesController(IUserProfileRepository userProfileRepository)
        {
            UserProfileRepository = userProfileRepository;
        }

        [HttpGet]
        public IEnumerable<UserProfile> GetCollection([FromQuery]string emailAddress = null, [FromQuery]string googleId = null, [FromQuery]string facebookId = null, [FromQuery]string query = null)
        {
            var userProfileList = new List<UserProfile>();

            if (!string.IsNullOrEmpty(googleId))
            {
                userProfileList.Add(UserProfileRepository.FindByGoogleId(googleId));
            }
            else if (!string.IsNullOrEmpty(facebookId))
            {
                userProfileList.Add(UserProfileRepository.FindByFacebookId(facebookId));
            }
            else if (!string.IsNullOrEmpty(emailAddress))
            {
                userProfileList.Add(UserProfileRepository.FindByEmailAddress(emailAddress));
            }
            else if (!string.IsNullOrEmpty(query))
            {
                userProfileList.AddRange(UserProfileRepository.Query(query));
            }
            else
            {
                userProfileList = UserProfileRepository.GetAll().ToList();
            }

            return userProfileList;
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(UserProfile))]
        public IActionResult Post([FromBody]UserProfile newUserProfile)
        {
            if (UserProfileRepository.FindByEmailAddress(newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = UserProfileRepository.Add(newUserProfile);

            return CreatedAtRoute("UserProfileDetail", new { controller = "UserProfilesController", userProfileId = result.Id }, result);
        }

        [HttpGet("{userProfileId}", Name = "UserProfileDetail")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, "User profile", typeof(UserProfile))]
        public IActionResult GetDetail(int userProfileId)
        {
            var userProfile = UserProfileRepository.FindById(userProfileId);

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
            var userProfile = UserProfileRepository.FindById(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            if (userProfile.EmailAddress != newUserProfile.EmailAddress && UserProfileRepository.FindByEmailAddress(newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            userProfile.FirstName = newUserProfile.FirstName;
            userProfile.LastName = newUserProfile.LastName;
            userProfile.EmailAddress = newUserProfile.EmailAddress;

            UserProfileRepository.Update(userProfile);

            return new OkResult();
        }
    }
}
