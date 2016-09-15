using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Dsp;
using NAudio.Utils;

namespace NAudioExtensions
{
    public class MySignalGenerator : ISampleProvider
    {
        // Wave format
        private readonly WaveFormat waveFormat;

        // Random Number for the White Noise & Pink Noise Generator
        private readonly Random random = new Random();

        private readonly double[] pinkNoiseBuffer = new double[7];

        // Const Math
        private const double TwoPi = 2 * Math.PI;

        // Generator variable
        private int nSample;

        // Sweep Generator variable
        private double phi;

        /// <summary>
        /// Initializes a new instance for the Generator (Default :: 44.1Khz, 2 channels, Sinus, Frequency = 440, Gain = 1)
        /// </summary>
        public MySignalGenerator()
            : this(44100, 2)
        {
            Octave = 1;
            Tuning = 1;
        }

        /// <summary>
        /// Initializes a new instance for the Generator (UserDef SampleRate &amp; Channels)
        /// </summary>
        /// <param name="sampleRate">Desired sample rate</param>
        /// <param name="channel">Number of channels</param>
        public MySignalGenerator(int sampleRate, int channel)
        {
            phi = 0;
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channel);

            // Default
            Type = SignalGeneratorType.Sin;
            Frequency = 440.0;
            Gain = 1;
            PhaseReverse = new bool[channel];
            SweepLengthSecs = 2;
        }

        /// <summary>
        /// Octave sets octave interval of frequency. Base note is A3
        /// </summary>
        public double Octave { get; set; }

        /// <summary>
        /// Fine tuning for octave interval
        /// </summary>
        public double Tuning { get; set; }

        /// <summary>
        /// Frequency for generator. (20 to 20000 hz)
        /// </summary>
        public double Frequency { get; set; }

        /// <summary>
        /// Gain for the Generator. (0.0 to 1.0)
        /// </summary>
        public double Gain { get; set; }

        /// <summary>
        /// The waveformat of this WaveProvider (same as the source)
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        /// <summary>
        /// Return Log of Frequency Start (Read only)
        /// </summary>
        public double FrequencyLog
        {
            get { return Math.Log(Frequency); }
        }

        /// <summary>
        /// End Frequency for the Sweep Generator. (Start Frequency in Frequency)
        /// </summary>
        public double FrequencyEnd { get; set; }

        /// <summary>
        /// Return Log of Frequency End (Read only)
        /// </summary>
        public double FrequencyEndLog
        {
            get { return Math.Log(FrequencyEnd); }
        }

        /// <summary>
        /// Channel PhaseReverse
        /// </summary>
        public bool[] PhaseReverse { get; private set; }

        /// <summary>
        /// Type of Generator.
        /// </summary>
        public SignalGeneratorType Type { get; set; }

        /// <summary>
        /// Length Seconds for the Sweep Generator.
        /// </summary>
        public double SweepLengthSecs { get; set; }

        /// <summary>
        /// Reads from this provider.
        /// </summary>
        public int Read(float[] buffer, int offset, int count)
        {
            int outIndex = offset;

            // Generator current value
            double multiple;
            double sampleValue;
            double sampleSaw;

            // Complete Buffer
            for (int sampleCount = 0; sampleCount < count / waveFormat.Channels; sampleCount++)
            {
                switch (Type)
                {
                    case SignalGeneratorType.Sin:

                        // Sin Generator
                        multiple = TwoPi * Frequency / waveFormat.SampleRate;
                        sampleValue = Gain * Math.Sin(nSample * multiple);

                        nSample++;

                        break;


                    case SignalGeneratorType.Square:

                        // Square Generator
                        multiple = 2 * Frequency / waveFormat.SampleRate;
                        sampleSaw = ((nSample * multiple) % 2) - 1;
                        sampleValue = sampleSaw > 0 ? Gain : -Gain;

                        nSample++;
                        break;

                    case SignalGeneratorType.Triangle:

                        // Triangle Generator
                        multiple = 2 * Frequency / waveFormat.SampleRate;
                        sampleSaw = ((nSample * multiple) % 2);
                        sampleValue = 2 * sampleSaw;
                        if (sampleValue > 1)
                        {
                            sampleValue = 2 - sampleValue;
                        }
                        if (sampleValue < -1)
                        {
                            sampleValue = -2 - sampleValue;
                        }

                        sampleValue *= Gain;

                        nSample++;
                        break;

                    case SignalGeneratorType.SawTooth:

                        // SawTooth Generator
                        multiple = 2 * Frequency / waveFormat.SampleRate;
                        sampleSaw = ((nSample * multiple) % 2) - 1;
                        sampleValue = Gain * sampleSaw;

                        nSample++;
                        break;

                    case SignalGeneratorType.White:

                        // White Noise Generator
                        sampleValue = (Gain * NextRandomTwo());
                        break;

                    case SignalGeneratorType.Pink:

                        // Pink Noise Generator
                        double white = NextRandomTwo();
                        pinkNoiseBuffer[0] = 0.99886 * pinkNoiseBuffer[0] + white * 0.0555179;
                        pinkNoiseBuffer[1] = 0.99332 * pinkNoiseBuffer[1] + white * 0.0750759;
                        pinkNoiseBuffer[2] = 0.96900 * pinkNoiseBuffer[2] + white * 0.1538520;
                        pinkNoiseBuffer[3] = 0.86650 * pinkNoiseBuffer[3] + white * 0.3104856;
                        pinkNoiseBuffer[4] = 0.55000 * pinkNoiseBuffer[4] + white * 0.5329522;
                        pinkNoiseBuffer[5] = -0.7616 * pinkNoiseBuffer[5] - white * 0.0168980;
                        double pink = pinkNoiseBuffer[0] + pinkNoiseBuffer[1] + pinkNoiseBuffer[2] + pinkNoiseBuffer[3] + pinkNoiseBuffer[4] + pinkNoiseBuffer[5] + pinkNoiseBuffer[6] + white * 0.5362;
                        pinkNoiseBuffer[6] = white * 0.115926;
                        sampleValue = (Gain * (pink / 5));
                        break;

                    case SignalGeneratorType.Sweep:

                        // Sweep Generator
                        double f = Math.Exp(FrequencyLog + (nSample * (FrequencyEndLog - FrequencyLog)) / (SweepLengthSecs * waveFormat.SampleRate));

                        multiple = TwoPi * f / waveFormat.SampleRate;
                        phi += multiple;
                        sampleValue = Gain * (Math.Sin(phi));
                        nSample++;
                        if (nSample > SweepLengthSecs * waveFormat.SampleRate)
                        {
                            nSample = 0;
                            phi = 0;
                        }
                        break;

                    default:
                        sampleValue = 0.0;
                        break;
                }

                // Phase reverse per channel and fill buffer
                for (int i = 0; i < waveFormat.Channels; i++)
                {
                    if (PhaseReverse[i])
                        buffer[outIndex++] = (float)-sampleValue;
                    else
                        buffer[outIndex++] = (float)sampleValue;
                }
            }
            return count;
        }

        /// <summary>
        /// Private :: Random for WhiteNoise &amp; Pink Noise (Value form -1 to 1)
        /// </summary>
        /// <returns>Random value from -1 to +1</returns>
        private double NextRandomTwo()
        {
            return 2 * random.NextDouble() - 1;
        }

        public void SetType(string type)
        {
            switch (type)
            {
                case "sin":
                    this.Type = SignalGeneratorType.Sin;
                    break;
                case "square":
                    this.Type = SignalGeneratorType.Square;
                    break;
                case "triangle":
                    this.Type = SignalGeneratorType.Triangle;
                    break;
                case "sawtooth":
                    this.Type = SignalGeneratorType.SawTooth;
                    break;
                case "pink":
                    this.Type = SignalGeneratorType.Pink;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// NAudio dependency class for MyMixingSampleProvider taken from NAudio source
    /// </summary>
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

    /// <summary>
    /// My implementation of NAudio's MixingSampleProvider class
    /// made to utilize a lowpass filter and ADSR envelope
    /// </summary>
    public class MyMixingSampleProvider : ISampleProvider
    {
        private List<ISampleProvider> sources;
        private WaveFormat waveFormat;
        private float[] sourceBuffer;
        private const int maxInputs = 1024; // protect ourselves against doing something silly
        public EnvelopeGenerator env = new EnvelopeGenerator();
        private BiQuadFilter filter;
        public float cutoffFrq = 300f;

        /// <summary>
        /// Cutoff frequency for lowpass filter. (20 to 20000 hz)
        /// </summary>
        public float Cutoff
        {
            get { return cutoffFrq; }
            set
            {
                cutoffFrq = value;
                filter = BiQuadFilter.LowPassFilter(44100, value, 1);
            }
        }

        /// <summary>
        /// Creates a new MixingSampleProvider with no inputs, but a specified WaveFormat
        /// </summary>
        /// <param name="waveFormat">The WaveFormat of this mixer. All inputs must be in this format</param>
        public MyMixingSampleProvider(WaveFormat waveFormat)
        {
            if (waveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                throw new ArgumentException("Mixer wave format must be IEEE float");
            }
            this.sources = new List<ISampleProvider>();
            this.waveFormat = waveFormat;
        }

        /// <summary>
        /// Creating a new MixingSampleProvider with a collection of sources
        /// </summary>
        /// <param name="sources"></param>
        public MyMixingSampleProvider(IEnumerable<ISampleProvider> sources)
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
        /// makes this a never-ending sample provider.
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
            // Call a lock around AddMixerInput to protect against an AddMixerInput at
            // the same time as a Read
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
            // Ensure a full buffer is returned
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
