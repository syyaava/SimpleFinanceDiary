using Core.Exceptions;
using Core.Interfaces;
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

        public IOperationResult<MonetaryOperationDTO> Add(MonetaryOperationDTO operationDTO)
        {
            try
            {
                var operation = operationDTO.AsMonetaryOperation();
                source.Add(operation);
                ILogger.Log(loggers, $"Operation {operation.Id} for user {operation.UserId} was added.");
                return new OperationResult<MonetaryOperationDTO>(operation.AsDto(), Status.Ok);
            }
            catch(ItemAlreadyExistException ex)
            {
                ILogger.Log(loggers, $"Operation with this id {operationDTO.Id} already exists. Exception message: {ex.Message}");
                return new OperationResult<MonetaryOperationDTO>(operationDTO, Status.Error, $"Operation with this id {operationDTO.Id} already exists.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Operation was't added. Unknown exception. Exception message: {ex.Message}");
                return new OperationResult<MonetaryOperationDTO>(operationDTO, Status.Error, "Operation was't added. Unknown exception.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperationDTO>> AddRange(IEnumerable<MonetaryOperationDTO> operations)
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
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(operations, Status.Ok, "All operations were added successfully");                
            else
            {
                int delteOp = operationCount - addedOperationCount;
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(operations, Status.Error, $"{delteOp} were not added.");
            }
        }

        public IOperationResult<MonetaryOperationDTO> Get(string id, string userId)
        {
            try
            {
                var operation = source.Get(id, userId);
                ILogger.Log(loggers, $"Operation with id {id} was received.");
                return new OperationResult<MonetaryOperationDTO>(operation.AsDto(), Status.Ok);
            }
            catch(ObjectNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operation with id {id} not found. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperationDTO>(MonetaryOperation.GetDefaultOperation().AsDto(), Status.Error, 
                                                                $"Operation with id {id} not found.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Operation not found. Unknown exception. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperationDTO>(MonetaryOperation.GetDefaultOperation().AsDto(), Status.Error, 
                                                                "Operation not found. Unknown exception.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperationDTO>> GetAll()
        {
            try
            {
                var operations = from operation in source.GetAll()
                                 select operation.AsDto();
                ILogger.Log(loggers, $"All operations was received.");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(operations, Status.Ok);
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Operations not received. Unknown exception. Exception message: {ex.Message}.");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(new List<MonetaryOperationDTO>(), Status.Error, 
                                                                            "Operations not received. Unknown exception.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperationDTO>> GetAllByType(string userId, OperationType type)
        {
            try
            {
                var operations = from operation in source.GetAllByType(userId, type)
                                 select operation.AsDto();
                ILogger.Log(loggers, $"All operation of the type {type} were received.");
                if(operations.Count() > 0)
                    return new OperationResult<IEnumerable<MonetaryOperationDTO>>(operations, Status.Ok);
                else
                    return new OperationResult<IEnumerable<MonetaryOperationDTO>>(operations, Status.Error, $"Operations for user {userId} not found.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when receiving all operations for the user {userId}. Exception message: {ex.Message}.");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(new List<MonetaryOperationDTO>(), Status.Error,
                                                                            $"Unknown exception when receiving all operations for the user {userId}.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperationDTO>> GetAllByUser(string userId)
        {
            try
            {
                var operations = from operation in source.GetAllByUser(userId)
                                 select operation.AsDto();
                ILogger.Log(loggers, $"All operations for user {userId} was received.");
                if (operations.Count() > 0)
                    return new OperationResult<IEnumerable<MonetaryOperationDTO>>(operations, Status.Ok);
                else
                    return new OperationResult<IEnumerable<MonetaryOperationDTO>>(operations, Status.Error, $"Operations for user {userId} not found.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when receiving all operations for the user. Exception message: {ex.Message}.");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(new List<MonetaryOperationDTO>(), Status.Error, 
                                                                             "Unknown exception when receiving all operations for the user.");
            }
        }

        public IOperationResult<MonetaryOperationDTO> Remove(MonetaryOperationDTO operationDTO)
        {
            try
            {
                var operation = operationDTO.AsMonetaryOperation();
                source.Remove(operation);
                ILogger.Log(loggers, $"Operation {operation.Id} was removed successfully.");
                return new OperationResult<MonetaryOperationDTO>(operation.AsDto(), Status.Ok);
            }
            catch(ObjectNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operation {operationDTO.Id} not found for remove. Exception message: {ex.Message}");
                return new OperationResult<MonetaryOperationDTO>(operationDTO, Status.Error, $"Operation {operationDTO.Id} not found for remove.");
            }
            catch (Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when removing. Operation id: {operationDTO.Id}. Exception message: {ex.Message}");
                return new OperationResult<MonetaryOperationDTO>(operationDTO, Status.Error, 
                                                                $"Unknown exception when removing. Operation id: {operationDTO.Id}.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperationDTO>> RemoveAll(string userId)
        {
            try
            {
                var userOperations = GetAllByUser(userId).Result;
                source.RemoveAll(userId);
                ILogger.Log(loggers, $"All operations for user {userId} was removed.");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(userOperations, Status.Ok);
            }
            catch(ObjectNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operations for user {userId} not found. Exception message: {ex.Message}");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(new List<MonetaryOperationDTO>(), Status.Error, 
                                                                             $"Operations for user {userId} not found.");
            }
            catch (Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when remove operations for user {userId}. Exception message: {ex.Message}");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(new List<MonetaryOperationDTO>(), Status.Error,
                                                                             "Unknown exception when received operations for user {userId}.");
            }
        }

        public IOperationResult<IEnumerable<MonetaryOperationDTO>> RemoveAllByType(string userId, OperationType type)
        {
            try
            {
                var userOperations = GetAllByType(userId, type).Result;
                source.RemoveAllByType(userId, type);
                ILogger.Log(loggers, $"All operations ({type}) for user {userId} was removed.");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(userOperations, Status.Ok);
            }
            catch(ObjectNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operations ({type}) for user {userId} not found.");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(new List<MonetaryOperationDTO>(), Status.Error,
                                                                            $"Operations ({type}) for user {userId} not found.");
            }
            catch (Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when remove operations ({type}) for user {userId}. Exception message: {ex.Message}");
                return new OperationResult<IEnumerable<MonetaryOperationDTO>>(new List<MonetaryOperationDTO>(), Status.Error,
                                                                            $"Unknown exception when received operations ({type}) for user {userId}.");
            }
        }

        public IOperationResult<MonetaryOperationDTO> Update(MonetaryOperationDTO oldOperationDTO, MonetaryOperationDTO newOperationDTO)
        {
            try
            {
                var oldOperation = oldOperationDTO.AsMonetaryOperation();
                var newOperation = newOperationDTO.AsMonetaryOperation();
                source.Update(oldOperation, newOperation);
                ILogger.Log(loggers, $"Operation {oldOperation.Id} was updated to {newOperation.Id}.");
                return new OperationResult<MonetaryOperationDTO>(newOperation.AsDto(), Status.Ok);
            }
            catch(ObjectNotFoundException ex)
            {
                ILogger.Log(loggers, $"Operation {oldOperationDTO.Id} not found for update. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperationDTO>(MonetaryOperation.GetDefaultOperation().AsDto(), Status.Error,
                                                                 $"Operation {oldOperationDTO.Id} not found for update.");
            }
            catch(ItemAlreadyExistException ex)
            {
                ILogger.Log(loggers, $"Operation with id {newOperationDTO.Id} already exists. Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperationDTO>(MonetaryOperation.GetDefaultOperation().AsDto(), Status.Error,
                                                                 $"Operation with id {newOperationDTO.Id} already exists.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"Unknown exception when update {oldOperationDTO.Id} operation to {newOperationDTO.Id}. " +
                            $"Exception message: {ex.Message}.");
                return new OperationResult<MonetaryOperationDTO>(MonetaryOperation.GetDefaultOperation().AsDto(), Status.Error,
                                                                 $"Unknown exception when update {oldOperationDTO.Id} operation to {newOperationDTO.Id}.");
            }
        }
    }
}
