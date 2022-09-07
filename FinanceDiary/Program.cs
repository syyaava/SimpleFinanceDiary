using Core;
using FinanceDiaryConsole;
using Infrastructure;

IUserSource userSource = new SQLiteUserSource();
IFinanceSource financeSource = new SQLiteFinanceSource();
List<ILogger> loggerList = new List<ILogger>()
{
    new FileLogger(),
};
IUserHandler userHandler = new UserHandler(userSource, loggerList);
IUserValidator userValidator = new UserValidator();
IFinanceHandler financeHandler = new FinanceHandler(financeSource);

var loginScreen = new LoginScreen(userHandler, userValidator);
var user = loginScreen.Go();
Console.Clear();

var mainScreen = new MainScreen(user, financeHandler);
mainScreen.Go();
Console.ReadLine();