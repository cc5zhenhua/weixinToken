using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
 
 
namespace weixin.Models
{
    public class LogUtil
    {
        private StreamWriter logwriter;
        private bool console=false;
 
        private LogUtil(StreamWriter sw, bool includeConsole)
        {
            this.logwriter = sw;
            this.console = includeConsole;
        }
 
 
        public void Info(String message)
        {
            this.logwriter.WriteLine(DateTime.Now);
            this.logwriter.Write("*Infor*:   ");
            this.logwriter.WriteLine(message);
            if (console)
                Console.WriteLine(message);
            this.logwriter.Flush();
        }
 
        public void Error(String message)
        {
            this.logwriter.Write(DateTime.Now);
            this.logwriter.Write("**Error**:   ");
            this.logwriter.WriteLine(message);
            if (console)
                Console.WriteLine("Error:"+ message);
            this.logwriter.Flush();
        }
 
        public void Dispose()
        {
            if (log != null)
            {
                this.logwriter.Close();
                this.logwriter.Dispose();
                log = null;
            }
        }
 
        private static LogUtil log = null;
        public static LogUtil GetTodayLog(bool includeConsole)
        {
            if (log == null)
            {
                String logfilePath ="C:\\Logs" + String.Format("\\{0}{1}{2}.log", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);                
                StreamWriter sw = new StreamWriter(logfilePath, true, Encoding.UTF8);
                log = new LogUtil(sw, includeConsole);
                return log;
            }
            else
                return log;
        }
    }
}
