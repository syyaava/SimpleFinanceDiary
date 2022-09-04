using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class FinanceHandler : IFinanceHandler
    {
        IFinanceSource source;
        IEnumerable<ILogger> loggers;

        public FinanceHandler(IFinanceSource source) : this(source, new List<ILogger>()) { }

        public FinanceHandler(IFinanceSource source, IEnumerable<ILogger> loggers)
        {
            this.source = source;
            this.loggers = loggers;
        }

        public void Add(MonetaryOperation operation)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<MonetaryOperation> operations)
        {
            throw new NotImplementedException();
        }

        public MonetaryOperation Get(string id, string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MonetaryOperation> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MonetaryOperation> GetAllByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MonetaryOperation> GetAllByType(string userId, OperationType type)
        {
            throw new NotImplementedException();
        }

        public void Remove(MonetaryOperation operation)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(string userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllByType(string userId, OperationType type)
        {
            throw new NotImplementedException();
        }

        public void Update(MonetaryOperation oldOperation, MonetaryOperation newOperation)
        {
            throw new NotImplementedException();
        }
    }
}
