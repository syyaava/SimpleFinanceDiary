﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class ItemNotFoundException: Exception
    {
        public ItemNotFoundException() : base("Item not found.") { }

        public ItemNotFoundException(string message) : base(message) { }
    }
}
