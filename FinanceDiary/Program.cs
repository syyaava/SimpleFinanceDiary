using Core;
using FinanceDiaryConsole;
using Infrastructure;

IUserSource userSource = new SQLiteUserSource();
List<ILogger> loggerList = new List<ILogger>()
{
    new FileLogger(),
};
IUserHandler userHandler = new UserHandler(userSource, loggerList);
IUserValidator userValidator = new UserValidator();

var loginScreen = new LoginScreen(userHandler, userValidator);
var user = loginScreen.Go();
Console.Clear();

