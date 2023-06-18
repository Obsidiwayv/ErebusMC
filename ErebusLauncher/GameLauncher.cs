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

namespace ErebusLauncher
{
    internal class GameLauncher
    {

        private MainWindow Main;

        private DefaultGameCore core;

        private Guid token;

        private CMLauncher launcher;

        public GameLauncher(MainWindow main)
        {
            Main = main;

            token = Guid.NewGuid();

            launcher = new CMLauncher(new MinecraftPath());

            var rootPath = MinecraftPath.GetOSDefaultPath();

            core = new DefaultGameCore
            {
                ClientToken = token, // Pick any GUID as you like, and it does not affect launching.
                RootPath = rootPath,
                VersionLocator = new DefaultVersionLocator(rootPath, token)
                {
                    LauncherProfileParser = new DefaultLauncherProfileParser(rootPath, token),
                    LauncherAccountParser = new DefaultLauncherAccountParser(rootPath, token)
                },
                GameLogResolver = new DefaultGameLogResolver()
            };
        }

        public async void LaunchGame()
        {
            var configs = Main.json.config;
            var user_data = Main.json.data;


            try
            {
                var launchSettings = new LaunchSettings
                {
                    FallBackGameArguments = new GameArguments
                    {
                        GcType = GcType.G1Gc,
                        JavaExecutable = configs.JavaVersion,
                        Resolution = new ResolutionModel
                        {
                            Height = 600,
                            Width = 800
                        },
                        MinMemory = 512,
                        MaxMemory = 3072
                    },
                    Version = configs.GameVersion,
                    GameName = configs.GameVersion,
                    VersionInsulation = false,
                    GameResourcePath = core.RootPath,
                    GamePath = core.RootPath,
                    VersionLocator = core.VersionLocator,
                    Authenticator = new OfflineAuthenticator //离线认证
                    {
                        Username = user_data.Name, //离线用户名
                        LauncherAccountParser = core.VersionLocator.LauncherAccountParser
                    }
                };

                // we are just downloading, no need to launch this...
                await launcher.CreateProcessAsync(configs.GameVersion, new MLaunchOption{});

                core.GameExitEventDelegate += HandleGameExit;

                launcher.FileChanged += Main.ChangeProgressBar;

                var result = await core.LaunchTaskAsync(launchSettings);
                Main.DownloadingText.Content = "Now launching the game...";
                Main.logger.StackLog($"{result.Error.Exception}");
            } catch (Exception err)
            {
                Main.logger.StackLine();
                Main.logger.StackLog($"MINECRAFT LAUNCH ERROR:\n{err}");
                Main.DownloadingText.Content = "Unable to launch the game";
            }
        }

        private void HandleGameExit(object? sender, GameExitEventArgs e)
        {
            Main.Show();
        }
    }
}
