using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiary.Tests
{
    public class SQLiteUserSourceTests
    {
        [Fact]
        public void AddUser_ValidUser_NoExceptions()
        {
            IUserSource userSource = CreateUserSource();
            var userToAdd = new User("TestUser");

            userSource.AddUser(userToAdd);

            Assert.Contains(userToAdd, userSource.GetUsers());
        }

        [Fact]
        public void AddUser_ExistingUser_ThrowItemAlreadyExistingException()
        {
            IUserSource userSource = CreateUserSource();
            var userToAdd = new User("TestUser");

            userSource.AddUser(new User("TestUser"));

            Assert.Throws<ItemAlreadyExistException>(() => userSource.AddUser(userToAdd));
        }

        [Fact]
        public void GetUser_ExistingUser_ReturnUser()
        {
            IUserSource userSource = CreateUserSource();
            var userToAdd = new User("GetUser");

            userSource.AddUser(userToAdd);

            var existingUser = userSource.GetUser(userToAdd.Id);

            Assert.NotNull(existingUser);
            Assert.Equal(userToAdd.Id, existingUser.Id);
        }

        [Fact]
        public void GetUser_NotExistingUser_ThrowItemNotFoundException()
        {
            IUserSource userSource = CreateUserSource();
            var userToAdd = new User("GetUser");

            userSource.AddUser(userToAdd);

            Assert.Throws<ItemNotFoundException>(() => userSource.GetUser("GetUser2"));
        }

        [Fact]
        public void GetUsers_NotEmptyUserList_ReturnIEnumerableUsers()
        {
            IUserSource userSource = CreateUserSource();

            userSource.AddUser(new User("Test1"));
            userSource.AddUser(new User("Test2"));
            userSource.AddUser(new User("Test3"));
            userSource.AddUser(new User("Test4"));
            userSource.AddUser(new User("Test5"));
            userSource.AddUser(new User("Test6"));
            userSource.AddUser(new User("Test7"));
            var users = userSource.GetUsers();

            Assert.NotNull(users);
            Assert.NotEmpty(users);
        }

        [Fact]
        public void GetUsers_EmptyUserList_ReturnIEnumerableUsers()
        {
            IUserSource userSource = CreateUserSource();

            var users = userSource.GetUsers();

            Assert.NotNull(users);
            Assert.Empty(users);
        }

        [Fact]
        public void RemoveUser_NorExistingUser_ThrowItemNotFoundException()
        {
            IUserSource userSource = CreateUserSource();
            var userToRemove = new User("RemoveUser");

            userSource.AddUser(userToRemove);
            userSource.RemoveUser(userToRemove);

            Assert.Throws<ItemNotFoundException>(() => userSource.GetUser(userToRemove.Id));
        }

        [Fact]
        public void RemoveUser_ExistingUser_ThrowItemNotFoundException()
        {
            IUserSource userSource = CreateUserSource();
            var userToRemove = new User("RemoveUser");

            Assert.Throws<ItemNotFoundException>(() => userSource.RemoveUser(userToRemove));
        }

        [Fact]
        public void UpdateUser_ExistingUser_ThrowItemNotFoundException()
        {
            IUserSource userSource = CreateUserSource();
            var userToUpdate = new User("UserToUpdate");
            var updatedUser = new User("UpdatedUser");

            userSource.AddUser(userToUpdate);
            userSource.UpdateUser(userToUpdate, updatedUser);

            Assert.Throws<ItemNotFoundException>(() => userSource.GetUser(userToUpdate.Id));
            Assert.True(userSource.GetUsers().Count() == 1);
        }

        [Fact]
        public void UpdateUser_NotExistingUser_ThrowItemNotFoundException()
        {
            IUserSource userSource = CreateUserSource();
            var userToUpdate = new User("UserToUpdate");
            var updatedUser = new User("UpdatedUser");

            Assert.Throws<ItemNotFoundException>(() => userSource.UpdateUser(userToUpdate, updatedUser));
        }

        private IUserSource CreateUserSource()
        {
            var userSource = new SQLiteUserSource();
            ClearDb(userSource);
            return userSource;
        }

        private void ClearDb(SQLiteUserSource userSource)
        {
            var users = userSource.GetUsers();
            foreach(var user in users)
                userSource.RemoveUser(user);
        }
    }
}
