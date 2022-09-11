using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserHandler : IUserHandler
    {
        IUserSource userSource;
        IEnumerable<ILogger> loggers;

        public UserHandler(IUserSource userSource) : this(userSource, new List<ILogger>()) { }

        public UserHandler(IUserSource userSource, IEnumerable<ILogger> loggers)
        {
            this.userSource = userSource;
            this.loggers = loggers;
        }

        public IOperationResult<UserDTO> AddUser(string userId)
        {
            var newUser = new User(userId);
            try
            {                
                userSource.AddUser(newUser);
                ILogger.Log(loggers, $"User {newUser.Id} was added.");
                return new OperationResult<UserDTO>(newUser.AsDto(), Status.Ok);
            }
            catch(ItemAlreadyExistException ex)
            {
                ILogger.Log(loggers, $"User with this id already contains. Exception message: {ex.Message}.");
                return new OperationResult<UserDTO>(newUser.AsDto(), Status.Error, "User with this id already contains.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"User wasn't added. Unknown exception. Exception message: {ex.Message}.");
                return new OperationResult<UserDTO>(newUser.AsDto(), Status.Error, "User wasn't added. Unknown exception.");
            }
        }

        public IOperationResult<UserDTO> GetUser(string userId)
        {
            try
            {
                var user = userSource.GetUser(userId);
                ILogger.Log(loggers, $"User with id {userId} was received.");
                return new OperationResult<UserDTO>(user.AsDto(), Status.Ok);
            }
            catch (ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"User with id {userId} not found. Exception message: {ex.Message}.");
                return new OperationResult<UserDTO>(User.GetUnknowUser().AsDto(), Status.Error, $"User with id {userId} not found.");
            }
        }

        public IOperationResult<IEnumerable<UserDTO>> GetUsers()
        {
            try
            {
                var users = from user in userSource.GetUsers()
                            select user.AsDto();
                ILogger.Log(loggers, $"User set was received.");
                return new OperationResult<IEnumerable<UserDTO>>(users, Status.Ok);
            }
            catch(ArgumentNullException ex)
            {
                ILogger.Log(loggers, $"User set is null.");
                return new OperationResult<IEnumerable<UserDTO>>(new List<UserDTO>(), Status.Error, $"User set is null. Exception message: {ex.Message}.");
            }
        }

        public IOperationResult<UserDTO> RemoveUser(string userId)
        {
            try
            {
                var user = userSource.GetUser(userId).AsDto();
                userSource.RemoveUser(userId);
                ILogger.Log(loggers, $"User with id {userId} was removed.");
                return new OperationResult<UserDTO>(user, Status.Ok, $"User with id {userId} war removed.");
            }
            catch (ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"User with id {userId} not fount to remove. Exception message: {ex.Message}.");
                return new OperationResult<UserDTO>(User.GetUnknowUser().AsDto(), Status.Error, $"User with id {userId} not found to remove.");
            }
        }

        public IOperationResult<UserDTO> UpdateUser(UserDTO oldUserDTO, UserDTO newUserDTO)
        {
            try
            {
                var oldUser = oldUserDTO.AsUser();
                var newUser = newUserDTO.AsUser();
                userSource.UpdateUser(oldUser, newUser);
                ILogger.Log(loggers, $"User with id {oldUser.Id} was updated.");
                return new OperationResult<UserDTO>(newUser.AsDto(), Status.Ok, $"Old user {oldUser.Id} was updated to {newUser.Id}.");
            }
            catch(ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"User with id {oldUserDTO.Id} not found. Exception message: {ex.Message}.");
                return new OperationResult<UserDTO>(oldUserDTO, Status.Error, $"User with id {oldUserDTO.Id} not found.");
            }
            catch(ItemAlreadyExistException ex)
            {
                ILogger.Log(loggers, $"User with id {newUserDTO.Id} already contains. Exception message: {ex.Message}.");
                return new OperationResult<UserDTO>(newUserDTO, Status.Error, $"User {newUserDTO.Id} already contains.");
            }
        }
    }
}
