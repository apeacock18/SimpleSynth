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

namespace SimpleSynth
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        // Basic settings window
        private void settingsClose_Click(object sender, RoutedEventArgs e)
        {
            // Set desired latency of audio
            if (latency.Text != "")
            {
                MainWindow.audio.Latency = Convert.ToInt32(latency.Text);
            }
            // Set buffer size of audio
            if (bufferSize.Text != "")
            {
                MainWindow.audio.Buffers = Convert.ToInt32(bufferSize.Text);
            }

            SettingsWindow.Close();
        }
    }
}
