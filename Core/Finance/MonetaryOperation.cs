using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class MonetaryOperation
    {
        public decimal Amount { get; init; }
        public OperationType OperationType { get; init; }
        public string Id { get; }
        public string UserId { get; init; }
        public string CreationDateTime { get; }

        public MonetaryOperation(decimal amount, OperationType operationType, string userId)
        {
            Amount = amount;
            this.OperationType = operationType;
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            CreationDateTime = DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss");
        }

        public static MonetaryOperation GetDefaultOperation()
        {
            return new MonetaryOperation(0M, OperationType.Default, User.UNKNOW_USERNAME);
        }

        public static bool operator ==(MonetaryOperation op1, MonetaryOperation op2)
        {
            return op1.Amount == op2.Amount &&
                   op1.CreationDateTime == op2.CreationDateTime &&
                   op1.OperationType == op2.OperationType &&
                   op1.Id == op2.Id &&
                   op1.UserId == op2.UserId;
        }

        public static bool operator != (MonetaryOperation op1, MonetaryOperation op2)
        {
            return op1.Amount != op2.Amount ||
                   op1.CreationDateTime != op2.CreationDateTime ||
                   op1.OperationType != op2.OperationType ||
                   op1.Id != op2.Id ||
                   op1.UserId != op2.UserId;
        }

        public override bool Equals(object? obj)
        {
            if (obj is MonetaryOperation op2)
                return this.Amount == op2.Amount &&
                       this.CreationDateTime == op2.CreationDateTime &&
                       this.OperationType == op2.OperationType &&
                       this.Id == op2.Id &&
                       this.UserId == op2.UserId;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
