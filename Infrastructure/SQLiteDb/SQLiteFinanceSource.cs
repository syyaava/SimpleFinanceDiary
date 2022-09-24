using Core;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SQLiteFinanceSource : DbContext, IFinanceSource
    {        
        //TODO: Вынести строку подключения в конфигурационный файл.
        const string CONNECTION_STRING = "Data Source=FinanceSource.db";
        public string Name { get; } = "SQLite Db finance source";

        private DbSet<MonetaryOperation> operations => Set<MonetaryOperation>();

        public SQLiteFinanceSource()
        {
            Database.EnsureCreated();
        }

        public void Add(MonetaryOperation operation)
        {
            try
            {
                var existingOperation = Get(operation.Id, operation.UserId);
                throw new ItemAlreadyExistException($"Operation with id {operation.Id} already contains.");
            }
            catch(ObjectNotFoundException)
            {
                operations.Add(operation);
                SaveChanges();
            }
        }

        public void AddRange(IEnumerable<MonetaryOperation> mOperations)
        {
            foreach (var operation in mOperations)
                Add(operation);
        }

        public MonetaryOperation Get(string id, string userId)
        {
            var existingOperation = operations.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            return existingOperation is not null ? existingOperation 
                   : throw new ObjectNotFoundException($"Operation with id: {id}, userId: {userId} not found.");
        }

        public IEnumerable<MonetaryOperation> GetAll()
        {
            if (operations is null)
                throw new ArgumentNullException("Operations set is null");
            return new List<MonetaryOperation>(operations); //TODO: не нравится мне это создание нового списка при каждом запросе.
        }

        public IEnumerable<MonetaryOperation> GetAllByUser(string userId)
        {
            return GetOperations(new Func<MonetaryOperation, bool>(x => x.UserId == userId));
        }

        public IEnumerable<MonetaryOperation> GetAllByType(string userId, OperationType type)
        {
            return GetOperations(new Func<MonetaryOperation, bool>(x => x.UserId == userId && x.OperationType == type));
        }

        public void Remove(MonetaryOperation operation)
        {
            var existingOperation = Get(operation.Id, operation.UserId);
            operations.Remove(existingOperation);
            SaveChanges();
        }

        public void RemoveAll(string userId)
        {
            RemoveManyOperations(GetAllByUser(userId));
        }

        public void RemoveAllByType(string userId, OperationType type)
        {
            RemoveManyOperations(GetAllByType(userId, type));
        }

        public void Update(MonetaryOperation oldOperation, MonetaryOperation newOperation)
        {
            Remove(oldOperation);
            Add(newOperation);
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(CONNECTION_STRING);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonetaryOperation>().HasKey(x => x.Id);
        }

        private IEnumerable<MonetaryOperation> GetOperations(Func<MonetaryOperation, bool> func)
        {
            var userOperations = operations.Where(func).ToList();
            return userOperations is not null ? userOperations : throw new ObjectNotFoundException("Operations not found.");
        }

        private void RemoveManyOperations(IEnumerable<MonetaryOperation> operationsToRemove)
        {
            if (operationsToRemove is null || operationsToRemove.Count() == 0)
                throw new ObjectNotFoundException("Operations not found.");
            foreach (var operation in operationsToRemove)
                Remove(operation);
        }
    }
}
