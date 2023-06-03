﻿using ErebusLauncher.Windows;
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

namespace ErebusLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Collections.Generic.IAsyncEnumerable<string> java;
        private LauncherFiles json;

        public MainWindow()
        {
            json = new LauncherFiles();
            java = SystemInfoHelper.FindJava();
            ServicePointManager.DefaultConnectionLimit = 512;
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            InitializeComponent();
            json.RunChecker();
        }

        private void Card_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        private void ThemeResources_SystemThemeChanged(object? sender, FunctionEventArgs<ThemeManager.SystemTheme> e)
        {
            ThemeManager.Current.AccentColor = Brushes.IndianRed;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new Settings();
            settings.Content = new UserControl();
            settings.Show();
        }

        private async void ShowJava_Click(object sender, RoutedEventArgs e)
        {
            var javaResult = new List<string>();
            var jIndex = 0;

            await foreach (var javas in java)
            {
                Console.WriteLine($"[{jIndex + 1}] 搜索到的 Java - {javas}");
                javaResult.Add(javas);
                jIndex++;
            }
        }

        private void UsernameSubmit_Click(object sender, RoutedEventArgs e)
        {

            if (UsernameBox.Text.Length < 3)
            {
                HandyControl.Controls.Growl.Error("Must have a username longer than 3 characters.");
                return;
            }
            else if (UsernameBox.Text.Length > 16)
            {
                HandyControl.Controls.Growl.Error("Username must not be longer than 16.");
                return;
            }
            else if (UsernameBox.Text.Length == 0)
            {
                HandyControl.Controls.Growl.Error("Cannot have an empty username");
                return;
            }

            json.UpdateUser(UsernameBox.Text);
        }
    }
}
