using ErebusLauncher.Windows;
using System.Windows;
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

namespace ErebusLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LauncherFiles json;

        private DiscordRpcClient client;

        private Boolean CanLaunchGame;

        public MainWindow()
        {
            json = new LauncherFiles();
            ServicePointManager.DefaultConnectionLimit = 512;
            InitializeComponent();
            json.RunChecker();

            var presence = new DiscordPresence();
            client = presence.RunConnection("Looking for a game");

            if (json.config.JavaVersion == "None" && json.config.GameVersion == "None")
            {
                LaunchGameButton.Background = Brushes.LightGray;
                LaunchGameButton.Foreground = Brushes.Black;
            }

            UpdateTheme(json.config.Theme);
        }

        private void Card_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        public void UpdateTheme(string dol)
        {
            if (dol == "Light")
            {
                MainCard.Background = Brushes.FloralWhite;
                GameCard.Background = Brushes.White;
            }
            else
            {
                MainCard.Background = Brushes.DarkSlateGray;
                GameCard.Background = Brushes.DimGray;
            }
        }

        private void ThemeResources_SystemThemeChanged(object? sender, FunctionEventArgs<ThemeManager.SystemTheme> e)
        {}

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new Settings();
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
    }
}
