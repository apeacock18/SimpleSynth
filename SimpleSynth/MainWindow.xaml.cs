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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio;
using NAudio.Dsp;
using NAudio.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Collections;

namespace SimpleSynth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WaveOut waveOut = new WaveOut();

        private SignalGenerator osc1;
        private SignalGenerator osc2;
        private SignalGenerator osc3;
        private MixingSampleProvider mixer;

        private double range1;
        private double range2;
        private double range3;
        private double tuning1 = 1;
        private double tuning2 = 1;
        private double tuning3 = 1;
        bool envOn;

        public MainWindow()
        {
            InitializeComponent();
            InitSound();
        }

        public void InitSound()
        {
            InitMixer();
            waveOut.Init(mixer);
        }

        public void InitMixer()
        {
            List<SignalGenerator> samples = new List<SignalGenerator>();
            samples.Add(osc1);
            samples.Add(osc2);
            samples.Add(osc3);
            mixer = new MixingSampleProvider(samples);
            mixer.env.AttackRate = (float)(attack.Value);
            mixer.env.DecayRate = (float)(decay.Value);
            mixer.env.SustainLevel = (float)(sustain.Value);
            mixer.env.ReleaseRate = (float)(release.Value);
            envOn = false;
        }

        private void SetFrq(double _frequency)
        {
            osc1.Frequency = _frequency * range1 * tuning1;
            osc2.Frequency = _frequency * range2 * tuning2;
            osc3.Frequency = _frequency * range3 * tuning3;
        }

        private void Key_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double freq = Convert.ToDouble(((Button)sender).Content);
            SetFrq(freq);
            mixer.env.Gate(envOn);
            waveOut.Play();
        }

        private void Key_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mixer.env.Gate(false);
            waveOut.Stop();
        }

        private void Key_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                double freq = Convert.ToDouble(((Button)sender).Content);
                SetFrq(freq);
                if (waveOut.PlaybackState == PlaybackState.Stopped)
                {
                    mixer.env.Gate(envOn);
                    waveOut.Play();
                }
            }
        }

        private void osc1Range_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                switch (osc1Range.SelectedIndex)
                {
                    case 0:
                        range1 = 4;
                        break;
                    case 1:
                        range1 = 2;
                        break;
                    case 2:
                        range1 = 1;
                        break;
                    case 3:
                        range1 = 0.5;
                        break;
                    case 4:
                        range1 = 0.25;
                        break;
                    case 5:
                        range1 = 0.125;
                        break;
                    default:
                        break;
                }
        }

        private void osc2Range_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (osc2Range.SelectedIndex)
            {
                case 0:
                    range2 = 4;
                    break;
                case 1:
                    range2 = 2;
                    break;
                case 2:
                    range2 = 1;
                    break;
                case 3:
                    range2 = 0.5;
                    break;
                case 4:
                    range2 = 0.25;
                    break;
                case 5:
                    range2 = 0.125;
                    break;
                default:
                    break;
            }
        }

        private void osc3Range_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (osc3Range.SelectedIndex)
            {
                case 0:
                    range3 = 4;
                    break;
                case 1:
                    range3 = 2;
                    break;
                case 2:
                    range3 = 1;
                    break;
                case 3:
                    range3 = 0.5;
                    break;
                case 4:
                    range3 = 0.25;
                    break;
                case 5:
                    range3 = 0.125;
                    break;
                default:
                    break;
            }
        }

        private void osc1Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            osc1.Gain = Convert.ToDouble(osc1Volume.Value);
        }

        private void osc2Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            osc2.Gain = Convert.ToDouble(osc2Volume.Value);
        }

        private void osc3Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            osc3.Gain = Convert.ToDouble(osc3Volume.Value);
        }

        private void osc1Wave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (osc1 == null)
            {
                osc1 = new SignalGenerator();
            }
            switch (osc1Wave.SelectedIndex)
            {
                case 0:
                    osc1.Type = SignalGeneratorType.Sin;
                    break;
                case 1:
                    osc1.Type = SignalGeneratorType.Square;
                    break;
                case 2:
                    osc1.Type = SignalGeneratorType.Triangle;
                    break;
                case 3:
                    osc1.Type = SignalGeneratorType.SawTooth;
                    break;
                case 4:
                    osc1.Type = SignalGeneratorType.Pink;
                    break;
                default:
                    break;    
            }
        }

        private void osc2Wave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (osc2 == null)
            {
                osc2 = new SignalGenerator();
            }
            switch (osc2Wave.SelectedIndex)
            {
                case 0:
                    osc2.Type = SignalGeneratorType.Sin;
                    break;
                case 1:
                    osc2.Type = SignalGeneratorType.Square;
                    break;
                case 2:
                    osc2.Type = SignalGeneratorType.Triangle;
                    break;
                case 3:
                    osc2.Type = SignalGeneratorType.SawTooth;
                    break;
                case 4:
                    osc2.Type = SignalGeneratorType.Pink;
                    break;
                default:
                    break;
            }
        }

        private void osc3Wave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (osc3 == null)
            {
                osc3 = new SignalGenerator();
            }
            switch (osc3Wave.SelectedIndex)
            {
                case 0:
                    osc3.Type = SignalGeneratorType.Sin;
                    break;
                case 1:
                    osc3.Type = SignalGeneratorType.Square;
                    break;
                case 2:
                    osc3.Type = SignalGeneratorType.Triangle;
                    break;
                case 3:
                    osc3.Type = SignalGeneratorType.SawTooth;
                    break;
                case 4:
                    osc3.Type = SignalGeneratorType.Pink;
                    break;
                default:
                    break;
            }
        }

        private void mainVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waveOut.Volume = (float)(mainVolume.Value);
        }

        private void osc1Tuning_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tuning1 = 1 + osc1Tuning.Value;
        }

        private void osc2Tuning_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tuning2 = 1 + osc2Tuning.Value;
        }

        private void osc3Tuning_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tuning3 = 1 + osc3Tuning.Value;
        }

        private void SetVoiceFrq()
        {

        }

        int i = 0;

        private void PlayKey(Button key)
        {
            double _frequency = Convert.ToDouble(key.Content);

            osc1.Frequency = _frequency * range1 * tuning1;
            osc2.Frequency = _frequency * range2 * tuning2;
            osc3.Frequency = _frequency * range3 * tuning3;

            mixer.env.Gate(envOn);
            if (waveOut.PlaybackState == PlaybackState.Playing)
                waveOut.Stop();

            if (waveOut.PlaybackState == PlaybackState.Stopped)
                waveOut.Play();
            i++;
        }

        private void OnKeyDownHandler(Object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                switch (e.Key)
                {
                    case Key.Q:
                        PlayKey(A3);
                        break;
                    case Key.W:
                        PlayKey(B3);
                        break;
                    case Key.E:
                        PlayKey(C4);
                        break;
                    case Key.R:
                        PlayKey(D4);
                        break;
                    case Key.T:
                        PlayKey(E4);
                        break;
                    case Key.Y:
                        PlayKey(F4);
                        break;
                    case Key.U:
                        PlayKey(G4);
                        break;
                    case Key.I:
                        PlayKey(A4);
                        break;
                    case Key.O:
                        PlayKey(B4);
                        break;
                    case Key.P:
                        PlayKey(C5);
                        break;
                    case Key.OemOpenBrackets:
                        PlayKey(D5);
                        break;
                    case Key.Z:
                        PlayKey(E5);
                        break;
                    case Key.X:
                        PlayKey(F5);
                        break;
                    case Key.C:
                        PlayKey(G5);
                        break;
                    case Key.V:
                        PlayKey(A5);
                        break;
                    case Key.B:
                        PlayKey(B5);
                        break;
                    case Key.N:
                        PlayKey(C6);
                        break;
                    case Key.M:
                        PlayKey(D6);
                        break;
                    case Key.OemComma:
                        PlayKey(E6);
                        break;
                    case Key.OemPeriod:
                        PlayKey(F6);
                        break;
                    default:
                        break;
                }
            }
        }

        private void StopKey(Button key)
        {
            if (IsKeyPressed())
                return;
            waveOut.Stop();
        }

        private bool IsKeyPressed()
        {
            return (Keyboard.IsKeyDown(Key.Q) || Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.R) || Keyboard.IsKeyDown(Key.T) || Keyboard.IsKeyDown(Key.Y)
                || Keyboard.IsKeyDown(Key.U) || Keyboard.IsKeyDown(Key.I) || Keyboard.IsKeyDown(Key.O) || Keyboard.IsKeyDown(Key.P) || Keyboard.IsKeyDown(Key.OemOpenBrackets) || Keyboard.IsKeyDown(Key.Z)
                || Keyboard.IsKeyDown(Key.X) || Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.V));
        }

        private void OnKeyUpHandler(Object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q:
                    StopKey(A3);
                    break;
                case Key.W:
                    StopKey(B3);
                    break;
                case Key.E:
                    StopKey(C4);
                    break;
                case Key.R:
                    StopKey(D4);
                    break;
                case Key.T:
                    StopKey(E4);
                    break;
                case Key.Y:
                    StopKey(F4);
                    break;
                case Key.U:
                    StopKey(G4);
                    break;
                case Key.I:
                    StopKey(A4);
                    break;
                case Key.O:
                    StopKey(B4);
                    break;
                case Key.P:
                    StopKey(C5);
                    break;
                case Key.OemOpenBrackets:
                    StopKey(D5);
                    break;
                case Key.Z:
                    StopKey(E5);
                    break;
                case Key.X:
                    StopKey(F5);
                    break;
                case Key.C:
                    StopKey(G5);
                    break;
                case Key.V:
                    StopKey(A5);
                    break;
                case Key.B:
                    StopKey(B5);
                    break;
                case Key.N:
                    StopKey(C6);
                    break;
                case Key.M:
                    StopKey(D6);
                    break;
                case Key.OemComma:
                    StopKey(E6);
                    break;
                case Key.OemPeriod:
                    StopKey(F6);
                    break;
                default:
                    break;
            }
        }

        private void attack_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mixer != null)
            {
                mixer.env.AttackRate = (float)(attack.Value);
            }
        }

        private void decay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mixer != null)
            {
                mixer.env.DecayRate = (float)(decay.Value);
            }
        }

        private void sustain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mixer != null)
            {
                mixer.env.SustainLevel = (float)(sustain.Value);
            }
        }

        private void release_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mixer != null)
            {
                mixer.env.ReleaseRate = (float)(release.Value);
            }
        }

        private void envPower_Click(object sender, RoutedEventArgs e)
        {
            envOn = !envOn;
            if (envOn)
                envPower.Content = "On";
            else
                envPower.Content = "Off";
        }

        private void filterCutoff_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mixer != null)
            {
                mixer.Cutoff = (float)(filterCutoff.Value);
            }
        }

        private void filterPower_Click(object sender, RoutedEventArgs e)
        {
            mixer.filterOn = !(mixer.filterOn);
            if (mixer.filterOn)
            {
                filterPower.Content = "On";
            }
            else
            {
                filterPower.Content = "Off";
            }
        }
    }

    static class SampleProviderConverters
    {
        /// <summary>
        /// Helper function to go from IWaveProvider to a SampleProvider
        /// Must already be PCM or IEEE float
        /// </summary>
        /// <param name="waveProvider">The WaveProvider to convert</param>
        /// <returns>A sample provider</returns>
        public static ISampleProvider ConvertWaveProviderIntoSampleProvider(IWaveProvider waveProvider)
        {
            ISampleProvider sampleProvider;
            if (waveProvider.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
            {
                // go to float
                if (waveProvider.WaveFormat.BitsPerSample == 8)
                {
                    sampleProvider = new Pcm8BitToSampleProvider(waveProvider);
                }
                else if (waveProvider.WaveFormat.BitsPerSample == 16)
                {
                    sampleProvider = new Pcm16BitToSampleProvider(waveProvider);
                }
                else if (waveProvider.WaveFormat.BitsPerSample == 24)
                {
                    sampleProvider = new Pcm24BitToSampleProvider(waveProvider);
                }
                else if (waveProvider.WaveFormat.BitsPerSample == 32)
                {
                    sampleProvider = new Pcm32BitToSampleProvider(waveProvider);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported bit depth");
                }
            }
            else if (waveProvider.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
            {
                if (waveProvider.WaveFormat.BitsPerSample == 64)
                    sampleProvider = new WaveToSampleProvider64(waveProvider);
                else
                    sampleProvider = new WaveToSampleProvider(waveProvider);
            }
            else
            {
                throw new ArgumentException("Unsupported source encoding");
            }
            return sampleProvider;
        }
    }

    public class MixingSampleProvider : ISampleProvider
    {
        private List<ISampleProvider> sources;
        private WaveFormat waveFormat;
        private float[] sourceBuffer;
        private const int maxInputs = 1024; // protect ourselves against doing something silly
        public EnvelopeGenerator env = new EnvelopeGenerator();
        private BiQuadFilter filter;
        public float cutoffFrq = 300f;

        public float Cutoff
        {
            get { return cutoffFrq; }
            set
            {
                cutoffFrq = value;
                filter = BiQuadFilter.LowPassFilter(44100, value, 1);
                //switch (filterType)
                //{
                //    case "lowpass":
                //        filter = BiQuadFilter.LowPassFilter(44100, value, 1);
                //        break;
                //    case "highpass":
                //        filter = BiQuadFilter.HighPassFilter(44100, value, 1);
                //        break;
                //    default:
                //        break;
                //}
            }
        }

        //public string filterType
        //{
        //    get { return filterType; }
        //    set
        //    {
        //        switch (value)
        //        {
        //            case "lowpass":
        //                filter = BiQuadFilter.LowPassFilter(44100, cutoffFrq, 1);
        //                break;
        //            case "highpass":
        //                filter = BiQuadFilter.HighPassFilter(44100, cutoffFrq, 1);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        /// <summary>
        /// Creates a new MixingSampleProvider, with no inputs, but a specified WaveFormat
        /// </summary>
        /// <param name="waveFormat">The WaveFormat of this mixer. All inputs must be in this format</param>
        public MixingSampleProvider(WaveFormat waveFormat)
        {
            if (waveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                throw new ArgumentException("Mixer wave format must be IEEE float");
            }
            this.sources = new List<ISampleProvider>();
            this.waveFormat = waveFormat;
        }

        /// <summary>
        /// Creates a new MixingSampleProvider, based on the given inputs
        /// </summary>
        /// <param name="sources">Mixer inputs - must all have the same waveformat, and must
        /// all be of the same WaveFormat. There must be at least one input</param>
        public MixingSampleProvider(IEnumerable<ISampleProvider> sources)
        {
            env.AttackRate = 44100f;
            env.DecayRate = 44100f;
            env.SustainLevel = .8f;
            env.ReleaseRate = 44100f;
            filter = BiQuadFilter.LowPassFilter(44100f, cutoffFrq, 1);

            this.sources = new List<ISampleProvider>();
            foreach (var source in sources)
            {
                AddMixerInput(source);
            }
            if (this.sources.Count == 0)
            {
                throw new ArgumentException("Must provide at least one input in this constructor");
            }
        }

        /// <summary>
        /// When set to true, the Read method always returns the number
        /// of samples requested, even if there are no inputs, or if the
        /// current inputs reach their end. Setting this to true effectively
        /// makes this a never-ending sample provider, so take care if you plan
        /// to write it out to a file.
        /// </summary>
        public bool ReadFully { get; set; }

        /// <summary>
        /// Adds a WaveProvider as a Mixer input.
        /// Must be PCM or IEEE float already
        /// </summary>
        /// <param name="mixerInput">IWaveProvider mixer input</param>
        public void AddMixerInput(IWaveProvider mixerInput)
        {
            AddMixerInput(SampleProviderConverters.ConvertWaveProviderIntoSampleProvider(mixerInput));
        }

        /// <summary>
        /// Adds a new mixer input
        /// </summary>
        /// <param name="mixerInput">Mixer input</param>
        public void AddMixerInput(ISampleProvider mixerInput)
        {
            // we'll just call the lock around add since we are protecting against an AddMixerInput at
            // the same time as a Read, rather than two AddMixerInput calls at the same time
            lock (sources)
            {
                if (this.sources.Count >= maxInputs)
                {
                    throw new InvalidOperationException("Too many mixer inputs");
                }
                this.sources.Add(mixerInput);
            }
            if (this.waveFormat == null)
            {
                this.waveFormat = mixerInput.WaveFormat;
            }
            else
            {
                if (this.WaveFormat.SampleRate != mixerInput.WaveFormat.SampleRate ||
                    this.WaveFormat.Channels != mixerInput.WaveFormat.Channels)
                {
                    throw new ArgumentException("All mixer inputs must have the same WaveFormat");
                }
            }
        }

        /// <summary>
        /// Removes a mixer input
        /// </summary>
        /// <param name="mixerInput">Mixer input to remove</param>
        public void RemoveMixerInput(ISampleProvider mixerInput)
        {
            lock (sources)
            {
                this.sources.Remove(mixerInput);
            }
        }

        /// <summary>
        /// Removes all mixer inputs
        /// </summary>
        public void RemoveAllMixerInputs()
        {
            lock (sources)
            {
                this.sources.Clear();
            }
        }

        /// <summary>
        /// The output WaveFormat of this sample provider
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return this.waveFormat; }
        }

        public bool filterOn = false;

        /// <summary>
        /// Reads samples from this sample provider
        /// </summary>
        /// <param name="buffer">Sample buffer</param>
        /// <param name="offset">Offset into sample buffer</param>
        /// <param name="count">Number of samples required</param>
        /// <returns>Number of samples read</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int outputSamples = 0;
            this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, count);
            lock (sources)
            {
                int index = sources.Count - 1;
                while (index >= 0)
                {
                    var source = sources[index];
                    int samplesRead = source.Read(this.sourceBuffer, 0, count);
                    int outIndex = offset;
                    for (int n = 0; n < samplesRead; n++)
                    {
                        if (env.State != EnvelopeGenerator.EnvelopeState.Idle)
                        {
                            if (filterOn)
                            {
                                if (n >= outputSamples)
                                {
                                    buffer[outIndex++] = filter.Transform(this.sourceBuffer[n] * env.Process());
                                }
                                else
                                {
                                    buffer[outIndex++] += filter.Transform(this.sourceBuffer[n] * env.Process());
                                }
                            }
                            else
                            {
                                if (n >= outputSamples)
                                {
                                    buffer[outIndex++] = this.sourceBuffer[n] * env.Process();
                                }
                                else
                                {
                                    buffer[outIndex++] += this.sourceBuffer[n] * env.Process();
                                }
                            }
                        }
                        else
                        {
                            if (filterOn)
                            {
                                if (n >= outputSamples)
                                {
                                    buffer[outIndex++] = filter.Transform(this.sourceBuffer[n]);
                                }
                                else
                                {
                                    buffer[outIndex++] += filter.Transform(this.sourceBuffer[n]);
                                }
                            }
                            else
                            {
                                if (n >= outputSamples)
                                {
                                    buffer[outIndex++] = this.sourceBuffer[n];
                                }
                                else
                                {
                                    buffer[outIndex++] += this.sourceBuffer[n];
                                }
                            }
                        }
                    }
                    outputSamples = Math.Max(samplesRead, outputSamples);
                    if (samplesRead == 0)
                    {
                        sources.RemoveAt(index);
                    }
                    index--;
                }
            }
            // optionally ensure we return a full buffer
            if (ReadFully && outputSamples < count)
            {
                int outputIndex = offset + outputSamples;
                while (outputIndex < offset + count)
                {
                    buffer[outputIndex++] = 0;
                }
                outputSamples = count;
            }
            return outputSamples;
        }
    }

}
