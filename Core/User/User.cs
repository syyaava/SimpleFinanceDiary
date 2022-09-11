using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class User
    {
        public const string UNKNOW_USERNAME = "__UNKNOW__";
        public const string USERNAME_PATTERN = "^([0-9a-zA-Z_]{5,64})$";

        public string Id { get; init; }        

        public User(string id)
        {
            Id = id;
        }

        public static User GetUnknowUser()
        {
            return new User(UNKNOW_USERNAME);
        }

        public UserDTO AsDto()
        {
            return new UserDTO(Id);
        }
    }
}
