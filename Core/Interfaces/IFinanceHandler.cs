using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFinanceHandler
    {
        IOperationResult<MonetaryOperationDTO> Add(MonetaryOperationDTO operation);
        IOperationResult<IEnumerable<MonetaryOperationDTO>> AddRange(IEnumerable<MonetaryOperationDTO> operations);
        IOperationResult<MonetaryOperationDTO> Get(string id, string userId);
        IOperationResult<IEnumerable<MonetaryOperationDTO>> GetAll();
        IOperationResult<IEnumerable<MonetaryOperationDTO>> GetAllByUser(string userId);
        IOperationResult<IEnumerable<MonetaryOperationDTO>> GetAllByType(string userId, OperationType type);
        IOperationResult<MonetaryOperationDTO> Remove(MonetaryOperationDTO operation);
        IOperationResult<IEnumerable<MonetaryOperationDTO>> RemoveAll(string userId);
        IOperationResult<IEnumerable<MonetaryOperationDTO>> RemoveAllByType(string userId, OperationType type);
        IOperationResult<MonetaryOperationDTO> Update(MonetaryOperationDTO oldOperation, MonetaryOperationDTO newOperation);
    }
}
