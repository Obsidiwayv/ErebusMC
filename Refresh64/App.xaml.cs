using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Refresh64
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow wnd = new MainWindow();
            wnd.Show();

            var request = await CheckGithub();

            try
            {
                if (e.Args[0] != request.Content)
                {
                    MessageBox.Show("An update for Erebus Launcher has been found, please close the launcher to start the update.");
                } else
                {
                    wnd.Close();
                }
            } catch (Exception exc)
            {
                MessageBox.Show($"Cannot check for updates, stack:\n{exc}", "Alert");
                wnd.Close();
            }
        }

        private async Task<RestResponse> CheckGithub()
        {
            var client = new RestClient("https://raw.githubusercontent.com/Obsidiwayv/VoidBlock-Builds/main");
            var request = new RestRequest("/version.txt");
            return await client.GetAsync(request);
        }
    }
}
