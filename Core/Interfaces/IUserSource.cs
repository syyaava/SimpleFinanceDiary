using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserSource
    {
        string Name { get; }
        User GetUser(string userId);
        IEnumerable<User> GetUsers();
        void AddUser(User user);
        void RemoveUser(string user);
        void UpdateUser(User oldUser, User newUser);
    }
}
