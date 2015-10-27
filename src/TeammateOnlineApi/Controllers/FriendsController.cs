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
    public class FriendsController : BaseController
    {
        [HttpGet]
        public IEnumerable<FriendViewModel> GetCollection()
        {
            return Mapper.Map<IEnumerable<FriendViewModel>>(TeammateOnlineContext.Friends);
        }

        [HttpPost]
        [ValidateModelState]
        public IActionResult Post([FromBody]FriendViewModel request)
        {
            var newFriend = Mapper.Map<Friend>(request);

            TeammateOnlineContext.Friends.Add(newFriend);
            TeammateOnlineContext.SaveChanges();

            return CreatedAtRoute("GetDetail", new { controller = "FriendsController", friendId = newFriend.Id }, Mapper.Map<FriendViewModel>(newFriend));
        }

        [HttpGet("{friendId}")]
        public IActionResult GetDetail(int friendId)
        {
            var friend = TeammateOnlineContext.Friends.FirstOrDefault(x => x.Id == friendId);

            if (friend == null)
            {
                return HttpNotFound();
            }

            return new HttpOkObjectResult(Mapper.Map<FriendViewModel>(friend));
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