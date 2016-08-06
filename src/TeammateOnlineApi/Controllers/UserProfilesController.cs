using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class UserProfilesController : BaseController
    {
        public IUserProfileRepository Repository;

        public UserProfilesController(IUserProfileRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public IEnumerable<UserProfile> GetCollection([FromQuery]string emailAddress = null, [FromQuery]string googleId = null, [FromQuery]string facebookId = null, [FromQuery]string query = null)
        {
            var userProfileList = new List<UserProfile>();

            if (!string.IsNullOrEmpty(googleId))
            {
                userProfileList.Add(Repository.FindByGoogleId(googleId));
            }
            else if(!string.IsNullOrEmpty(facebookId))
            {
                userProfileList.Add(Repository.FindByFacebookId(facebookId));
            }
            else if(!string.IsNullOrEmpty(emailAddress))
            {
                userProfileList.Add(Repository.FindByEmailAddress(emailAddress));
            }
            else if(!string.IsNullOrEmpty(query))
            {
                userProfileList.AddRange(Repository.Query(query));
            }
            else
            {
                userProfileList = Repository.GetAll().ToList();
            }

            return userProfileList;
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(UserProfile))]
        public IActionResult Post([FromBody]UserProfile newUserProfile)
        {
            if (Repository.FindByEmailAddress(newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = Repository.Add(newUserProfile);

            return CreatedAtRoute("UserProfileDetail", new { controller = "UserProfilesController", userProfileId = result.Id }, result);
        }

        [HttpGet("{userProfileId}", Name = "UserProfileDetail")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, "User profile", typeof(UserProfile))]
        public IActionResult GetDetail(int userProfileId)
        {
            var userProfile = Repository.FindById(userProfileId);

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
            var userProfile = Repository.FindById(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            if (userProfile.EmailAddress != newUserProfile.EmailAddress && Repository.FindByEmailAddress(newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            userProfile.FirstName = newUserProfile.FirstName;
            userProfile.LastName = newUserProfile.LastName;
            userProfile.EmailAddress = newUserProfile.EmailAddress;

            Repository.Update(userProfile);

            return new OkResult();
        }
    }
}
