using Core.Interfaces;

namespace FinanceDiary.Tests
{
    internal class MockUserSource : IUserSource
    {
        public string Name { get; } = "Mock user source.";
        private List<User> users = new List<User>();

        public void AddUser(User user)
        {
            try
            {
                var existingUser = GetUser(user.Id);
                throw new ItemAlreadyExistException("User with this id already exists.");
            }
            catch (ObjectNotFoundException)
            {
                users.Add(user);
            }
        }

        public User GetUser(string userId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            return user is not null ? user : throw new ObjectNotFoundException("User not found.");
        }

        public IEnumerable<User> GetUsers()
        {
            if (users is null)
                throw new ArgumentNullException("Users set is null.");

            return users;
        }

        public void RemoveUser(string userId)
        {
            var userToRemove = GetUser(userId);
            users.Remove(userToRemove);
        }

        public void UpdateUser(User oldUser, User newUser)
        {
            var userToUpdate = GetUser(oldUser.Id);
            users.Remove(userToUpdate);
            users.Add(newUser);
        }
    }
}
