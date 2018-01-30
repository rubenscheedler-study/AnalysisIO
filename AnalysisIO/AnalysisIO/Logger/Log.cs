using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisIO_Console.Logger
{
    public class Log
    {
        public static void Write(string message)
        {
            string PATH_STRING = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/analysisIO/" + "log.txt";
            if (!File.Exists(PATH_STRING))
            {
                File.Create(PATH_STRING).Close();
            }
            File.AppendAllText(PATH_STRING, message+"\r\n");
        }
    }
}
