using Core.Interfaces;
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
        public void Add_ValidOperation_ReturnOkWithAddedOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperation = new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser");

            var result = financeHandler.Add(monetaryOperation);

            Assert.NotNull(result.Result);
            Assert.Equal(monetaryOperation.Amount, result.Result.Amount);
            Assert.Equal(monetaryOperation.UserId, result.Result.UserId);
            Assert.Equal(monetaryOperation.CreationDateTime, result.Result.CreationDateTime);
            Assert.NotNull(financeHandler.GetAll().Result.FirstOrDefault(x => x.Amount == monetaryOperation.Amount &&
                                                                              x.UserId == monetaryOperation.UserId &&
                                                                              x.CreationDateTime == monetaryOperation.CreationDateTime &&
                                                                              x.OperationType == monetaryOperation.OperationType));
        }

        [Fact]
        public void Add_OperationWithIdThatAlreadyContains_ReturnErrorWithNotAddedOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperation = new MonetaryOperation(785.44M, OperationType.Income, "TestUser").AsDto();

            financeHandler.Add(monetaryOperation);
            var result = financeHandler.Add(monetaryOperation);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(monetaryOperation.Amount, result.Result.Amount);
            Assert.Equal(monetaryOperation.UserId, result.Result.UserId);
            Assert.Equal(monetaryOperation.CreationDateTime, result.Result.CreationDateTime);
        }

        [Fact]
        public void AddRange_SeveralOperations_ReturnOkWithAddedIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(711M, OperationType.Income, "TestUser4"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            var result = financeHandler.AddRange(monetaryOperations);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(monetaryOperations.Count, financeHandler.GetAll().Result?.Count());
            Assert.True(financeHandler.GetAll().Result.All(op => monetaryOperations.FirstOrDefault(x => op.Amount == x.Amount
                                                          && op.UserId == x.UserId && op.CreationDateTime == x.CreationDateTime) != null));
        }


        [Fact]
        public void AddRange_SeveralDuplicateOperations_ReturnErrorWithAddedIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var op = new MonetaryOperation(785.44M, OperationType.Income, "TestUser").AsDto();
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                op,
                op,
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(711M, OperationType.Income, "TestUser4"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            var result = financeHandler.AddRange(monetaryOperations);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            foreach(var operation in monetaryOperations)
                Assert.Contains(operation, result.Result);
        }

        [Fact]
        public void AddRange_EmptyOperationsList_ReturnOkWithEmptyIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>();

            var result = financeHandler.AddRange(monetaryOperations);

            Assert.Empty(financeHandler.GetAll().Result);
            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
        }

        [Fact]
        public void Get_ValidIdAndUserId_ReturnOkWithOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser").AsDto(),
                new MonetaryOperation(78511.44M, OperationType.Income, "TestUser").AsDto(),
                new MonetaryOperation(124544M, OperationType.Expense, "TestUser11").AsDto()
            };
            var selectedOperation = monetaryOperations[1];

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.Get(selectedOperation.Id, selectedOperation.UserId);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(selectedOperation.Amount, result.Result.Amount);
            Assert.Equal(selectedOperation.UserId, result.Result.UserId);
            Assert.Equal(selectedOperation.CreationDateTime, result.Result.CreationDateTime);
        }

        [Fact]
        public void Get_NoExistingOperation_ReturnErrorWithDefaultOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78511.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            var result = financeHandler.Get(Guid.NewGuid().ToString(), "MyUser");

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, result.Result?.UserId);
            Assert.Equal(0, result.Result?.Amount);
        }

        [Fact]
        public void Get_FromEmptyOperationSet_ReturnErrorWithDefaultOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());

            var result = financeHandler.Get(Guid.NewGuid().ToString(), "MyUser");

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, result.Result?.UserId);
            Assert.Equal(0, result.Result?.Amount);
        }

        [Fact]
        public void GetAll_NoEmptySet_ReturnOkWithIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78511.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.GetAll();

            Assert.NotNull(result.Result);
            Assert.NotEmpty(result.Result);
            Assert.Equal(Status.Ok, result.Status);
        }

        [Fact]
        public void GetAll_EmptyOperationSet_ReturnOkWithEmptyIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());

            var result = financeHandler.GetAll();

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Empty(result.Result);
        }

        [Fact]
        public void GetAllByUser_NotEmptyOperationSetExistingUser_ReturnOkWithIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.GetAllByUser("TestUser");

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.NotEmpty(result.Result);
            Assert.Equal(monetaryOperations.Count(x => x.UserId == "TestUser"), result.Result?.Count());
        }

        [Fact]
        public void GetAllByUser_EmptyOperationSet_ReturnErrorWithEmptyIEnumerableOperations()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());

            var result = financeHandler.GetAllByUser("TestUser");

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Empty(result.Result);
        }

        [Fact]
        public void GetAllByUser_NoExistingOperatin_ReturnErrorWithEmptyIEnumerableOperations()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.GetAllByUser("TestUser111111");

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Empty(result.Result);
        }

        [Fact]
        public void GetAllByType_ExistingOperations_ReturnOkWithIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var resultIncome = financeHandler.GetAllByType("TestUser", OperationType.Income);
            var resultExpense = financeHandler.GetAllByType("TestUser", OperationType.Expense);

            Assert.NotNull(resultIncome.Result);
            Assert.Equal(Status.Ok, resultIncome.Status);
            Assert.NotNull(resultExpense.Result);
            Assert.Equal(Status.Ok, resultExpense.Status);
            Assert.Equal(monetaryOperations.Count(x => x.OperationType == OperationType.Income && x.UserId == "TestUser"), resultIncome.Result?.Count());
            Assert.Equal(monetaryOperations.Count(x => x.OperationType == OperationType.Expense && x.UserId == "TestUser"), resultExpense.Result?.Count());
        }

        [Fact]
        public void GetAllByType_EmptyOperationSet_ReturnErrorWithEmptyIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());

            var resultIncome = financeHandler.GetAllByType("TestUser", OperationType.Income);
            var resultExpense = financeHandler.GetAllByType("TestUser", OperationType.Expense);

            Assert.NotNull(resultIncome.Result);
            Assert.Equal(Status.Error, resultIncome.Status);
            Assert.NotNull(resultExpense.Result);
            Assert.Equal(Status.Error, resultExpense.Status);
            Assert.Empty(resultIncome.Result);
            Assert.Empty(resultExpense.Result);
        }

        [Fact]
        public void GetAllByType_NoExistingOperation_ReturnErrorWithEmptyIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var resultIncome = financeHandler.GetAllByType("TestUser1111", OperationType.Income);
            var resultExpense = financeHandler.GetAllByType("TestUser1111", OperationType.Expense);

            Assert.NotNull(resultIncome.Result);
            Assert.Equal(Status.Error, resultIncome.Status);
            Assert.NotNull(resultExpense.Result);
            Assert.Equal(Status.Error, resultExpense.Status);
            Assert.Empty(resultIncome.Result);
            Assert.Empty(resultExpense.Result);
        }

        [Fact]
        public void Remove_ExistingOperation_ReturnOkWithRemovedOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser").AsDto(),
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser").AsDto(),
                new MonetaryOperation(78115.44M, OperationType.Expense, "TestUser").AsDto(),
            };
            var selectedOperation = monetaryOperations[1];

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.Remove(selectedOperation);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.DoesNotContain(selectedOperation, financeHandler.GetAll().Result);
        }

        [Fact]
        public void Remove_NoExistingOperation_ReturnErrorWithNotRemovedOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(1237M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78115.44M, OperationType.Expense, "TestUser"),
            };

            financeHandler.AddRange(monetaryOperations);
            MonetaryOperationDTO operation = new MonetaryOperationDTO(88888M, OperationType.Expense, "MyUser");
            var result = financeHandler.Remove(operation);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(operation, result.Result);
        }

        [Fact]
        public void RemoveAll_AllUserOperations_ReturnOkWithRemovedIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.RemoveAll("TestUser");

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            foreach(var item in result.Result)
                Assert.DoesNotContain(item, financeHandler.GetAll().Result);
        }

        [Fact]
        public void RemoveAll_NoExistingUser_ReturnErrorWithEmptyIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.RemoveAll("TestUser2222");

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Empty(result.Result);
        }

        [Fact]
        public void RemoveAllByType_SeveralOperations_ReturnOkWithRemovedIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78115.44M, OperationType.Expense, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Expense, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(124544M, OperationType.Expense, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.RemoveAllByType("TestUser", OperationType.Income);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(0, financeHandler.GetAll().Result?.Count(x => x.OperationType == OperationType.Income && x.UserId == "TestUser"));
        }

        [Fact]
        public void RemoveAllByType_NoExistingTypeOperations_ReturnErrorWithEmptyIEnumerableOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78115.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(52.4M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(45M, OperationType.Income, "TestUser2"),
                new MonetaryOperationDTO(124544M, OperationType.Income, "TestUser11")
            };

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.RemoveAllByType("TestUser", OperationType.Expense);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Empty(result.Result);
        }

        [Fact]
        public void Update_ExistingOperation_ReturnOkWithUpdatedOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperation(785.44M, OperationType.Income, "TestUser").AsDto(),
                new MonetaryOperation(185.5M, OperationType.Income, "TestUser").AsDto(),
                new MonetaryOperation(78115.44M, OperationType.Income, "TestUser").AsDto(),
            };
            var selectedOperation = monetaryOperations[1];
            var updatedOperation = new MonetaryOperationDTO(6666M, OperationType.Expense, "MyUser");

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.Update(selectedOperation, updatedOperation);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal(updatedOperation.Amount, result.Result.Amount);
            Assert.Equal(updatedOperation.UserId, result.Result.UserId);
            Assert.Equal(updatedOperation.CreationDateTime, result.Result.CreationDateTime);
            Assert.DoesNotContain(selectedOperation, financeHandler.GetAll().Result);
            Assert.NotNull(financeHandler.GetAll().Result.FirstOrDefault(x => x.Amount == updatedOperation.Amount &&
                                                                        x.OperationType == updatedOperation.OperationType &&
                                                                        x.UserId == updatedOperation.UserId));
        }

        [Fact]
        public void Update_NoExistingOperation_ReturnErrorWithDefaultOperation()
        {
            IFinanceHandler financeHandler = new FinanceHandler(new MockFinanceSource());
            var monetaryOperations = new List<MonetaryOperationDTO>()
            {
                new MonetaryOperationDTO(785.44M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(185.5M, OperationType.Income, "TestUser"),
                new MonetaryOperationDTO(78115.44M, OperationType.Income, "TestUser"),
            };
            var selectedOperation = new MonetaryOperationDTO(111M, OperationType.Income, "TestUser");
            var updatedOperation = new MonetaryOperationDTO(6666M, OperationType.Expense, "MyUser");

            financeHandler.AddRange(monetaryOperations);
            var result = financeHandler.Update(selectedOperation, updatedOperation);

            Assert.NotNull(result.Result);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal(User.UNKNOW_USERNAME, result.Result?.UserId);
            Assert.Equal(0, result.Result?.Amount);
        }
    }
}
