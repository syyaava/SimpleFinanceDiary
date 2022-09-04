using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiary.Tests
{
    public class FinanceHandlerTests
    {
        [Fact]
        public void Add_ValidOperation_NoExceptions()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperation = new MonetaryOperation(785.44M, OperationType.Income, "TestUser");

            financeHandler.Add(monetaryOperation);

            Assert.Contains(monetaryOperation, financeHandler.GetAll());
        }

        [Fact]
        public void Add_OperationWithIdThatAlreadyContains_ThrowItemAlreadyContainsException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperation = new MonetaryOperation(785.44M, OperationType.Income, "TestUser");

            financeHandler.Add(monetaryOperation);

            Assert.Throws<ItemAlreadyExistException>(() => financeHandler.Add(monetaryOperation));
        }

        [Fact]
        public void AddRange_SeveralOperations_NoException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser"),
                new MonetaryOperation(711M, OperationType.Income, "TestUser4"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);

            Assert.Equal(monetaryOperations.Count, financeHandler.GetAll().Count());
            foreach (var monetaryOperation in monetaryOperations)
                Assert.Contains(monetaryOperation, financeHandler.GetAll());
        }


        [Fact]
        public void AddRange_SeveralDuplicateOperations_ThrowItemAlreadyExistException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser"),
                new MonetaryOperation(711M, OperationType.Income, "TestUser4"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            Assert.Throws<ItemAlreadyExistException>(() => financeHandler.AddRange(monetaryOperations));
        }

        [Fact]
        public void AddRange_EmptyOperationsList_NoException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>();

            financeHandler.AddRange(monetaryOperations);

            Assert.Empty(financeHandler.GetAll());
        }

        [Fact]
        public void Get_ValidIdAndUserId_ReturnMonetaryOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78511.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };
            var selectedOperation = monetaryOperations[1];

            var operation = financeHandler.Get(selectedOperation.Id, selectedOperation.UserId);

            Assert.NotNull(operation);
            Assert.Equal(selectedOperation, operation);
        }

        [Fact]
        public void Get_NoExistingOperation_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78511.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            Assert.Throws<ItemNotFoundException>(() => financeHandler.Get(Guid.NewGuid().ToString(), "MyUser"));
        }

        [Fact]
        public void Get_FromEmptyOperationSet_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());

            Assert.Throws<ItemNotFoundException>(() => financeHandler.Get(Guid.NewGuid().ToString(), "MyUser"));
        }

        [Fact]
        public void GetAll_NoEmptySet_ReturnIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78511.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var set = financeHandler.GetAll();

            Assert.NotEmpty(set);
            Assert.Equal(monetaryOperations.Count, set.Count());
        }

        [Fact]
        public void GetAll_EmptyOperationSet_ReturnEmptyIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());

            var set = financeHandler.GetAll();

            Assert.Empty(set);
        }

        [Fact]
        public void GetAllByUser_NotEmptyOperationSetExistingUser_ReturnIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var userOperations = financeHandler.GetAllByUser("TestUser");

            Assert.NotEmpty(userOperations);
            Assert.Equal(monetaryOperations.Count(x => x.UserId == "TestUser"), userOperations.Count());
        }

        [Fact]
        public void GetAllByUser_EmptyOperationSet_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());

            Assert.Throws<ItemNotFoundException>(() => financeHandler.GetAllByUser("TestUser"));
        }

        [Fact]
        public void GetAllByUser_NoExistingOperatin_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);

            Assert.Throws<ItemNotFoundException>(() => financeHandler.GetAllByUser("TestUser111111"));
        }

        [Fact]
        public void GetAllByType_ExistingOperations_ReturnIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var userIncome = financeHandler.GetAllByType("TestUser", OperationType.Income);
            var userExpense = financeHandler.GetAllByType("TestUser", OperationType.Expense);

            Assert.Equal(userIncome.Count(), monetaryOperations.Count(x => x.OperationType == OperationType.Income && x.UserId == "Testuser"));
            Assert.Equal(userExpense.Count(), monetaryOperations.Count(x => x.OperationType == OperationType.Expense && x.UserId == "Testuser"));
        }

        [Fact]
        public void GetAllByType_EmptyOperationSet_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());

            Assert.Throws<ItemNotFoundException>(() => financeHandler.GetAllByType("TestUser", OperationType.Income));
            Assert.Throws<ItemNotFoundException>(() => financeHandler.GetAllByType("TestUser", OperationType.Expense));
        }

        [Fact]
        public void GetAllByType_NoExistingOperation_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);

            Assert.Throws<ItemNotFoundException>(() => financeHandler.GetAllByType("TestUser1111", OperationType.Income));
            Assert.Throws<ItemNotFoundException>(() => financeHandler.GetAllByType("TestUser1111", OperationType.Expense));
        }

        [Fact]
        public void Remove_ExistingOperation_NoException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Expense, "TestUser"),
            };
            var selectedOperation = monetaryOperations[1];

            financeHandler.AddRange(monetaryOperations);
            financeHandler.Remove(selectedOperation);

            Assert.DoesNotContain(selectedOperation, financeHandler.GetAll());
        }

        [Fact]
        public void Remove_NoExistingOperation_ItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(1237M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Expense, "TestUser"),
            };

            financeHandler.AddRange(monetaryOperations);

            Assert.Throws<ItemNotFoundException>(() => financeHandler.Remove(new MonetaryOperation(88888M, OperationType.Expense, "MyUser")));
        }

        [Fact]
        public void RemoveAll_SeveralOperations_NoException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            financeHandler.RemoveAll("TestUser");

            Assert.Empty(financeHandler.GetAllByUser("TestUser"));
        }

        [Fact]
        public void RemoveAll_NoExistingUser_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);

            Assert.Throws<ItemNotFoundException>(() => financeHandler.RemoveAll("TestUser2222"));
        }

        [Fact]
        public void RemoveAllByType_SeveralOperations_NoException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            financeHandler.RemoveAllByType("TestUser", OperationType.Income);

            Assert.Equal(0, financeHandler.GetAll().Count(x => x.OperationType == OperationType.Income && x.UserId == "TestUser"));
        }

        [Fact]
        public void RemoveAllByType_NoExistingTypeOperations_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(52.4M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperation(124544M, OperationType.Income, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);            ;

            Assert.Throws<ItemNotFoundException>(() => financeHandler.RemoveAllByType("TestUser", OperationType.Expense));
        }

        [Fact]
        public void Update_ExistingOperation_NoException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Income, "TestUser"),
            };
            var selectedOperation = monetaryOperations[1];
            var updatedOperation = new MonetaryOperation(6666M, OperationType.Expense, "MyUser");

            financeHandler.AddRange(monetaryOperations);
            financeHandler.Update(selectedOperation, updatedOperation);

            Assert.DoesNotContain(selectedOperation, financeHandler.GetAll());
            Assert.Contains(updatedOperation, financeHandler.GetAll());
        }

        [Fact]
        public void Update_NoExistingOperation_ThrowItemNotFoundException()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperation>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperation(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperation(78115.44M, OperationType.Income, "TestUser"),
            };
            var selectedOperation = new MonetaryOperation(111M, OperationType.Income, "TestUser");
            var updatedOperation = new MonetaryOperation(6666M, OperationType.Expense, "MyUser");

            financeHandler.AddRange(monetaryOperations);

            Assert.Throws<ItemNotFoundException>(() => financeHandler.Update(selectedOperation, updatedOperation));
        }
    }
}
