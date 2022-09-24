using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserValidator
    {
        public bool ValidateUserIdByRegex(string userId);
        public bool ValidateUserIdByCustom(Func<object[], bool> func, params object[] parametrs);
    }
}
