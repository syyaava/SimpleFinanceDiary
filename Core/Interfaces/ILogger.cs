using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ILogger
    {
        void Write(string message);

        static void Log(IEnumerable<ILogger> loggers, string message)
        {
            foreach (var logger in loggers)
                logger.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " " + message);
        }
    }
}
