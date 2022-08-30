using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiary.Tests
{
    public class UserHandlerTests
    {
        [Fact]
        public void AddUser_ValidUser_ReturnOkWithAddedUser()
        {
            var user = new User("TestUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.AddUser(user);

            Assert.Equal(Status.Ok, result.Status);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void AddUser_ExistingUser_ReturnErrorWithUnknowUser()
        {
            var user = new User("TestUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(new User("TestUser"));
            var result = userHandler.AddUser(user);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, result.Result?.Id);
        }

        [Fact]
        public void GetUser_ExistingUser_ReturnOkWithUser()
        {
            var user = new User("TestUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(user);
            var result = userHandler.GetUser(user.Id);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(user.Id, result.Result?.Id);
        }

        [Fact]
        public void GetUser_NotExistingUser_ReturnErrorWithUnknowUser()
        {
            var user = new User("TestUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.GetUser(user.Id);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, result.Result?.Id);
        }

        [Fact]
        public void AddUser_ValidNotExistingUser_ReturnOkWithUser()
        {
            var userToAdd = new User("TestUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.AddUser(userToAdd);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(userToAdd.Id, result.Result?.Id);
        }

        [Fact]
        public void AddUser_ValidExistingUser_ReturnErrorWithUnknowUser()
        {
            var userToAdd = new User("TestUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(new User("TestUser"));
            var result = userHandler.AddUser(userToAdd);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, result.Result?.Id);
        }

        [Fact]
        public void RemoveUser_ExistingUser_ReturnOkWithUser()
        {
            var userToRemove = new User("TestUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(userToRemove);
            var result = userHandler.RemoveUser(userToRemove.Id);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, userHandler.GetUser(userToRemove.Id).Result?.Id);
        }

        [Fact]
        public void RemoveUser_NotExistingUser_ReturnErrorWithUnknowUser()
        {
            var userToRemove = new User("TestUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.RemoveUser(userToRemove.Id);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, result.Result?.Id);
        }

        [Fact]
        public void UpdateUser_ExistingUser_ReturnOkWithNewUser()
        {
            var userToUpdate = new User("UserToUpdate");
            var updatedUser = new User("UpdatedUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(userToUpdate);
            var result = userHandler.UpdateUser(userToUpdate, updatedUser);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(updatedUser.Id, result.Result?.Id);
        }

        [Fact]
        public void UpdateUser_NotExistingUser_ReturnErrorWithDefaultUser()
        {
            var userToUpdate = new User("UserToUpdate");
            var updatedUser = new User("UpdatedUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.UpdateUser(userToUpdate, updatedUser);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, result.Result?.Id);
        }
    }

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
