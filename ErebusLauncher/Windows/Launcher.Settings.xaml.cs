using Erebus.MojangAPI;
using Erebus.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ErebusLauncher.Windows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private LauncherFiles config;

        private MainWindow Main;

        private List<String> JavaPaths;

        private List<String> MCVersions;

        private List<String> ColorLists;

        public Settings(MainWindow main)
        {
            InitializeComponent();

            config = new LauncherFiles();
            config.RunChecker();

            ColorLists = new List<string>();

            JavaPaths = new List<String>();

            MCVersions = new List<String>();

            Main = main;

            SetJavaBox();
            SetVersionBox();

            var boxUtils = new BoxUtils();
            var colors = boxUtils.GetColors();
            main.logger.StackLog(colors.Count.ToString());
            for (int i = 0; i < colors.Count; i++)
            {
                var color = colors[i];
                main.logger.DevStackLog($"Added color {color}");
                ListBoxItem itm = new()
                {
                    Content = color
                };

                ColorLists.Add(color);
                ColorBox.Items.Add(itm);
            }
        }

        private void MakeInfoNotifcation(String content)
        {
            HandyControl.Controls.Growl.Info(content);
        }

        private void JavaVers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedJava = JavaPaths[JavaBox.SelectedIndex];
            Main.logger.StackLog($"user selected java [{selectedJava}]");
            Main.json.config.JavaVersion = selectedJava;
            Main.logger.StackLog("Saving Java Configuration");
            Main.json.SaveConfig();
            MakeInfoNotifcation($"Using java path: {selectedJava}");
            Main.JavaPath.Content = $"Java Path: {selectedJava}";
        }

        private void VersionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = MCVersions[GameVersionBox.SelectedIndex];
            Main.logger.StackLog($"user selected Minecraft Version [{selected}]");
            Main.json.config.GameVersion = selected;
            Main.logger.StackLog($"Saving Minecraft configuration");
            Main.json.SaveConfig();
            Main.GameVersion.Content = $"Game Version: {selected}";
            MakeInfoNotifcation($"Switched game version to {selected}");
        }

        private void ColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Main.logger.StackLog($"Selected color: {ColorLists[ColorBox.SelectedIndex]}");
            var selection = ColorLists[ColorBox.SelectedIndex];
            Main.json.config.ThemeColor = selection;
            Main.json.SaveConfig();

            ThemeUpdater.UpdateAccents(Main, false, selection);
        }

        private async void SetVersionBox()
        {
            try
            {
                var versions = await Versions.GetVersionJSON();

                Main.logger.StackLog("Looping through all known minecraft versions");

                foreach (var version in versions.Versions)
                {
                    var versionItem = new ListBoxItem()
                    {
                        Content = version.Id
                    };
                    MCVersions.Add(version.Id);
                    GameVersionBox.Items.Add(versionItem);
                }
            }
            catch (Exception e)
            {
                Main.logger.StackLog($"Unable to loop due to an error\nA stack has been provided{e}");
                Main.logger.StackLine();
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
                }
                else
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

                    Main.logger.StackLog($"found java path: {content}");

                    ListBoxItem itm = new()
                    {
                        Content = content
                    };

                    JavaPaths.Add(content);
                    JavaBox.Items.Add(itm);
                }
            }

        }
    }
}
