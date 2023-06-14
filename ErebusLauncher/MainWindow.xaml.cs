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

        private List<String> JavaPaths;

        private List<String> MCVersions;

        public MainWindow()
        {
            var splash = new Splash();
            splash.Show();

            InitializeComponent();
            json = new LauncherFiles();
            JavaPaths = new List<String>();
            MCVersions = new List<String>();
            logger = new Obsidi.Jupiter.Logger(SystemConfig.DEFAULT_NAME);

            ServicePointManager.DefaultConnectionLimit = 512;
            json.RunChecker();
            SetJavaBox();
            setVersionBox();

            splash.Close();

            var presence = new DiscordPresence();
            client = presence.RunConnection("Looking for a game", this);

            CurrentConfig = json.GetLauncherConfigFile();

            if (CurrentConfig.JavaVersion == "None" && CurrentConfig.GameVersion == "None")
            {
                if (CurrentConfig.Theme == "Light")
                {
                    //LaunchGameButton.Background = Brushes.Black;
                    //LaunchGameButton.Foreground = Brushes.White;
                } else
                {
                    //LaunchGameButton.Background = Brushes.White;
                    //LaunchGameButton.Foreground = Brushes.Black;
                }
                CanLaunchGame = false;
            }

            //JavaText_2.Content = $"Java Version: {CurrentConfig.JavaVersion}";
            //GameVersion_Text.Content = $"Game Version: {CurrentConfig.GameVersion}";

            UpdateTheme(CurrentConfig.Theme);
        }

        private void Card_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        private async void setVersionBox()
        {
            try
            {
                var versions = await Versions.GetVersionJSON();

                logger.StackLog("Looping through all known minecraft versions");

                foreach (var version in versions.Versions)
                {
                    MCVersions.Add(version.Id);
                    var versionItem = new ListBoxItem()
                    {
                        Content = $"{version.Id}\n{version.Type}"
                    };
                    //VersionListBox.Items.Add(versionItem);
                }
            } catch(Exception e)
            {
                logger.StackLog($"Unable to loop due to an error\nA stack has been provided{e}");
                logger.StackLine();
            }
        }

        private void SetJavaBox()
        {
            Boolean JavaInSystem = true;
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            string joined = docPath + "\\java";
            string Adoptium = docPath + "\\Eclipse Adoptium";

            if (!Directory.Exists(joined))
            {
                if (Directory.Exists(Adoptium))
                {
                    joined = Adoptium;
                } else
                {
                    JavaInSystem = false;
                }
            }

            if (JavaInSystem)
            {
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(joined));
                foreach (var dir in dirs)
                {
                    var content = $"{dir}\\bin\\java.exe";

                    logger.StackLog($"found java path: {content}");

                    ListBoxItem itm = new()
                    {
                        Content = content
                    };

                    //JavaVers.Items.Add(itm);
                    JavaPaths.Add(content);
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

            //if (UsernameBox.Text.Length == 0)
            //{
                HandyControl.Controls.Growl.Error("Cannot have an empty username.");
            //    return;
            //}
            //else if (UsernameBox.Text.Length < 3)
            //{
                HandyControl.Controls.Growl.Error("Must have a username longer than 3 characters.");
            //    return;
            //}
            //else if (UsernameBox.Text.Length > 16)
            //{
                HandyControl.Controls.Growl.Error("Username must not be longer than 16.");
            //    return;
            //}

            //json.data.Name = UsernameBox.Text;
            json.SaveData();
            //HandyControl.Controls.Growl.Success($"Updated username to {UsernameBox.Text}");
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

        private void JavaVers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedJava = JavaPaths[0];
            logger.StackLog($"user selected java [{selectedJava}]");
            json.config.JavaVersion = selectedJava;
            logger.StackLog("Saving Java Configuration");
            json.SaveConfig();
            //JavaText_2.Content = $"Java Version: {selectedJava}";

            MakeInfoNotifcation($"Using java path: {selectedJava}");
        }

        private void MakeInfoNotifcation(String content)
        {
            HandyControl.Controls.Growl.Info(content);
        }

        private void VersionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = MCVersions[0];
            logger.StackLog($"user selected Minecraft Version [{selected}]");
            json.config.GameVersion  = selected;
            logger.StackLog($"Saving Minecraft configuration");
            json.SaveConfig();
            //GameVersion_Text.Content = $"Game Version: {selected}";
            MakeInfoNotifcation($"Switched game version to {selected}");
        }
    }
}
