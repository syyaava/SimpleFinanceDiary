using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiaryConsole
{
    internal class HistoryScreen : Screen<bool>
    {
        readonly User user;
        readonly IFinanceHandler financeHandler;
        public HistoryScreen(IFinanceHandler financeHandler, User user)
        {
            this.financeHandler = financeHandler;
            this.user = user;
        }

        public override bool Go()
        {
            var flag = true;
            while (flag)
            {
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
                        var userHistory = financeHandler.GetAllByUser(user.Id);
                        if (userHistory.Status != Status.Ok)
                        {
                            DisplayError("Your history not found.");
                            continue;
                        }
                        Console.WriteLine();
                        foreach(var operation in userHistory.Result)
                        {
                            Console.WriteLine($"{operation.OperationType} - {operation.Amount.ToString("C2")} - {operation.CreationDateTime}");
                        }
                        Console.ReadLine();
                        break;
                    case 0:
                        flag = false;
                        break;
                    default:
                        DisplayError("Incorrect number. Try again.");
                        continue;
                }
                Console.Clear();
            }
            return false;
        }

        private void DisplayMenu()
        {
            Console.WriteLine("==========History menu==========");
            Console.WriteLine("Enter the appropriate number:");
            Console.WriteLine("1 - Get all history");
            Console.WriteLine("0 - Exit.");
            Console.WriteLine();
        }
    }
}
