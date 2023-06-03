using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Text.Json;
using System.Text.Json.Serialization;
using Erebus.Utils.Data;
using System.Diagnostics;

namespace Erebus.Utils
{
    internal class LauncherFiles
    {
        public DataUser? data;
        public void RunChecker()
        {
                data = new DataUser()
                {
                    Name = "GenericUser"
                };

                Directory.CreateDirectory(GetLauncherDataFolderPath());
                var playerPath = Path.Combine(GetLauncherDataFolderPath(), "player.json");
                if (!File.Exists(playerPath))
                {
                    string json = JsonSerializer.Serialize(data);
                    File.WriteAllText(playerPath, json);
                }
        }

        public async void SaveData()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            await File.WriteAllTextAsync(Path.Combine(GetLauncherDataFolderPath(), "player.json"), json);
        }

        public string GetLauncherDataFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ErebusLauncher");
        }
    }
}
