using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class FileLogger : ILogger
    {
        private const string LOG_PATH = "logs/";
        public FileLogger()
        {

        }

        public void Write(string message)
        {
            if(!Directory.Exists(LOG_PATH))
                Directory.CreateDirectory(LOG_PATH);

            using(StreamWriter sw = new StreamWriter(LOG_PATH + DateTime.Now.ToString("hh-dd-MM-yyyy") + ".txt", true))
            {
                sw.WriteLine(message);
            }
        }
    }
}
