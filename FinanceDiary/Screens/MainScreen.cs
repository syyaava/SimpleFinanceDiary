using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiaryConsole.Screens
{
    internal class MainScreen : Screen<bool>
    {
        readonly User user;
        public MainScreen(User user)
        {
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
                        break;
                    case 2:
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
            Console.WriteLine("2 - Statistics.");
            Console.WriteLine("0 - Exit.");
            Console.WriteLine();
        }
    }
}
