using System.Collections.Generic;
using TeammateOnlineApi.Models;

namespace TeammateOnlineApi.Database.Repositories
{
    public interface IGroupRepository
    {
        Group Add(Group group);

        Group FindById(int id);

        Group FindByName(string name);

        IEnumerable<Group> GetAll();

        void Remove(Group group);

        void Update(Group group);
    }
}
