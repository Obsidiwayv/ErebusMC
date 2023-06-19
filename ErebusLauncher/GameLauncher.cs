using CmlLib.Core;
using CmlLib.Core.Auth;
using ProjBobcat.DefaultComponent.ResourceInfoResolver;
using ProjBobcat.DefaultComponent;
using ProjBobcat.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjBobcat.Class.Model;
using Erebus.MojangAPI;
using ProjBobcat.Class.Model.Forge;
using ProjBobcat.Class.Model.LauncherProfile;
using ProjBobcat.DefaultComponent.Authenticator;
using ProjBobcat.DefaultComponent.Launch.GameCore;
using ProjBobcat.DefaultComponent.Launch;
using ProjBobcat.DefaultComponent.Logging;
using System.Threading;
using ProjBobcat.Event;
using System.Linq;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Net.Http;

namespace ErebusLauncher
{
    internal class GameLauncher
    {

        private MainWindow Main;

        private CMLauncher launcher;

        public GameLauncher(MainWindow main)
        {
            Main = main;

            launcher = new CMLauncher(new MinecraftPath());

            var rootPath = MinecraftPath.GetOSDefaultPath();
        }

        public async void LaunchGame()
        {
            var configs = Main.json.config;
            var user_data = Main.json.data;

            try
            {
                launcher.FileChanged += Main.ChangeProgressBar;

                // we are just downloading, no need to launch this...
                var process = await launcher.CreateProcessAsync(configs.GameVersion, new MLaunchOption
                {
                    MaximumRamMb = int.Parse(configs.RamSize),
                    Session = MSession.GetOfflineSession(user_data.Name),
                    JavaPath = configs.JavaVersion,
                    VersionType = SystemConfig.DEFAULT_NAME,
                    GameLauncherName = SystemConfig.DEFAULT_NAME,
                    Path = new MinecraftPath(),
                    JVMArguments = new string[]
                    { 
                        $"-javaagent:{launcher.MinecraftPath.BasePath + "\\Authlib.jar"}=ely.by"
                    }
                });

                process.Exited += HandleGameExit;
                process.ErrorDataReceived += Process_ErrorDataReceived;

                //var result = await core.LaunchTaskAsync(launchSettings);
                Main.DownloadingText.Content = "Now launching the game...";
                process.Start();
            } catch (Exception err)
            {
                Main.logger.StackLine();
                Main.logger.StackLog($"MINECRAFT LAUNCH ERROR:\n{err}");
                Main.DownloadingText.Content = "Unable to launch the game";
            }
        }

        private void Process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Main.logger.StackLog(e.Data);
        }

        private void HandleGameExit(object? sender, EventArgs e)
        {
        }
    }
}
