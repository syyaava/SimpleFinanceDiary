using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IFinanceHandler
    {
        void Add(MonetaryOperation operation);
        void AddRange(IEnumerable<MonetaryOperation> operations);
        MonetaryOperation Get(string id, string userId);
        IEnumerable<MonetaryOperation> GetAll();
        IEnumerable<MonetaryOperation> GetAllByUser(string userId);
        IEnumerable<MonetaryOperation> GetAllByType(string userId, OperationType type);
        void Remove(MonetaryOperation operation);
        void RemoveAll(string userId);
        void RemoveAllByType(string userId, OperationType type);
        void Update(MonetaryOperation oldOperation, MonetaryOperation newOperation);
    }
}
