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
        public void GetUsers_NotEmtyUserList_ReturnOkWithUserList()
        {
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(new User("TestUser1"));
            userHandler.AddUser(new User("TestUser2"));
            userHandler.AddUser(new User("TestUser3"));
            userHandler.AddUser(new User("TestUser4"));
            userHandler.AddUser(new User("TestUser5"));
            userHandler.AddUser(new User("TestUser6"));
            userHandler.AddUser(new User("TestUser7"));
            var result = userHandler.GetUsers();

            Assert.NotNull(result.Result);
            Assert.Equal(7, result.Result?.Count());
            for (var i = 1; i <= 7; i++)
                Assert.Contains($"TestUser{i}", result.Result?.Select(x => x.Id));
        }

        [Fact]
        public void GetUsers_EmptyUserList_ReturnOkWithEmptyUserList()
        {
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.GetUsers();

            Assert.NotNull(result.Result);
            Assert.Empty(result.Result);
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
}
