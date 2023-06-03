using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Utils;

namespace Erebus.Utils
{
    internal class Misc
    {
        public static string? GetUsername()
        {
            var files = new LauncherFiles();
            var FilePath = Path.Combine(files.GetLauncherDataFolderPath(), "player.json");
            if (FilePath != null)
            {
                String json = File.ReadAllText(FilePath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                return jsonObj[0]["User"];
            } else
            {
                return null;
            }
        }
    }
}
