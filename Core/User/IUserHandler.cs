using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IUserHandler
    {
        IOperationResult<UserDTO> GetUser(string userId);
        IOperationResult<IEnumerable<UserDTO>> GetUsers();
        IOperationResult<UserDTO> AddUser(string user);
        IOperationResult<UserDTO> RemoveUser(string userId);
        IOperationResult<UserDTO> UpdateUser(UserDTO oldUser, UserDTO newUser);
    }
}
