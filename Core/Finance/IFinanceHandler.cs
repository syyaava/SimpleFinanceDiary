using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IFinanceHandler
    {
        IOperationResult<MonetaryOperation> Add(MonetaryOperation operation);
        IOperationResult<IEnumerable<MonetaryOperation>> AddRange(IEnumerable<MonetaryOperation> operations);
        IOperationResult<MonetaryOperation> Get(string id, string userId);
        IOperationResult<IEnumerable<MonetaryOperation>> GetAll();
        IOperationResult<IEnumerable<MonetaryOperation>> GetAllByUser(string userId);
        IOperationResult<IEnumerable<MonetaryOperation>> GetAllByType(string userId, OperationType type);
        IOperationResult<MonetaryOperation> Remove(MonetaryOperation operation);
        IOperationResult<IEnumerable<MonetaryOperation>> RemoveAll(string userId);
        IOperationResult<IEnumerable<MonetaryOperation>> RemoveAllByType(string userId, OperationType type);
        IOperationResult<MonetaryOperation> Update(MonetaryOperation oldOperation, MonetaryOperation newOperation);
    }
}
