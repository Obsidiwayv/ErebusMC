using Erebus.Utils.Data;
using ErebusLauncher.Windows;
using HandyControl.Controls;
using HandyControl.Themes;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ErebusLauncher
{
    internal class ThemeUpdater
    {
        public static void Update(String theme, MainWindow main) {
            if (theme == "Light")
            {
                main.BottomCard_Dark.Hide();
                main.BottomCard_Light.Show();
                main.NewsCardDark.Hide();
                main.NewsCardLight.Show();
                main.NewsText1.Foreground = Brushes.Black;
            }
            else
            {
                main.BottomCard_Dark.Show();
                main.BottomCard_Light.Hide();
                main.NewsCardDark.Show();
                main.NewsCardLight.Hide();
                main.NewsText1.Foreground = Brushes.White;
            }
        }

        public static void UpdateAccents(MainWindow main, bool useConfig, String? uColor)
        {
            String color;
            main.logger.StackLog("Theme: Updating accent colors to what selected");
            
            if (useConfig)
            {
                UserConfig config = main.json.GetLauncherConfigFile();
                color = config.ThemeColor;
            } else
            {
                color = uColor;
            }

            Brush brushColor;

            if (color == "Galaxy")
            {
                brushColor = ConvertColor(ThemeColors.Galaxy);
                UpdateGlobalColors(main, brushColor);
            } else if (color == "HotPink")
            {
                brushColor = ConvertColor(ThemeColors.HotPink);
                UpdateGlobalColors(main, brushColor);
            } else
            {
                brushColor = ConvertColor(ThemeColors.LightGray);
                UpdateGlobalColors(main, brushColor);
            }

            ThemeManager.Current.AccentColor = brushColor;
        }
        
        private static Brush ConvertColor(String hex)
        {
            return (Brush)new BrushConverter().ConvertFrom(hex);
        }
        private static void UpdateGlobalColors(MainWindow main, Brush color)
        {
            main.LaunchGame.Background = color;
            main.SettingsButton.Background = color;
        }

    }
}
