using System;
using System.IO;

namespace UI.Helpers
{
    public class LogHelper
    {
        static string LogText = string.Empty;

        public static async void AddLog(string operation, string message)
        {
            string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            string fileDate = DateTime.Now.ToString("yyyy-MM-dd");
            string line = date + "\t" + operation + "\t" + message;

            Console.WriteLine(line);
            //return;

            try
            {
                StreamWriter sw = new StreamWriter("Logs\\Log-" + fileDate + ".txt", true);
                sw.WriteLine(line);
                sw.Close();
                sw.Dispose();

            }
            catch
            {

            }
           
            LogText += line + "\r\n<br />";
        }

        public static void ClearLog()
        {
            LogText = string.Empty;
        }

        public static string GetLog()
        {
            return LogText;
        }
    }
}
