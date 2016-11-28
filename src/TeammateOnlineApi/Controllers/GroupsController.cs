using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen.Annotations;
using TeammateOnlineApi.Database.Repositories;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class GroupsController : BaseController
    {
        private IGroupRepository groupRepository;

        public GroupsController(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        [HttpGet]
        public IActionResult GetCollection()
        {
            return new OkObjectResult(groupRepository.GetAll());
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]Group newGroup)
        {
            if (groupRepository.FindByName(newGroup.Name) != null)
            {
                ModelState.AddModelError("Name", "Name already taken.");
                return new BadRequestObjectResult(ModelState);
            }

            var result = groupRepository.Add(newGroup);
            // Add UserGroup for creator (as an admin)

            return CreatedAtRoute("GroupDetail", new { controller = "GroupsController", groupId = result.Id }, result);
        }

        [HttpGet("{groupId}", Name = "GroupDetail")]
        public IActionResult GetDetail(int groupId)
        {
            var group = groupRepository.FindById(groupId);

            if (group == null)
            {
                return NotFound();
            }

            return new OkObjectResult(group);
        }

        [HttpPut("{groupId}")]
        [ValidateModelState]
        public IActionResult Put(int groupId, [FromBody]Group newGroup)
        {
            //Todo: make sure an Admin is modifying it
            var group = groupRepository.FindById(groupId);

            if (group == null)
            {
                return NotFound();
            }

            group.Name = newGroup.Name;
            group.IsPrivate = newGroup.IsPrivate;
            group.Description = newGroup.Description;

            groupRepository.Update(group);

            return new OkResult();
        }

        [HttpDelete("{groupId}")]
        public IActionResult Delete(int groupId)
        {
            //Todo: make sure an admin is deleting it
            var group = groupRepository.FindById(groupId);

            if (group == null)
            {
                return NotFound();
            }

            groupRepository.Remove(group);

            return new NoContentResult();
        }
    }
}
