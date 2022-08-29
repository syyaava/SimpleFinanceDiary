using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IUserValidator
    {
        public bool ValidateUserIdByRegex(string userId, string pattern);
        public bool ValidateUserIdByCustom(Func<object[], bool> func, params object[] parametrs);
    }
}
