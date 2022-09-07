﻿using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiaryConsole
{
    internal class MainScreen : Screen<bool>
    {
        readonly User user;
        readonly IFinanceHandler financeHandler;

        public MainScreen(User user, IFinanceHandler financeHandler)
        {
            this.user = user;
            this.financeHandler = financeHandler;
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
                        var inputScreen = new InputIncomeExpensesScreen(financeHandler, user);
                        inputScreen.Go();
                        break;
                    case 2:
                        var historyScreen = new HistoryScreen(financeHandler, user);
                        historyScreen.Go();
                        break;
                    case 0:
                        flag = false;
                        break;
                    default:
                        DisplayError("Incorrect number. Try again.");
                        continue;
                }
            }
            return false;
        }

        private void DisplayMenu()
        {
            Console.WriteLine("==========Main menu==========");
            Console.WriteLine("Enter the appropriate number:");
            Console.WriteLine("1 - Input Income/Expenses.");
            Console.WriteLine("2 - History.");
            Console.WriteLine("0 - Exit.");
            Console.WriteLine();
        }
    }
}
