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
        public UserConfig config;

        public void RunChecker()
        {
            data = new DataUser()
            {
                Name = "GenericUser"
            };

            config = new UserConfig()
            {
                GameVersion = "None",
                JavaVersion = "None",
                Theme = "Dark",
                ThemeColor = "Galaxy",
                DiscordPresence = "Enabled",
                WallpaperPath = "None",
                RamSize = "4096"
            };

            Directory.CreateDirectory(GetLauncherDataFolderPath());
            var playerPath = Path.Combine(GetLauncherDataFolderPath(), "player.json");
            var configPath = Path.Combine(GetLauncherDataFolderPath(), "config.json");
            if (!File.Exists(playerPath))
            {
                string json = JsonSerializer.Serialize(data);
                File.WriteAllText(playerPath, json);
            }
            else
            {
                data = GetUserDataFile();
            }
            if (!File.Exists(configPath))
            {
                String json = JsonSerializer.Serialize(config);
                File.WriteAllText(configPath, json);
            } else
            {
                config = GetLauncherConfigFile();
            }
        }

        public async void SaveData()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            await File.WriteAllTextAsync(Path.Combine(GetLauncherDataFolderPath(), "player.json"), json);
        }

        public async void SaveConfig()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(config);
            await File.WriteAllTextAsync(Path.Combine(GetLauncherDataFolderPath(), "config.json"), json);
        }

        public string GetLauncherDataFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ErebusLauncher");
        }

        public UserConfig GetLauncherConfigFile()
        {
            var path = Path.Combine(GetLauncherDataFolderPath(), "config.json");
            var configFile = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<UserConfig>(configFile);
            return config;
        }

        public DataUser GetUserDataFile()
        {
            var path = Path.Combine(GetLauncherDataFolderPath(), "player.json");
            var configFile = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<DataUser>(configFile);
            return config;
        }
    }
}
