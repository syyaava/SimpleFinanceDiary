using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiaryConsole
{
    internal class InputIncomeExpensesScreen : Screen<bool>
    {
        readonly User user;
        readonly IFinanceHandler financeHandler;

        public InputIncomeExpensesScreen(IFinanceHandler financeHandler, User user)
        {
            this.financeHandler = financeHandler;
            this.user = user;
        }
        //TODO: Что-то слишком большой метод.
        public override bool Go()
        {
            var flag = true;
            while (flag)
            {
                Console.Clear();
                DisplayMenu();
                int inputNum;
                if (!int.TryParse(Console.ReadLine(), out inputNum))
                {
                    DisplayError("Input error. Check the correctness of the entered number.\nPress \"Enter\" to continue.");
                    continue;
                }
                switch (inputNum)
                {
                    case 1:
                        Console.WriteLine("Input income:");
                        if(decimal.TryParse(Console.ReadLine(), out decimal income))
                        {
                            financeHandler.Add(new MonetaryOperationDTO(income, OperationType.Income, user.Id));
                        }
                        else
                        {
                            DisplayError("Incorrect entered number. Try again (\"Enter\").");
                            continue;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Input expense:");
                        if (decimal.TryParse(Console.ReadLine(), out decimal expense))
                        {
                            financeHandler.Add(new MonetaryOperationDTO(expense, OperationType.Expense, user.Id));
                        }
                        else
                        {
                            DisplayError("Incorrect entered number. Try again (\"Enter\").");
                            continue;
                        }
                        break;
                    case 0:
                        flag = false;
                        break;
                    default:
                        DisplayError("Incorrect number. Try again (\"Enter\").");
                        continue;
                }
            }
            return false;
        }

        private void DisplayMenu()
        {
            Console.WriteLine("==========Input menu==========");
            Console.WriteLine("Enter the appropriate number:");
            Console.WriteLine("1 - Input Income.");
            Console.WriteLine("2 - Input Expenses.");
            Console.WriteLine("0 - Exit.");
            Console.WriteLine();
        }
    }
}
