using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    [Authorize]
    [Route("api/UserProfiles/{userProfileId}/Groups")]
    public class UserGroupsController : BaseController
    {
        private IUserGroupRepository userGroupRepository;

        private IUserProfileRepository userProfileRepository;

        public UserGroupsController(IUserGroupRepository userGroupRepository, IUserProfileRepository userProfileRepository)
        {
            this.userGroupRepository = userGroupRepository;
            this.userProfileRepository = userProfileRepository;
        }

        [HttpGet]
        public IActionResult GetCollection(int userProfileId)
        {
            var userProfile = userProfileRepository.FindById(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            return new OkObjectResult(userGroupRepository.GetAllByUserProfileId(userProfileId));
        }

        [HttpPost]
        [ValidateModelState]
        [Produces(typeof(UserGroup))]
        public IActionResult Post(int userProfileId, [FromBody]UserGroup newUserGroup)
        {
            var userProfile = userProfileRepository.FindById(userProfileId);

            if (userProfile == null)
            {
                return NotFound();
            }

            if (userGroupRepository.FindGroupOfAUser(userProfileId, newUserGroup.GroupId) != null)
            {
                ModelState.AddModelError("GroupId", "Group already exists.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = userGroupRepository.Add(newUserGroup);

            return CreatedAtRoute("UserGroupDetail", new { controller = "UserGroupsController", userGroupId = result.Id }, result);
        }
    }
}
