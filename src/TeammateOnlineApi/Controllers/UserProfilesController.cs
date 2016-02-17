using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
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
        public IEnumerable<UserProfile> GetCollection()
        {
            return Repository.GetAll();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]UserProfile newUserProfile)
        {
            if (Repository.FindByEmailAddress(newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = Repository.Add(newUserProfile);

            return CreatedAtRoute("GetDetail", new { controller = "UserProfilesController", userProfileId = result.Id }, result);
        }

        [HttpGet("{userProfileId}")]
        public IActionResult GetDetail(int userProfileId)
        {
            var userProfile = Repository.FinBdyId(userProfileId);

            if (userProfile == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(userProfile);
        }

        [HttpPut("{userProfileId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, [FromBody]UserProfile newUserProfile)
        {
            var userProfile = Repository.FinBdyId(userProfileId);

            if (userProfile == null)
            {
                return HttpNotFound();
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

            return new HttpOkResult();
        }
    }
}
