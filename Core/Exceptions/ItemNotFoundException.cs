using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class ObjectNotFoundException: Exception
    {
        public ObjectNotFoundException() : base("Object not found.") { }

        public ObjectNotFoundException(string message) : base(message) { }
    }
}
