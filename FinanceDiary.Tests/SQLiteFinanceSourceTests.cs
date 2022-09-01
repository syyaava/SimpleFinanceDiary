using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiary.Tests
{
    public class SQLiteFinanceSourceTests
    {
        [Fact]
        public void AddIncome_ValidOperation_NoException()
        {
            var financeSource = CreateFinanceSource();
            var operation = new MonetaryOperation(250.5M, OperationType.Income, "TestUser");

            financeSource.Add(operation);

            Assert.Contains(operation, financeSource.GetAll());
            Assert.Contains(operation, financeSource.GetAllByUser("TestUser"));
            Assert.Contains(operation, financeSource.GetAllByType("TestUser", OperationType.Income));
        }

        [Fact]
        public void AddExpence_ValidOperation_NoException()
        {
            var financeSource = CreateFinanceSource();
            var operation = new MonetaryOperation(150.5M, OperationType.Expense, "TestUser");

            financeSource.Add(operation);

            Assert.Contains(operation, financeSource.GetAll());
            Assert.Contains(operation, financeSource.GetAllByUser("TestUser"));
            Assert.Contains(operation, financeSource.GetAllByType("TestUser", OperationType.Expense));
        }

        [Fact]
        public void AddRange_ValidOperations_NoException()
        {
            var financeSource = CreateFinanceSource();

            List<MonetaryOperation> operations = AddManyOperations(financeSource);

            foreach (var operation in operations)
                Assert.Contains(operation, financeSource.GetAll());
            Assert.Equal(operations.Count(x => x.UserId == "TestUser"), financeSource.GetAll().Count(x => x.UserId == "TestUser"));
            Assert.Equal(operations.Count(x => x.OperationType == OperationType.Income),
                        financeSource.GetAll().Count(x => x.OperationType == OperationType.Income));
        }

        [Fact]
        public void Get_ValidIdAndUserid_ReturnMonetaryOperation()
        {
            var financeSource = CreateFinanceSource();

            List<MonetaryOperation> operations = AddManyOperations(financeSource);

            var operation1 = financeSource.Get(operations[2].Id, operations[2].UserId);
            var operation2 = financeSource.Get(operations[4].Id, operations[4].UserId);

            Assert.Equal(operations[2], operation1);
            Assert.Equal(operations[4], operation2);
        }
        
        [Fact]
        public void Get_NotValidIdAndUserid_ThrowItemNotFoundException()
        {
            var financeSource = CreateFinanceSource();

            List<MonetaryOperation> operations = AddManyOperations(financeSource);

            Assert.Throws<ItemNotFoundException>(() => financeSource.Get(operations[2].Id, "NoUser"));
            Assert.Throws<ItemNotFoundException>(() => financeSource.Get(Guid.NewGuid().ToString(), operations[4].UserId));
        }

        [Fact]
        public void GetAll_NotEmptySet_ReturnOperationSet()
        {
            var financeSource = CreateFinanceSource();

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            var existingOperations = financeSource.GetAll().OrderBy(x => x.Amount).ToList();
            operations = operations.OrderBy(x => x.Amount).ToList();

            Assert.NotEmpty(existingOperations);
            Assert.Equal(operations.Count, existingOperations.Count);
            for(var i = 0; i < operations.Count; i++)
                Assert.Equal(operations[i], existingOperations[i]);
        }

        [Fact]
        public void GetAll_EmptySet_ReturnEmptyOperationSet()
        {
            var financeSource = CreateFinanceSource();

            var existingOperations = financeSource.GetAll();

            Assert.Empty(existingOperations);
        }

        [Fact]
        public void GetAllByUser_ExistingUser_ReturnOperationSet()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser";

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            var userOperations = financeSource.GetAllByUser(userId).OrderBy(x => x.Amount).ToList();
            var existingOperations = operations.Where(x => x.UserId == userId).OrderBy(x => x.Amount).ToList();

            Assert.Equal(operations.Count, userOperations.Count);
            for(var i = 0; i < operations.Count(x => x.UserId == userId); i++)
                Assert.Equal(existingOperations[i], userOperations[i]);
        }

        [Fact]
        public void GetAllByUser_NotExistingUser_ReturnEmptyOperationSet()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser55";

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            var userOperations = financeSource.GetAllByUser(userId).OrderBy(x => x.Amount).ToList();
            var existingOperations = operations.Where(x => x.UserId == userId).OrderBy(x => x.Amount).ToList();

            Assert.Equal(operations.Count, userOperations.Count);
            for(var i = 0; i < operations.Count(x => x.UserId == userId); i++)
                Assert.Equal(existingOperations[i], userOperations[i]);
        }

        [Fact]
        public void GetAllByUser_EmptyOperationSet_ReturnOperationSet()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser55";

            var userOperations = financeSource.GetAllByUser(userId).OrderBy(x => x.Amount).ToList();

            Assert.NotNull(userOperations);
            Assert.Empty(userOperations);
        }

        [Fact]
        public void GetAllByType_ValidUserId_ReturnOperationSet()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser2";

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            var userIncomes = financeSource.GetAllByType(userId, OperationType.Income);
            var userExpenses = financeSource.GetAllByType(userId, OperationType.Expense);

            Assert.Equal(operations.Count(x => x.UserId == userId && x.OperationType == OperationType.Income), userIncomes.Count());
            Assert.Equal(operations.Count(x => x.UserId == userId && x.OperationType == OperationType.Expense), userExpenses.Count());
        }

        [Fact]
        public void GetAllByType_NotExistingUserId_ReturnEmptyOperationSet()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser2222";

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            var userIncomes = financeSource.GetAllByType(userId, OperationType.Income);
            var userExpenses = financeSource.GetAllByType(userId, OperationType.Expense);

            Assert.Empty(userIncomes);
            Assert.Empty(userExpenses);
        }

        [Fact]
        public void Remove_ExistingOperation_NoException()
        {
            var financeSource = CreateFinanceSource();

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            financeSource.Remove(operations[2]);

            Assert.Throws<ItemNotFoundException>(() => financeSource.Get(operations[2].Id, operations[2].UserId));
        }

        [Fact]
        public void Remove_ExistingOperation_ThrowItemNotFoundException()
        {
            var financeSource = CreateFinanceSource();
            var operation = new MonetaryOperation(6546.55M, OperationType.Expense, "user");

            List<MonetaryOperation> operations = AddManyOperations(financeSource);

            Assert.Throws<ItemNotFoundException>(() => financeSource.Remove(operation));
        }

        [Fact]
        public void RemoveAll_ExistingUser_NoException()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser";

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            financeSource.RemoveAll(userId);

            Assert.NotNull(financeSource.GetAllByUser(userId));
            Assert.Empty(financeSource.GetAllByUser(userId));            
        }

        [Fact]
        public void RemoveAll_NotExistingUser_ThrowItemNotFoundException()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser44";

            List<MonetaryOperation> operations = AddManyOperations(financeSource);

            Assert.Throws<ItemNotFoundException>(() => financeSource.RemoveAll(userId));       
        }

        [Fact]
        public void RemoveAllByType_ExistingUser_NoException()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser";

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            financeSource.RemoveAllByType(userId, OperationType.Income);
            var userOperations = financeSource.GetAllByUser(userId);

            foreach (var operation in userOperations)
                Assert.NotEqual(OperationType.Income, operation.OperationType);         
        }

        [Fact]
        public void RemoveAllByType_NotExistingUser_ThrowItenNotFoundException()
        {
            var financeSource = CreateFinanceSource();
            var userId = "TestUser44";

            List<MonetaryOperation> operations = AddManyOperations(financeSource);

            Assert.Throws<ItemNotFoundException>(() => financeSource.RemoveAllByType(userId, OperationType.Income));       
        }

        [Fact]
        public void Update_ExistingOperation_NoException()
        {
            var financeSource = CreateFinanceSource();

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            var operationToUpdate = operations[1];
            var updatedOperation = new MonetaryOperation(888M, OperationType.Expense, operationToUpdate.UserId);
            financeSource.Update(operationToUpdate, updatedOperation);

            Assert.DoesNotContain(operationToUpdate, operations);
            Assert.Contains(updatedOperation, operations);
            Assert.Equal(updatedOperation, financeSource.Get(updatedOperation.Id, updatedOperation.UserId));
        }

        [Fact]
        public void Update_NotExistingOperation_NoException()
        {
            var financeSource = CreateFinanceSource();

            List<MonetaryOperation> operations = AddManyOperations(financeSource);
            var operationToUpdate = new MonetaryOperation(222M, OperationType.Income, "MyUser");
            var updatedOperation = new MonetaryOperation(888M, OperationType.Expense, operationToUpdate.UserId);

            Assert.Throws<ItemNotFoundException>(() => financeSource.Update(operationToUpdate, updatedOperation));
        }

        private static List<MonetaryOperation> AddManyOperations(IFinanceSource financeSource)
        {
            var operations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(250.5M, OperationType.Income, "TestUser"),
                new MonetaryOperation(10050.11M, OperationType.Income, "TestUser"),
                new MonetaryOperation(150.52M, OperationType.Expense, "TestUser"),
                new MonetaryOperation(3250.3M, OperationType.Income, "TestUser"),
                new MonetaryOperation(43M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(20M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(50.5M, OperationType.Expense, "TestUser3"),
                new MonetaryOperation(20.12M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(111M, OperationType.Expense, "TestUser2")
            };

            financeSource.AddRange(operations);
            return operations;
        }

        private IFinanceSource CreateFinanceSource()
        {
            var financeSource = new SQLiteFinanceSource();
            ClearDb(financeSource);
            return financeSource;
        }

        private void ClearDb(SQLiteFinanceSource financeSource)
        {
            var operations = financeSource.GetAll();
            foreach (var operation in operations)
                financeSource.Remove(operation);
        }
    }
}
