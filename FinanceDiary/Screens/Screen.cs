using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceDiaryConsole
{
    internal abstract class Screen<T>
    {
        public abstract T Go();
        protected void DisplayError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.ReadLine();
            Console.Clear();
        }
    }
}
