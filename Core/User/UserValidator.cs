using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
    public class UserValidator : IUserValidator
    {
        public bool ValidateUserIdByCustom(Func<object[], bool> func, params object[] parametrs)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUserIdByRegex(string userId, string pattern)
        {
            return Regex.IsMatch(userId, pattern);
        }
    }
}
