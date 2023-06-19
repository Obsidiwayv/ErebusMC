using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Refresh64
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            var request = await CheckGithub();

            try
            {
                if (e.Args[0] == request.Content)
                {
                    main.T.Content = "Killing current Erebus Process...";
                    KillLauncherProc();
                    Thread.Sleep(2000);
                    main.T.Content = "Downloading and extracting erebus update... ";
                    GetUpdate(main, e.Args[0]);
                }
                else
                {
                    WriteToLog("ErebusLauncher is already updated");
                }
            } catch (Exception exc)
            {
                MessageBox.Show($"Cannot check for updates, stack:\n{exc}", "Alert");
            }
        }

        private void KillLauncherProc()
        {
            string processName = "ErebusLauncher";

            // Find the process by name
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                // Kill all found processes
                foreach (Process process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                    WriteToLog("Terminated ErebusLauncher.exe");
                }
            }
            else
            {
                WriteToLog("Could not find process ErebusLauncher... continuing...");
                MessageBox.Show("Unable to terminate Erebus");
            }
        }

        private void WriteToLog(string content)
        {
            var path = ".\\log.txt";

            if (File.Exists(path))
            {
                using (StreamWriter w = File.AppendText(path))
                {
                    w.Write($"{content}\n");
                }
                // Read text from file
                File.ReadAllText(path);
            }
            else
            {
                var cont = "Refresh64 Logs File\n------------\n";
                cont += $"{content}";
                File.WriteAllText(path, cont);
            }
        }

        public string GetLauncherDataFolderPath()
        {
            return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ErebusLauncher");
        }

        private async void GetUpdate(MainWindow main, string ver)
        {
            string zipUrl = $"https://github.com/Obsidiwayv/VoidBlock-Builds/releases/{ver}/download/PublicRelease.zip";
            string downloadPath = GetLauncherDataFolderPath() + "\\Update\\Latest.zip";
            string extractPath = "..\\Launcher";
            string backupPath = GetLauncherDataFolderPath() + "\\Backup";

            Directory.CreateDirectory(GetLauncherDataFolderPath() + "\\Update");
            Directory.CreateDirectory(GetLauncherDataFolderPath() + "\\Backup");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(zipUrl);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (FileStream fileStream = File.Create(downloadPath))
                        {
                            WriteToLog($"Downloaded Erebus {ver}");
                            await stream.CopyToAsync(fileStream);
                        }
                    }

                    ZipFile.ExtractToDirectory(downloadPath, extractPath, true);
                    ZipFile.ExtractToDirectory(downloadPath, backupPath, true);
                    WriteToLog("Extracted zip file to Backup and updated launcher");
                    MessageBox.Show("Finished Downloading update");
                    File.Delete(downloadPath);
                    this.Shutdown();
                }
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
