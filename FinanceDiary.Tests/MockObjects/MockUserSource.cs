namespace FinanceDiary.Tests
{
    internal class MockUserSource : IUserSource
    {
        public string Name { get; } = "Mock user source.";
        private List<User> users = new List<User>();

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public User GetUser(string userId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            return user is not null ? user : throw new ItemNotFoundException("User not found.");
        }

        public IEnumerable<User> GetUsers()
        {
            return users;
        }

        public void RemoveUser(User user)
        {
            var userToRemove = GetUser(user.Id);
            users.Remove(userToRemove);
        }

        public void UpdateUser(User oldUser, User newUser)
        {
            var userToUpdate = GetUser(oldUser.Id);
            userToUpdate = newUser;
        }
    }
}
