using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Refresh64
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProg.Value = e.ProgressPercentage;
        }

        private void Completed(object? sender, AsyncCompletedEventArgs e)
        {
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            WebClient webClient = new WebClient();                                                          // Creates a webclient
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);                   // Uses the Event Handler to check whether the download is complete
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);  // Uses the Event Handler to check for progress made
            webClient.DownloadFileAsync(new Uri("https://github.com/Obsidiwayv/VoidBlock-Builds/raw/main/test.zip"), "..\\");
        }
    }
}
