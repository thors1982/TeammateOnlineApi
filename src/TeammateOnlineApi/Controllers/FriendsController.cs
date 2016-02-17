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
    public class FriendsController : BaseController
    {
        public IFriendRepository Repository;

        public FriendsController(IFriendRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public IEnumerable<Friend> GetCollection()
        {
            return Repository.GetAll();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]Friend newFriend)
        {
            var result = Repository.Add(newFriend);

            return CreatedAtRoute("GetDetail", new { controller = "FriendsController", friendId = result.Id }, result);
        }

        [HttpGet("{friendId}")]
        public IActionResult GetDetail(int friendId)
        {
            var friend = Repository.FinBdyId(friendId);

            if (friend == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(friend);
        }

        [HttpDelete("{friendId}")]
        public IActionResult Delete(int friendId)
        {
            var friend = Repository.FinBdyId(friendId);

            if (friend == null)
            {
                return HttpNotFound();
            }

            Repository.Remove(friend);

            return new NoContentResult();
        }
    }
}