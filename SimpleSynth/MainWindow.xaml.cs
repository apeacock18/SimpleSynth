using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SynthAudio;

namespace SimpleSynth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static AudioProvider audio; // Instance of main sound generation class
        private int noteIndex = 0; // Keeps track of number of keys pressed

        public MainWindow()
        {
            try
            {
                audio = new AudioProvider();
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured: " + ex.Message + "\n\n" + ex.TargetSite, "Application error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Play note when right mouse button clicks piano keyboard
        private void Key_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Set new frequency, then play note
                double freq = Convert.ToDouble(((Button)sender).Content);
                audio.SetFrq(freq);
                audio.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured: " + ex.Message + "\n\n" + ex.TargetSite, "Application error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Stop audio when mouse button lifts from keyboard
        private void Key_MouseUp(object sender, MouseButtonEventArgs e)
        {
            audio.mixer.env.Gate(false);
            audio.waveOut.Stop();
        }

        // Change frequency to new note if mouse enters different key while pressed
        private void Key_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                double freq = Convert.ToDouble(((Button)sender).Content);
                audio.SetFrq(freq);
                audio.Play();
            }
        }

        private void PlayKey(Button key)
        {
            try
            {
                // Play new note if audio is stopped
                if (audio.IsStopped())
                    audio.waveOut.Play();

                // Set frequency of oscillators
                double _frequency = Convert.ToDouble(key.Content);
                audio.SetFrq(_frequency);

                // Increment index to show that a key was pressed
                // Note: This index sometimes gets unexpectadly incremented, leading to notes
                // being played after the button is released. The other solution would have been to
                // check if any other key was pressed.
                noteIndex++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured: " + ex.Message + "\n\n" + ex.TargetSite, "Application error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnKeyDownHandler(Object sender, KeyEventArgs e)
        {
            // Check if key down event is repeated for thread safety
            if (!e.IsRepeat)
            {
                switch (e.Key)
                {
                    case Key.Q:
                        PlayKey(A3);
                        break;
                    case Key.D2:
                        PlayKey(_A3);
                        break;
                    case Key.W:
                        PlayKey(B3);
                        break;
                    case Key.E:
                        PlayKey(C4);
                        break;
                    case Key.D4:
                        PlayKey(_C4);
                        break;
                    case Key.R:
                        PlayKey(D4);
                        break;
                    case Key.D5:
                        PlayKey(_D4);
                        break;
                    case Key.T:
                        PlayKey(E4);
                        break;
                    case Key.Y:
                        PlayKey(F4);
                        break;
                    case Key.D7:
                        PlayKey(_F4);
                        break;
                    case Key.U:
                        PlayKey(G4);
                        break;
                    case Key.D8:
                        PlayKey(_G4);
                        break;
                    case Key.I:
                        PlayKey(A4);
                        break;
                    case Key.D9:
                        PlayKey(_A4);
                        break;
                    case Key.O:
                        PlayKey(B4);
                        break;
                    case Key.P:
                        PlayKey(C5);
                        break;
                    case Key.OemMinus:
                        PlayKey(_C5);
                        break;
                    case Key.OemOpenBrackets:
                        PlayKey(D5);
                        break;
                    case Key.OemPlus:
                        PlayKey(_D5);
                        break;
                    case Key.Z:
                        PlayKey(E5);
                        break;
                    case Key.X:
                        PlayKey(F5);
                        break;
                    case Key.D:
                        PlayKey(_F5);
                        break;
                    case Key.C:
                        PlayKey(G5);
                        break;
                    case Key.F:
                        PlayKey(_G5);
                        break;
                    case Key.V:
                        PlayKey(A5);
                        break;
                    case Key.G:
                        PlayKey(_A5);
                        break;
                    case Key.B:
                        PlayKey(B5);
                        break;
                    case Key.N:
                        PlayKey(C6);
                        break;
                    case Key.J:
                        PlayKey(_C6);
                        break;
                    case Key.M:
                        PlayKey(D6);
                        break;
                    case Key.K:
                        PlayKey(_D6);
                        break;
                    case Key.OemComma:
                        PlayKey(E6);
                        break;
                    case Key.OemPeriod:
                        PlayKey(F6);
                        break;
                    case Key.OemSemicolon:
                        PlayKey(_F6);
                        break;
                    case Key.OemQuestion:
                        PlayKey(G6);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnKeyUpHandler(Object sender, KeyEventArgs e)
        {
            // Update number of keys pressed
            noteIndex--;

            // Only stop output when no keys are pressed
            if (noteIndex <= 0)
                audio.waveOut.Stop();
        }

        // Change the octave modifier on osc1
        private void osc1Range_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (osc1Range.SelectedIndex)
            {
                case 0:
                    audio.oscs[0].Octave = 4;
                    break;
                case 1:
                    audio.oscs[0].Octave = 2;
                    break;
                case 2:
                    audio.oscs[0].Octave = 1;
                    break;
                case 3:
                    audio.oscs[0].Octave = 0.5;
                    break;
                case 4:
                    audio.oscs[0].Octave = 0.25;
                    break;
                case 5:
                    audio.oscs[0].Octave = 0.125;
                    break;
                default:
                    break;
            }
        }

        // Change the octave modifier on osc2
        private void osc2Range_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (osc2Range.SelectedIndex)
            {
                case 0:
                    audio.oscs[1].Octave = 4;
                    break;
                case 1:
                    audio.oscs[1].Octave = 2;
                    break;
                case 2:
                    audio.oscs[1].Octave = 1;
                    break;
                case 3:
                    audio.oscs[1].Octave = 0.5;
                    break;
                case 4:
                    audio.oscs[1].Octave = 0.25;
                    break;
                case 5:
                    audio.oscs[1].Octave = 0.125;
                    break;
                default:
                    break;
            }
        }

        // Change the octave modifier on osc3
        private void osc3Range_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (osc3Range.SelectedIndex)
            {
                case 0:
                    audio.oscs[2].Octave = 4;
                    break;
                case 1:
                    audio.oscs[2].Octave = 2;
                    break;
                case 2:
                    audio.oscs[2].Octave = 1;
                    break;
                case 3:
                    audio.oscs[2].Octave = 0.5;
                    break;
                case 4:
                    audio.oscs[2].Octave = 0.25;
                    break;
                case 5:
                    audio.oscs[2].Octave = 0.125;
                    break;
                default:
                    break;
            }
        }

        // Modify the gain/ volume
        private void osc1Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.oscs[0].Gain = Convert.ToDouble(osc1Volume.Value);
        }

        private void osc2Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.oscs[1].Gain = Convert.ToDouble(osc2Volume.Value);
        }

        private void osc3Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.oscs[2].Gain = Convert.ToDouble(osc3Volume.Value);
        }

        // Change the wave type of the oscillator
        private void osc1Wave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (osc1Wave.SelectedIndex)
            {
                case 0:
                    audio.oscs[0].SetType("sin");
                    break;
                case 1:
                    audio.oscs[0].SetType("square");
                    break;
                case 2:
                    audio.oscs[0].SetType("triangle");
                    break;
                case 3:
                    audio.oscs[0].SetType("sawtooth");
                    break;
                case 4:
                    audio.oscs[0].SetType("pink");
                    break;
                default:
                    break;
            }
        }

        private void osc2Wave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (osc2Wave.SelectedIndex)
            {
                case 0:
                    audio.oscs[1].SetType("sin");
                    break;
                case 1:
                    audio.oscs[1].SetType("square");
                    break;
                case 2:
                    audio.oscs[1].SetType("triangle");
                    break;
                case 3:
                    audio.oscs[1].SetType("sawtooth");
                    break;
                case 4:
                    audio.oscs[1].SetType("pink");
                    break;
                default:
                    break;
            }
        }

        private void osc3Wave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (osc3Wave.SelectedIndex)
            {
                case 0:
                    audio.oscs[2].SetType("sin");
                    break;
                case 1:
                    audio.oscs[2].SetType("square");
                    break;
                case 2:
                    audio.oscs[2].SetType("triangle");
                    break;
                case 3:
                    audio.oscs[2].SetType("sawtooth");
                    break;
                case 4:
                    audio.oscs[2].SetType("pink");
                    break;
                default:
                    break;
            }
        }

        // Adjust the main volume
        private void mainVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.waveOut.Volume = (float)(mainVolume.Value);
        }

        // Adjust the fine tuned frequency
        private void osc1Tuning_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.oscs[0].Tuning = 1 + osc1Tuning.Value;
        }

        private void osc2Tuning_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.oscs[1].Tuning = 1 + osc2Tuning.Value;
        }

        private void osc3Tuning_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.oscs[2].Tuning = 1 + osc3Tuning.Value;
        }

        // Modify envelope attack
        private void attack_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (audio.mixer != null)
            {
                audio.mixer.env.AttackRate = (float)(attack.Value);
            }
        }

        // Modify envelope decay
        private void decay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (audio.mixer != null)
            {
                audio.mixer.env.DecayRate = (float)(decay.Value);
            }
        }

        // Modify envelope sustain amplitude
        private void sustain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (audio.mixer != null)
            {
                audio.mixer.env.SustainLevel = (float)(sustain.Value);
            }
        }

        // Modify envelope release
        private void release_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (audio.mixer != null)
            {
                audio.mixer.env.ReleaseRate = (float)(release.Value);
            }
        }

        // Toggle envelope
        private void envPower_Click(object sender, RoutedEventArgs e)
        {
            audio.envOn = !(audio.envOn);
            if (audio.envOn)
                envPower.Content = "On";
            else
                envPower.Content = "Off";
        }

        private void filterCutoff_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (audio.mixer != null)
            {
                audio.mixer.Cutoff = (float)(filterCutoff.Value);
            }
        }

        // Toggle filter
        private void filterPower_Click(object sender, RoutedEventArgs e)
        {
            audio.mixer.filterOn = !(audio.mixer.filterOn);
            if (audio.mixer.filterOn)
            {
                filterPower.Content = "On";
            }
            else
            {
                filterPower.Content = "Off";
            }
        }

        // Toggle phase
        private void phaseLeft_Checked(object sender, RoutedEventArgs e)
        {
            audio.PhaseLeft(true);
        }

        private void phaseLeft_Unchecked(object sender, RoutedEventArgs e)
        {
            audio.PhaseLeft(false);
        }

        private void phaseRight_Checked(object sender, RoutedEventArgs e)
        {
            audio.PhaseRight(true);
        }

        private void phaseRight_Unchecked(object sender, RoutedEventArgs e)
        {
            audio.PhaseRight(false);
        }

        // Audio kill switch
        private void stopOutput_Click(object sender, RoutedEventArgs e)
        {
            audio.waveOut.Stop();
            noteIndex = 0; // Bugs usually arise from unwanted index incrementation
        }

        // Create and display settings menu
        private void latencyMenu_Click(object sender, RoutedEventArgs e)
        {
            Settings w = new Settings();
            w.SettingsWindow.Show();
        }

        // Set cutoff value for filter
        private void filterLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.waveOut.Volume = (float)(filterLevel.Value);
        }
    }
}
