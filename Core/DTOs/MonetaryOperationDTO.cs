using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class MonetaryOperationDTO
    {
        public decimal Amount { get; set; }
        public OperationType OperationType { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CreationDateTime { get; set; }

        public MonetaryOperationDTO(decimal amount, OperationType operationType, string userId) : this(amount, operationType, userId, "")
        {

        }

        public MonetaryOperationDTO(decimal amount, OperationType operationType, string userId, string id)
        {
            Amount = amount;
            OperationType = operationType;
            UserId = userId;
            Id = id;
            CreationDateTime = DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss");
        }

        public MonetaryOperation AsMonetaryOperation()
        {
            if (string.IsNullOrEmpty(Id))
                return new MonetaryOperation(Amount, OperationType, UserId);
            else
                return new MonetaryOperation(Amount, OperationType, Id, UserId);
        }
    }
}
