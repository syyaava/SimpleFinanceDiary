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
            var userToAddId = "TestUser";
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(userToAddId);
            var result = userHandler.GetUser(userToAddId);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(userToAddId, result.Result?.Id);
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
        public void GetUsers_NotEmtyUserList_ReturnOkWithIEnumerableUser()
        {
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser("TestUser1");
            userHandler.AddUser("TestUser2");
            userHandler.AddUser("TestUser3");
            userHandler.AddUser("TestUser4");
            userHandler.AddUser("TestUser5");
            userHandler.AddUser("TestUser6");
            userHandler.AddUser("TestUser7");
            var result = userHandler.GetUsers();

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(7, result.Result?.Count());
            for (var i = 1; i <= 7; i++)
                Assert.Contains($"TestUser{i}", result.Result?.Select(x => x.Id));
        }

        [Fact]
        public void GetUsers_EmptyUserList_ReturnOkWithEmptyIEnumerableUser()
        {
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.GetUsers();

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Empty(result.Result);
        }

        [Fact]
        public void AddUser_ValidNotExistingUser_ReturnOkWithAddedUser()
        {
            var userToAddId = "TestUser";
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.AddUser(userToAddId);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(userToAddId, result.Result?.Id);
        }

        [Fact]
        public void AddUser_ValidExistingUser_ReturnErrorWithNotAddedUser()
        {
            var userToAddId = "TestUser";
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(userToAddId);
            var result = userHandler.AddUser(userToAddId);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(userToAddId, result.Result?.Id);
        }

        [Fact]
        public void RemoveUser_ExistingUser_ReturnOkWithRemovedUser()
        {
            var userToRemoveId = "TestUser";
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            userHandler.AddUser(userToRemoveId);
            var result = userHandler.RemoveUser(userToRemoveId);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, userHandler.GetUser(userToRemoveId).Result?.Id);
        }

        [Fact]
        public void RemoveUser_NotExistingUser_ReturnErrorWithNotRemovedUser()
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
            var userToUpdateId = "UserToUpdate";
            var updatedUser = new UserDTO("UpdatedUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var userToUpdate = userHandler.AddUser(userToUpdateId).Result;
            var result = userHandler.UpdateUser(userToUpdate, updatedUser);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(updatedUser.Id, result.Result?.Id);
        }

        [Fact]
        public void UpdateUser_NotExistingUser_ReturnErrorWithNotUpdatedUser()
        {
            var userToUpdate = new UserDTO("UserToUpdate");
            var updatedUser = new UserDTO("UpdatedUser");
            IUserHandler userHandler = new UserHandler(new MockUserSource());

            var result = userHandler.UpdateUser(userToUpdate, updatedUser);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(userToUpdate.Id, result.Result?.Id);
        }
    }
}
