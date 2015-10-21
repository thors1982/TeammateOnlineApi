using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Helpers;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class UserProfilesController : BaseController
    {
        [HttpGet]
        public IEnumerable<UserProfile> GetCollection()
        {
            var userProfileList = TeammateOnlineContext.UserProfiles;

            return userProfileList.ToList();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]UserProfile newUserProfile)
        {
            if (TeammateOnlineContext.UserProfiles.FirstOrDefault(x => x.EmailAddress == newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = TeammateOnlineContext.UserProfiles.Add(newUserProfile);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "UserProfilesController", userProfileId = result.Entity.Id }, result.Entity);
        }

        [HttpGet("{userProfileId}")]
        public IActionResult GetDetail(int userProfileId)
        {
            var userProfile = TeammateOnlineContext.UserProfiles.FirstOrDefault(x => x.Id == userProfileId);

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
            var userProfile = TeammateOnlineContext.UserProfiles.FirstOrDefault(x => x.Id == userProfileId);

            if (userProfile == null)
            {
                return HttpNotFound();
            }

            if (userProfile.EmailAddress != newUserProfile.EmailAddress && TeammateOnlineContext.UserProfiles.FirstOrDefault(x => x.EmailAddress == newUserProfile.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            userProfile.FirstName = newUserProfile.FirstName;
            userProfile.LastName = newUserProfile.LastName;
            userProfile.EmailAddress = newUserProfile.EmailAddress;
            TeammateOnlineContext.UserProfiles.Update(userProfile);
            TeammateOnlineContext.SaveChanges();

            return new HttpOkResult();
        }
    }
}
