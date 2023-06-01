using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErebusLauncher
{
    internal class Java
    {
        private static RegistryKey? key;

        public static void OpenRegistry()
        {
            key = Registry.LocalMachine.OpenSubKey(name: "SOFTWARE\\JavaSoft\\JDK");
        }

        public static String getVersion()
        {
            if (key == null)
            {
                return "Java not installed";
            } else
            {

            }
        }
    }
}
