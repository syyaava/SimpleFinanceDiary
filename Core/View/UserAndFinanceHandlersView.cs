using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.View
{
    public class UserAndFinanceHandlersView : IUserAndFinanceHandlersView
    {
        public IFinanceHandler FinanceHandler { get; private set; }
        public IUserHandler UserHandler { get; private set; }

        public UserAndFinanceHandlersView(IUserSource userSourse, IFinanceSource financeSource)
        {
            FinanceHandler = new FinanceHandler(financeSource);
            UserHandler = new UserHandler(userSourse);
        }

        public UserAndFinanceHandlersView(IFinanceHandler financeHandler, IUserHandler userHandler)
        {
            FinanceHandler = financeHandler;
            UserHandler = userHandler;
        }
    }
}
