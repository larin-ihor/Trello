using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Trello
{
    static class Logger
    {
        private const string logPath = "log.txt";

        public static void WriteLogAsync(string text)
        {
            Task.Run(() => SaveLog(text));
        }

        static void SaveLog(string text)
        {
            try
            {
                StreamWriter logFile = new StreamWriter(logPath, true, System.Text.Encoding.Default);

                String strLine = DateTime.Now.ToString() + " - " + text;

                logFile.WriteLine(strLine);

                logFile.Close();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
