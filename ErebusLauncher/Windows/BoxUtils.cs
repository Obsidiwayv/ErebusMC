using HandyControl.Themes;
using System;
using System.Collections.Generic;
using Erebus.Utils;

namespace ErebusLauncher
{
    class BoxUtils
    {

        public List<String> GetColors()
        {
            var currentTheme = new ThemeManager.SystemTheme();
            List<String> colors = new();

            if (!SystemUtils.IsLightTheme())
            {
                colors.Add("Galaxy");
            } else
            {
                colors.Add("LightGray");
                colors.Add("HotPink");
                colors.Add("White");
            }

            return colors;
        }
    }
}
