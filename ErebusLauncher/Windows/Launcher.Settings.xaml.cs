using Erebus.Utils;
using System;
using System.Collections.Generic;
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

        public Settings()
        {
            InitializeComponent();
            InitDarkAndLightBox();

            config = new LauncherFiles();
            config.RunChecker();

            var boxUtils = new BoxUtils();
            var colors = boxUtils.GetColors();
            for (int i = 0; i < colors.Count; i++)
            {
                var color = colors[i];
                ListBoxItem itm = new()
                {
                    Content = color
                };

                ColorBox.Items.Add(itm);
            }
        }

        private void ColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ThemeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String currentTheme = "";

            if (ThemeBox.SelectedItem.ToString() == "System.Windows.Controls.ListBoxItem: Light")
            {
                currentTheme = "Light";
            } else
            {
                currentTheme = "Dark";
            }

            config.config.Theme = currentTheme;
            config.SaveConfig();

            HandyControl.Controls.Growl.InfoGlobal($"Launcher theme updated to {currentTheme}");
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
