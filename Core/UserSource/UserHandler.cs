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
            throw new NotImplementedException();
        }

        public IOperationResult<User> GetUser(string userId)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<IEnumerable<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public IOperationResult<User> RemoveUser(string userId)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<User> UpdateUser(User oldUser, User newUser)
        {
            throw new NotImplementedException();
        }

        private void Log(string message)
        {
            foreach (var logger in loggers)
                logger.Write(message);
        }
    }
}
