using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IUserHandler
    {
        User GetUser(string userId);
        IEnumerable<User> GetUsers();
        void AddUser(User user);
        void RemoveUser(string userId);
        User UpdateUser(User oldUser, User newUser);
    }
}
