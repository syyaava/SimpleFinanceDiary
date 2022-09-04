using Core;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SQLiteUserSource : DbContext, IUserSource
    {
        //TODO: Вынести строку подключения в конфигурационный файл.
        const string CONNECTION_STRING = "Data Source=FinanceDiary.db";
        public string Name { get; } = "SQLite Db user source.";
        private DbSet<User> users => Set<User>();

        public SQLiteUserSource()
        {
            Database.EnsureCreated();
        }

        public void AddUser(User user)
        {
            try
            {
                var existingUser = GetUser(user.Id);
                throw new ItemAlreadyExistException("User with this id already exists.");
            }
            catch(ItemNotFoundException)
            {
                users.Add(user);
                SaveChanges();
            }            
        }

        public User GetUser(string userId)
        {
            var user = users.FirstOrDefault(x => x.Id == userId);
            return user is not null ? user : throw new ItemNotFoundException("User not found.");
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
            SaveChanges();
        }

        public void UpdateUser(User oldUser, User newUser)
        {
            var userToUpdate = GetUser(oldUser.Id);
            users.Remove(userToUpdate);
            users.Add(newUser);
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(CONNECTION_STRING);
        }
    }
}
