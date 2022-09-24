using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class OperationResult<T> : IOperationResult<T>
    {
        public T? Result { get; set; }
        public Status Status { get; set; }
        public string Message { get; }

        public OperationResult(T? result, Status status) : this(result, status, "No message.")
        {

        }

        public OperationResult(T? result, Status status, string message)
        {
            Result = result;
            Status = status;
            Message = message;
        }
    }
}
