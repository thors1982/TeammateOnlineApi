using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Filters;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Controllers
{
    public class FriendsController : BaseController
    {
        [HttpGet]
        public IEnumerable<Friend> GetCollection()
        {
            var friendList = TeammateOnlineContext.Friends;

            return friendList.ToList();
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]Friend newFriend)
        {
            var result = TeammateOnlineContext.Friends.Add(newFriend);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "FriendsController", friendId = result.Entity.Id }, result.Entity);
        }

        [HttpGet("{friendId}")]
        public IActionResult GetDetail(int friendId)
        {
            var friend = TeammateOnlineContext.Friends.FirstOrDefault(x => x.Id == friendId);

            if (friend == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(friend);
        }

        [HttpDelete("{friendId}")]
        public IActionResult Delete(int friendId)
        {
            var friend = TeammateOnlineContext.Friends.FirstOrDefault(x => x.Id == friendId);

            if (friend == null)
            {
                return HttpNotFound();
            }

            var result = TeammateOnlineContext.Friends.Remove(friend);
            TeammateOnlineContext.SaveChanges();

            return new NoContentResult();
        }
    }
}