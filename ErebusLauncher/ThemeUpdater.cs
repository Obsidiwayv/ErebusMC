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
            }
            else
            {
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
        }

    }
}
