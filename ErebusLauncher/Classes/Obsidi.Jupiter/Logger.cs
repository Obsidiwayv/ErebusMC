using MassTransit;
using System;
using System.IO;
using System.Windows.Shapes;

namespace Obsidi.Jupiter
{
    internal class Logger
    {
        private LogFolder fs;

        private String ID;

        public Logger(string foldername)
        {
            fs = new LogFolder(foldername);

            ID = NewId.Next().ToString("D").ToUpperInvariant();
        }

        public void StackLog(string message)
        {
            OutputLogs($"{message}\n");
        }

        public void StackLine()
        {
            OutputLogs(GetLine());
        }

        private String GetLine()
        {
            return "-----------------------------------------------------\n";
        }

        // outputs a single file and writes it to that file with the current id
        // in the file.
        private void OutputLogs(string content)
        {
            var time = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
            var path = $"{fs.GetLogOutputDir()}/log-{ID}.txt";
            Directory.CreateDirectory(fs.GetLogOutputDir());
            if (File.Exists(path))
            {
                using (StreamWriter w = File.AppendText(path))
                {
                    w.Write($"{time} >> {content}\n");
                }
                // Read text from file
                File.ReadAllText(path);
            } else
            {
                var cont = "Erebus Launcher Logs File\n";
                cont += GetLine();
                cont += $"{time} >> {content}";
                File.WriteAllText(path, cont);
            }

        }
    }
}