using System;
using System.Collections.Generic;
using System.Linq;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private TeammateOnlineContext context;

        public GroupRepository(TeammateOnlineContext newContext)
        {
            context = newContext;
        }

        public Group Add(Group group)
        {
            context.Groups.Add(group);
            context.SaveChanges();

            return group;
        }

        public Group FindById(int id)
        {
            return context.Groups.FirstOrDefault(x => x.Id == id);
        }

        public Group FindByName(string name)
        {
            return context.Groups.FirstOrDefault(x => x.Name == name);
        }

        public IEnumerable<Group> GetAll()
        {
            return context.Groups.ToList();
        }

        public void Remove(Group group)
        {
            context.Remove(group);
            context.SaveChanges();
        }

        public void Update(Group group)
        {
            context.Groups.Update(group);
            context.SaveChanges();
        }

        public IEnumerable<Group> Query(string query, int count = 10)
        {
            return context.Groups.Where(x => x.Name.Contains(query)).Take(count);
        }
    }
}
