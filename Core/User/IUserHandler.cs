using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IUserHandler
    {
        IOperationResult<User> GetUser(string userId);
        IOperationResult<IEnumerable<User>> GetUsers();
        IOperationResult<User> AddUser(string user);
        IOperationResult<User> RemoveUser(string userId);
        IOperationResult<User> UpdateUser(User oldUser, User newUser);
    }
}
