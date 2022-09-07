using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiaryConsole
{
    internal class LoginScreen : Screen<User>
    {
        readonly IUserHandler userHandler;
        readonly IUserValidator userValidator;

        public LoginScreen(IUserHandler userHandler, IUserValidator userValidator)
        {
            this.userHandler = userHandler;
            this.userValidator = userValidator;
        }

        public override User Go()
        {
            var flag = true;
            var user = User.GetUnknowUser();
            while (flag)
            {
                DisplayMenu();
                int inputNum;
                if (!int.TryParse(Console.ReadLine(), out inputNum))
                {
                    DisplayError("Input error. Check the correctness of the entered number.\nPress \"Enter\" to continue.");
                    continue;
                }
                switch(inputNum)
                {
                    case 1:
                        user = GetUser((id) => userHandler.GetUser(id), "==========Login menu==========");
                        if (user.Id != User.UNKNOW_USERNAME)
                            return user;
                        else
                            continue;
                    case 2:
                        user = GetUser((id) => userHandler.AddUser(id), "=======Registration menu=======");
                        if (user.Id != User.UNKNOW_USERNAME)
                            return user;
                        else
                            continue;
                    case 0:
                        flag = false;
                        break;
                    default:
                        DisplayError("Incorrect number. Try again.");
                        continue;
                }
            }
            return user;
        }

        private User GetUser(Func<string, IOperationResult<User>> func, string menuName)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(menuName);
                Console.WriteLine("To exit the menu, enter \"/exit\".");
                Console.WriteLine("Enter your username:");

                var username = Console.ReadLine();
                if (username is null)
                {
                    DisplayError("Username cannot be null.");
                    continue;
                }

                if (username == "/exit")
                {
                    Console.Clear();
                    break;
                }

                if (!userValidator.ValidateUserIdByRegex(username))
                {
                    DisplayError("Login not correct.");
                    continue;
                }

                var receivingResult = func.Invoke(username);
                if (receivingResult.Status == Status.Ok && receivingResult is not null)
                    return receivingResult.Result;
                else
                    DisplayError($"User {username} not found.");
            }
            return User.GetUnknowUser();
        }

        private void DisplayMenu()
        {
            Console.WriteLine("==========Login menu==========");
            Console.WriteLine("Enter the appropriate number:");
            Console.WriteLine("1 - Login.");
            Console.WriteLine("2 - Registration.");
            Console.WriteLine("0 - Exit.");
            Console.WriteLine();
        }
    }
}
