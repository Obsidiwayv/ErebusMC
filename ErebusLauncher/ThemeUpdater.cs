using Erebus.Utils.Data;
using ErebusLauncher.Colors;
using ErebusLauncher.Properties;
using HandyControl.Controls;
using HandyControl.Themes;
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
                main.MainGrid.Background = Brushes.WhiteSmoke;
                main.GameCard.Background = Brushes.FloralWhite;
                main.MainCard.Background = Brushes.FloralWhite;
                main.GameCard_Extra.BorderBrush = Brushes.Black;
                main.GameCard_Extra.Background = Brushes.FloralWhite;
                main.JavaText.Foreground = Brushes.Black;
                main.JavaText_2.Foreground = Brushes.Black;
                main.GameVersion_Text.Foreground = Brushes.Black;
                main.VersionListBox.Background = Brushes.FloralWhite;
                main.VersionListBox.BorderBrush = Brushes.Black;
            }
            else
            {
                main.MainGrid.Background = (Brush)new BrushConverter().ConvertFrom("#0E1111");
                main.GameCard.Background = Brushes.Black;
                main.MainCard.Background = Brushes.Black;
                main.GameCard_Extra.BorderBrush = Brushes.FloralWhite;
                main.GameCard_Extra.Background = Brushes.Black;
                main.JavaText.Foreground = Brushes.FloralWhite;
                main.JavaText_2.Foreground = Brushes.FloralWhite;
                main.GameVersion_Text.Foreground = Brushes.FloralWhite;
                main.VersionListBox.Background = Brushes.Black;
                main.VersionListBox.BorderBrush = Brushes.FloralWhite;
            }
        }

        public static void UpdateAccents(MainWindow main, Settings settings)
        {
            UserConfig config = main.json.GetLauncherConfigFile();
            String color = config.ThemeColor;
            Brush brushColor;

            if (color == "Galaxy")
            {
                brushColor = ConvertColor(DarkModeColors.Galaxy);
                UpdateGlobalColors(main, settings, brushColor);
            } else
            {
                brushColor = ConvertColor(LightModeColors.White);
                UpdateGlobalColors(main, settings, brushColor);
            }

            ThemeManager.Current.AccentColor = brushColor;
        }
        
        private static Brush ConvertColor(String hex)
        {
            return (Brush)new BrushConverter().ConvertFrom(hex);
        }
        private static void UpdateGlobalColors(MainWindow main, Settings settings, Brush color)
        {
            main.LaunchGameButton.Background = color;
            main.UsernameSubmit.Background = color;
        }

    }
}
