﻿using ErebusLauncher.Windows;
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

            splash.Close();

            var presence = new DiscordPresence();
            client = presence.RunConnection("Looking for a game", this);

            CurrentConfig = json.GetLauncherConfigFile();

            ThemeUpdater.UpdateAccents(this, true, "NONE");

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

            JavaPath.Content = $"Java Path: {CurrentConfig.JavaVersion}";
            GameVersion.Content = $"Game Version: {CurrentConfig.GameVersion}";
            LauncherVer.Content = SystemConfig.COMBINE_VERSION;

            UpdateTheme(CurrentConfig.Theme);
            logger.DevStackLog(GetCurrentNews().Result.Entries[0].Title);
        }

        private void Card_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        private async Task<MainNewsManifest?> GetCurrentNews() => await News.GetNewsJSON();

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
    }
}
