using Core.Exceptions;
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

        public IOperationResult<MonetaryOperation> Add(MonetaryOperation operation)
        {
            try
            {
                source.Add(operation);
                ILogger.Log(loggers, $"Operation {operation.Id} for user {operation.UserId} was added.");
                return new OperationResult<MonetaryOperation>(operation, Status.Ok);
            }
            catch(ItemAlreadyExistException ex)
            {
                ILogger.Log(loggers, $"Operation with this id {operation.Id} already exists. Exception message: {ex.Message}");
                return new OperationResult<MonetaryOperation>(operation, Status.Error, $"Operation with this id {operation.Id} already exists.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Operation was't added. Unknown exception. Exception message: {ex.Message}");
                return new OperationResult<MonetaryOperation>(operation, Status.Error, "Operation was't added. Unknown exception.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperation>> AddRange(IEnumerable<MonetaryOperation> operations)
        {
            var addedOperationCount = 0;
            var operationCount = operations.Count();
            foreach(var operation in operations)
            {
                if (Add(operation).Status == Status.Ok)
                    addedOperationCount++;
            }

            ILogger.Log(loggers, $"{addedOperationCount} out of {operationCount} were added.");

            if (operationCount == addedOperationCount)            
                return new OperationResult<IEnumerable<MonetaryOperation>>(operations, Status.Ok, "All operations were added successfully");                
            else
            {
                int delteOp = operationCount - addedOperationCount;
                return new OperationResult<IEnumerable<MonetaryOperation>>(operations, Status.Error, $"{delteOp} were not added.");
            }
        }

        public IOperationResult<MonetaryOperation> Get(string id, string userId)
        {
            try
            {
                var operation = source.Get(id, userId);
                ILogger.Log(loggers, $"Operation with id {id} was received.");
                return new OperationResult<MonetaryOperation>(operation, Status.Ok);
            }
            catch(ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operation with id {id} not found. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperation>(MonetaryOperation.GetDefaultOperation(), Status.Error, 
                                                                $"Operation with id {id} not found.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Operation not found. Unknown exception. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperation>(MonetaryOperation.GetDefaultOperation(), Status.Error, 
                                                                "Operation not found. Unknown exception.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperation>> GetAll()
        {
            try
            {
                var operations = source.GetAll();
                ILogger.Log(loggers, $"All operations was received.");
                return new OperationResult<IEnumerable<MonetaryOperation>>(operations, Status.Ok);
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Operations not received. Unknown exception. Exception message: {ex.Message}.");
                return new OperationResult<IEnumerable<MonetaryOperation>>(new List<MonetaryOperation>(), Status.Error, 
                                                                            "Operations not received. Unknown exception.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperation>> GetAllByType(string userId, OperationType type)
        {
            try
            {
                var operations = source.GetAllByType(userId, type);
                ILogger.Log(loggers, $"All operation of the type {type} were received.");
                if(operations.Count() > 0)
                    return new OperationResult<IEnumerable<MonetaryOperation>>(operations, Status.Ok);
                else
                    return new OperationResult<IEnumerable<MonetaryOperation>>(operations, Status.Error, $"Operations for user {userId} not found.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when receiving all operations for the user {userId}. Exception message: {ex.Message}.");
                return new OperationResult<IEnumerable<MonetaryOperation>>(new List<MonetaryOperation>(), Status.Error,
                                                                            $"Unknown exception when receiving all operations for the user {userId}.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperation>> GetAllByUser(string userId)
        {
            try
            {
                var operations = source.GetAllByUser(userId);
                ILogger.Log(loggers, $"All operations for user {userId} was received.");
                if (operations.Count() > 0)
                    return new OperationResult<IEnumerable<MonetaryOperation>>(operations, Status.Ok);
                else
                    return new OperationResult<IEnumerable<MonetaryOperation>>(operations, Status.Error, $"Operations for user {userId} not found.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when receiving all operations for the user. Exception message: {ex.Message}.");
                return new OperationResult<IEnumerable<MonetaryOperation>>(new List<MonetaryOperation>(), Status.Error, 
                                                                            "Unknown exception when receiving all operations for the user.");
            }
        }

        public IOperationResult<MonetaryOperation> Remove(MonetaryOperation operation)
        {
            try
            {
                source.Remove(operation);
                ILogger.Log(loggers, $"Operation {operation.Id} was removed successfully.");
                return new OperationResult<MonetaryOperation>(operation, Status.Ok);
            }
            catch(ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operation {operation.Id} not found for remove. Exception message: {ex.Message}");
                return new OperationResult<MonetaryOperation>(operation, Status.Error, $"Operation {operation.Id} not found for remove.");
            }
            catch (Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when removing. Operation id: {operation.Id}. Exception message: {ex.Message}");
                return new OperationResult<MonetaryOperation>(operation, Status.Error, $"Unknown exception when removing. Operation id: {operation.Id}.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperation>> RemoveAll(string userId)
        {
            try
            {
                var userOperations = GetAllByUser(userId).Result;
                source.RemoveAll(userId);
                ILogger.Log(loggers, $"All operations for user {userId} was removed.");
                return new OperationResult<IEnumerable<MonetaryOperation>>(userOperations, Status.Ok);
            }
            catch(ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operations for user {userId} not found.");
                return new OperationResult<IEnumerable<MonetaryOperation>>(new List<MonetaryOperation>(), Status.Error, 
                                                                            $"Operations for user {userId} not found.");
            }
            catch (Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when remove operations for user {userId}. Exception message: {ex.Message}");
                return new OperationResult<IEnumerable<MonetaryOperation>>(new List<MonetaryOperation>(), Status.Error,
                                                                            "Unknown exception when received operations for user {userId}.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperation>> RemoveAllByType(string userId, OperationType type)
        {
            try
            {
                var userOperations = GetAllByType(userId, type).Result;
                source.RemoveAllByType(userId, type);
                ILogger.Log(loggers, $"All operations ({type}) for user {userId} was removed.");
                return new OperationResult<IEnumerable<MonetaryOperation>>(userOperations, Status.Ok);
            }
            catch(ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operations ({type}) for user {userId} not found.");
                return new OperationResult<IEnumerable<MonetaryOperation>>(new List<MonetaryOperation>(), Status.Error,
                                                                            $"Operations ({type}) for user {userId} not found.");
            }
            catch (Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when remove operations ({type}) for user {userId}. Exception message: {ex.Message}");
                return new OperationResult<IEnumerable<MonetaryOperation>>(new List<MonetaryOperation>(), Status.Error,
                                                                            $"Unknown exception when received operations ({type}) for user {userId}.");
            }
        }

        public IOperationResult<MonetaryOperation> Update(MonetaryOperation oldOperation, MonetaryOperation newOperation)
        {
            try
            {
                source.Update(oldOperation, newOperation);
                ILogger.Log(loggers, $"Operation {oldOperation.Id} was updated to {newOperation.Id}.");
                return new OperationResult<MonetaryOperation>(newOperation, Status.Ok);
            }
            catch(ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operation {oldOperation.Id} not found for update. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperation>(MonetaryOperation.GetDefaultOperation(), Status.Error,
                                                                 $"Operation {oldOperation.Id} not found for update.");
            }
            catch(ItemAlreadyExistException ex)
            {
                ILogger.Log(loggers, $"Operation with id {newOperation.Id} already exists. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperation>(MonetaryOperation.GetDefaultOperation(), Status.Error,
                                                                 $"Operation with id {newOperation.Id} already exists.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when update {oldOperation.Id} operation to {newOperation.Id}. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperation>(MonetaryOperation.GetDefaultOperation(), Status.Error,
                                                                 $"Unknown exception when update {oldOperation.Id} operation to {newOperation.Id}.");
            }
        }
    }
}
