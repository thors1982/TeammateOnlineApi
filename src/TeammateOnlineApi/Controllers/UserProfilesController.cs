using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;
using TeammateOnlineApi.ViewModels;

namespace TeammateOnlineApi.Controllers
{
    public class UserProfilesController : BaseController
    {
        [HttpGet]
        public IEnumerable<UserProfileViewModel> GetCollection()
        {
            return Mapper.Map<IEnumerable<UserProfileViewModel>>(TeammateOnlineContext.UserProfiles);
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]UserProfileViewModel request)
        {
            if (TeammateOnlineContext.UserProfiles.FirstOrDefault(x => x.EmailAddress == request.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            var newUserProfile = Mapper.Map<UserProfile>(request);

            TeammateOnlineContext.UserProfiles.Add(newUserProfile);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "UserProfilesController", userProfileId = newUserProfile.Id }, Mapper.Map<UserProfileViewModel>(newUserProfile));
        }

        [HttpGet("{userProfileId}")]
        public IActionResult GetDetail(int userProfileId)
        {
            var userProfile = TeammateOnlineContext.UserProfiles.FirstOrDefault(x => x.Id == userProfileId);

            if (userProfile == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(Mapper.Map<UserProfileViewModel>(userProfile));
        }

        [HttpPut("{userProfileId}")]
        [ValidateModelState]
        public IActionResult Put(int userProfileId, [FromBody]UserProfile request)
        {
            var userProfile = TeammateOnlineContext.UserProfiles.FirstOrDefault(x => x.Id == userProfileId);

            if (userProfile == null)
            {
                return HttpNotFound();
            }

            if (userProfile.EmailAddress != request.EmailAddress && TeammateOnlineContext.UserProfiles.FirstOrDefault(x => x.EmailAddress == request.EmailAddress) != null)
            {
                ModelState.AddModelError("EmailAddress", "Email address already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            var newUserProfile = Mapper.Map<UserProfile>(request);
            userProfile.FirstName = newUserProfile.FirstName;
            userProfile.LastName = newUserProfile.LastName;
            userProfile.EmailAddress = newUserProfile.EmailAddress;
            TeammateOnlineContext.UserProfiles.Update(userProfile);
            TeammateOnlineContext.SaveChanges();

            return new HttpOkResult();
        }
    }
}
