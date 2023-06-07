using System;
using System.IO;

namespace Obsidi.Jupiter
{
    internal class Logger
    {
        private String Stack = "";

        private LogFolder fs;

        public Logger(string foldername)
        {
            fs = new LogFolder(foldername);
        }

        public void StackLog(string level, string message)
        {
            Stack += $"[{level}] >> {message}\n";
        }

        // proj being needed to tell which library/application/operation is doing what
        // and split them into different folders
        public void OutputLogs(string proj)
        {
            var time = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
            Directory.CreateDirectory(fs.GetLogOutputDir());
            File.WriteAllText(Path.Combine(fs.GetLogOutputDir(), $"{proj}-{time}.txt"), Stack);
            Stack = "";
        }
    }
}