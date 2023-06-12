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

namespace ErebusLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LauncherFiles json;

        public DiscordRpcClient client;

        private Boolean CanLaunchGame;

        private UserConfig CurrentConfig;

        public MainWindow()
        {
            InitializeComponent();
            json = new LauncherFiles();
            ServicePointManager.DefaultConnectionLimit = 512;
            json.RunChecker();
            SetJavaBox();
            HandyControl.Controls.SplashWindow.Instance.LoadComplete();

            var presence = new DiscordPresence();
            client = presence.RunConnection("Looking for a game");

            if (json.config.JavaVersion == "None" && json.config.GameVersion == "None")
            {
                LaunchGameButton.Background = Brushes.LightGray;
                LaunchGameButton.Foreground = Brushes.Black;
                CanLaunchGame = false;
            }

            CurrentConfig = json.GetLauncherConfigFile();

            UpdateTheme(CurrentConfig.Theme);
        }

        private void Card_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        private async void SetJavaBox()
        {
            HandyControl.Controls.SplashWindow.Instance.AddMessage("Looking for Java Versions.");

            var java = SystemInfoHelper.FindJava();
            await foreach (var j in java)
            {
                ListBoxItem itm = new()
                {
                    Content = j
                };

                JavaVers.Items.Add(itm);
            }
        }

        public void UpdateTheme(string dol)
        {

            if (dol == "Light")
            {
                GameCard.Background = Brushes.FloralWhite;
                MainCard.Background = Brushes.FloralWhite;
                GameCard_Extra.BorderBrush = Brushes.Black;
                GameCard_Extra.Background = Brushes.FloralWhite;
                MainCard_Username.BorderBrush = Brushes.Black;
                MainCard_Username.Background = Brushes.FloralWhite;
            }
            else
            {
                GameCard.Background = Brushes.Black;
                MainCard.Background = Brushes.Black;
                GameCard_Extra.BorderBrush = Brushes.FloralWhite;
                GameCard_Extra.Background = Brushes.Black;
                MainCard_Username.BorderBrush = Brushes.FloralWhite;
                MainCard_Username.Background = Brushes.Black;
            }
        }

        private void ThemeResources_SystemThemeChanged(object? sender, FunctionEventArgs<ThemeManager.SystemTheme> e)
        {}

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
    }
}
