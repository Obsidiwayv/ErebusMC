using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidi.Jupiter
{
    internal class LogFolder
    {
        private String Folder = "";

        public LogFolder(string fold)
        {
            Folder = fold;
        }

        public String GetLogOutputDir()
        {
            return Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), Folder, "Logs");
        }
    }
}