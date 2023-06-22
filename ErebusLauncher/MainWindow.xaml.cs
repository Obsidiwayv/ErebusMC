using ErebusLauncher.Windows;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net;
using HandyControl.Data;
using HandyControl.Themes;
using ProjBobcat.Class.Helper;
using System.Collections.Generic;
using System;
using Erebus.Utils;
using DiscordRPC;
using Erebus.Utils.Data;
using System.Linq;
using System.Windows.Input;
using HandyControl.Tools;
using System.Xml.Linq;
using System.IO;
using System.Threading.Tasks;
using Erebus.MojangAPI;
using Erebus.MojangAPI.Model;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using CmlLib.Core.Downloader;

namespace ErebusLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal LauncherFiles json;

        public DiscordRpcClient client;

        internal Obsidi.Jupiter.Logger logger;

        private Boolean CanLaunchGame;

        private UserConfig CurrentConfig;

        public MainWindow()
        {
            var splash = new Splash();
            splash.Show();

            InitializeComponent();
            json = new LauncherFiles();
            logger = new Obsidi.Jupiter.Logger(SystemConfig.DEFAULT_NAME, SystemConfig.IS_DEV);
            ServicePointManager.DefaultConnectionLimit = 512;
            json.RunChecker();
            InitDarkAndLightBox();
            UpdateWallpaper();

            splash.Close();
            StartRefreshProgram();

            var presence = new DiscordPresence();
            client = presence.RunConnection("Looking for a game", this);
            //UpdateDiscord();

            CurrentConfig = json.GetLauncherConfigFile();

            ThemeUpdater.UpdateAccents(this, true, "NONE");

            if (CurrentConfig.JavaVersion == "None")
            {
                if (CurrentConfig.Theme == "Light")
                {
                    LaunchGame.Background = Brushes.Black;
                    LaunchGame.Foreground = Brushes.White;
                } else
                {
                    LaunchGame.Background = Brushes.White;
                    LaunchGame.Foreground = Brushes.Black;
                }
                CanLaunchGame = false;
            }

            JavaPath.Content = $"Java Path: {CurrentConfig.JavaVersion}";
            GameVersion.Content = $"Game Version: {CurrentConfig.GameVersion}";
            LauncherVer.Content = SystemConfig.COMBINE_VERSION;

            UpdateTheme(CurrentConfig.Theme);
        }

        private void Card_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        private void StartRefreshProgram()
        {
            try
            {
                var p = new Process();
                var processPath = "..\\Updater\\Refresh64.exe";
                p.StartInfo.FileName = processPath;
                p.StartInfo.ArgumentList.Add(SystemConfig.VERSION);
                p.Start();
            } catch (Exception ex)
            {
                if (!SystemConfig.IS_DEV)
                {
                    MessageBox.Show("Unable to find Refresh64.exe! cannot continue without updates");
                    Application.Current.Shutdown();
                }
            }
        }

        private async Task<MainNewsManifest?> GetCurrentNews() => await News.GetNewsJSON();

        private void UpdateDiscord()
        {
            if (json.config.DiscordPresence == "Disabled")
            {
                client.Deinitialize();
            }
        }

        private void UpdateWallpaper()
        {
            if (json.config.WallpaperPath != "None")
            {
                try
                {
                    ImageBrush myBrush = new ImageBrush();
                    var image = new Image()
                    {
                        Source = new BitmapImage(new Uri(json.config.WallpaperPath))
                    };
                    myBrush.ImageSource = image.Source;
                    BackgroundGrid.Background = myBrush;
                } catch (Exception err)
                {
                    logger.StackLog("Clearing wallpaper from config file...");
                    json.config.WallpaperPath = "None";
                    json.SaveConfig();
                    logger.StackLog($"Cannot use wallpaper, stack has been provided\n{err}");
                }
            }
        }

        public void UpdateTheme(string dol)
        {
            ThemeUpdater.Update(dol, this);
        }

        private void ThemeResources_SystemThemeChanged(object? sender, FunctionEventArgs<ThemeManager.SystemTheme> e)
        { }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new Settings(this);
            settings.Show();
        }

        private void UsernameSubmit_Click(object sender, RoutedEventArgs e)
        {

            if (UsernameBox.Text.Length == 0)
            {
                HandyControl.Controls.Growl.Error("Cannot have an empty username.");
                return;
            }
            else if (UsernameBox.Text.Length < 3)
            {
                HandyControl.Controls.Growl.Error("Must have a username longer than 3 characters.");
                return;
            }
            else if (UsernameBox.Text.Length > 16)
            {
                HandyControl.Controls.Growl.Error("Username must not be longer than 16.");
                return;
            }

            json.data.Name = UsernameBox.Text;
            json.SaveData();
            HandyControl.Controls.Growl.Success($"Updated username to {UsernameBox.Text}");
        }

        private void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CanLaunchGame)
            {
                MessageBox.Show("Minecraft cannot be launched because java is missing.");
                return;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        { }

        private void RefreshUIButton_Click(object sender, RoutedEventArgs e)
        {
            var config = json.GetLauncherConfigFile();
            UpdateTheme(config.Theme);
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {

        }

        

        private void ThemeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String currentTheme = "";

            if (ThemeBox.SelectedItem.ToString() == "System.Windows.Controls.ListBoxItem: Light")
            {
                currentTheme = "Light";
            }
            else
            {
                currentTheme = "Dark";
            }

            UpdateTheme(currentTheme);

            json.config.Theme = currentTheme;
            json.SaveConfig();
        }

        private void InitDarkAndLightBox()
        {
            ListBoxItem dark = new()
            {
                Content = "Dark"
            };

            ListBoxItem light = new()
            {
                Content = "Light"
            };

            ThemeBox.Items.Add(light);

            ThemeBox.Items.Add(dark);
        }

        public void ChangeProgressBar(DownloadFileChangedEventArgs e)
        {
            DownloadingText.Content = $"Downloading/Checking Files: {e.ProgressedFileCount}/{e.TotalFileCount}";
        }

        public void ProgressBarChange(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        { }


        private void LaunchGame_Click(object sender, RoutedEventArgs e)
        {
            if (json.config.GameVersion == "None")
            {
                MessageBox.Show("You have not selected a minecraft version yet ( its in the settings :] )");
                return;
            }

            if (json.data.Name == "GenericUser")
            {
                MessageBox.Show("Your username is empty, make one up or use something.");
            }

            var launcher = new GameLauncher(this);
            launcher.LaunchGame();
        }
    }
}
