using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class ItemAlreadyExistException : Exception
    {
        public ItemAlreadyExistException() : base("Item already exists.") { }
        public ItemAlreadyExistException(string message) : base(message) { }

    }
}
