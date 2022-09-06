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

        public IOperationResult<User> AddUser(User user)
        {
            try
            {
                userSource.AddUser(user);
                ILogger.Log(loggers, $"User {user.Id} was added.");
                return new OperationResult<User>(user, Status.Ok);
            }
            catch(ItemAlreadyExistException ex)
            {
                ILogger.Log(loggers, $"User with this id already contains. Exception message: {ex.Message}.");
                return new OperationResult<User>(user, Status.Error, "User with this id already contains.");
            }
            catch(Exception ex)
            {
                ILogger.Log(loggers, $"User wasn't added. Unknown exception. Exception message: {ex.Message}.");
                return new OperationResult<User>(user, Status.Error, "User wasn't added. Unknown exception.");
            }
        }

        public IOperationResult<User> GetUser(string userId)
        {
            try
            {
                var user = userSource.GetUser(userId);
                ILogger.Log(loggers, $"User with id {userId} was received.");
                return new OperationResult<User>(user, Status.Ok);
            }
            catch (ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"User with id {userId} not found. Exception message: {ex.Message}.");
                return new OperationResult<User>(User.GetUnknowUser(), Status.Error, $"User with id {userId} not found.");
            }
        }

        public IOperationResult<IEnumerable<User>> GetUsers()
        {
            try
            {
                var users = userSource.GetUsers();
                ILogger.Log(loggers, $"User set was received.");
                return new OperationResult<IEnumerable<User>>(users, Status.Ok);
            }
            catch(ArgumentNullException ex)
            {
                ILogger.Log(loggers, $"User set is null.");
                return new OperationResult<IEnumerable<User>>(new List<User>(), Status.Error, $"User set is null. Exception message: {ex.Message}.");
            }
        }

        public IOperationResult<User> RemoveUser(string userId)
        {
            try
            {
                var user = userSource.GetUser(userId);
                userSource.RemoveUser(userId);
                ILogger.Log(loggers, $"User with id {userId} was removed.");
                return new OperationResult<User>(user, Status.Ok, $"User with id {userId} war removed.");
            }
            catch (ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"User with id {userId} not fount to remove. Exception message: {ex.Message}.");
                return new OperationResult<User>(User.GetUnknowUser(), Status.Error, $"User with id {userId} not found to remove.");
            }
        }

        public IOperationResult<User> UpdateUser(User oldUser, User newUser)
        {
            try
            {
                userSource.UpdateUser(oldUser, newUser);
                ILogger.Log(loggers, $"User with id {oldUser.Id} was updated.");
                return new OperationResult<User>(newUser, Status.Ok, $"Old user {oldUser.Id} was updated to {newUser.Id}.");
            }
            catch(ItemNotFoundException ex)
            {
                ILogger.Log(loggers, $"User with id {oldUser.Id} not found. Exception message: {ex.Message}.");
                return new OperationResult<User>(oldUser, Status.Error, $"User with id {oldUser.Id} not found.");
            }
            catch(ItemAlreadyExistException ex)
            {
                ILogger.Log(loggers, $"User with id {newUser.Id} already contains. Exception message: {ex.Message}.");
                return new OperationResult<User>(newUser, Status.Error, $"User {newUser.Id} already contains.");
            }
        }
    }
}
