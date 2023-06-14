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

            colors.Add("HotPink");
            colors.Add("Galaxy");
            colors.Add("LightGray");

            return colors;
        }
    }
}
