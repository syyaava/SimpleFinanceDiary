using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserDTO
    {
        public string Id { get; init; }

        public UserDTO(string id)
        {
            Id = id;
        }

        public User AsUser()
        {
            return new User(Id);
        }
    }
}
