using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiary.Tests
{
    public class MockFinanceSource : IFinanceSource
    {
        public string Name => "Mock finance source.";
        List<MonetaryOperation> operations = new List<MonetaryOperation>();

        public void Add(MonetaryOperation operation)
        {
            try
            {
                var existingOperation = Get(operation.Id, operation.UserId);
                throw new ItemAlreadyExistException($"Operation with id {operation.Id} already contains.");
            }
            catch (ItemNotFoundException)
            {
                operations.Add(operation);
            }
        }

        public void AddRange(IEnumerable<MonetaryOperation> mOperations)
        {
            foreach (var operation in mOperations)
                Add(operation);
        }

        public MonetaryOperation Get(string id, string userId)
        {
            var existingOperation = operations.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            return existingOperation is not null ? existingOperation
                   : throw new ItemNotFoundException($"Operation with id: {id}, userId: {userId} not found.");
        }

        public IEnumerable<MonetaryOperation> GetAll()
        {
            if (operations is null)
                throw new ArgumentNullException("Operations set is null");
            return new List<MonetaryOperation>(operations);
        }

        public IEnumerable<MonetaryOperation> GetAllByUser(string userId)
        {
            return GetOperations(new Func<MonetaryOperation, bool>(x => x.UserId == userId));
        }

        public IEnumerable<MonetaryOperation> GetAllByType(string userId, OperationType type)
        {
            return GetOperations(new Func<MonetaryOperation, bool>(x => x.UserId == userId && x.OperationType == type));
        }

        public void Remove(MonetaryOperation operation)
        {
            var existingOperation = Get(operation.Id, operation.UserId);
            operations.Remove(existingOperation);
        }

        public void RemoveAll(string userId)
        {
            RemoveManyOperations(GetAllByUser(userId));
        }

        public void RemoveAllByType(string userId, OperationType type)
        {
            RemoveManyOperations(GetAllByType(userId, type));
        }

        public void Update(MonetaryOperation oldOperation, MonetaryOperation newOperation)
        {
            Remove(oldOperation);
            Add(newOperation);
        }

        private IEnumerable<MonetaryOperation> GetOperations(Func<MonetaryOperation, bool> func)
        {
            var userOperations = operations.Where(func).ToList();
            return userOperations is not null ? userOperations : throw new ItemNotFoundException("Operations not found.");
        }

        private void RemoveManyOperations(IEnumerable<MonetaryOperation> operationsToRemove)
        {
            if (operationsToRemove is null || operationsToRemove.Count() == 0)
                throw new ItemNotFoundException("Operations not found.");
            foreach (var operation in operationsToRemove)
                Remove(operation);
        }
    }
}
