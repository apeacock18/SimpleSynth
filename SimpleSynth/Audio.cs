using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudioExtensions;

namespace SynthAudio
{
    public class AudioProvider
    {
        // Main WaveOut instance. Only disposed of on program exit
        public WaveOut waveOut = new WaveOut();
        // List of oscillators to be passed into mixer
        public List<MySignalGenerator> oscs = new List<MySignalGenerator>();
        // MixingSampleProvider that supplies waveOut with its waveform
        public MyMixingSampleProvider mixer;
        // Holds enevlope on/off state
        public bool envOn;

        /// <summary>
        /// Sets desired latency in ms. (Default is 150)
        /// </summary>
        public int Latency
        {
            get { return waveOut.DesiredLatency; }
            set { waveOut.DesiredLatency = value; }
        }

        /// <summary>
        /// Sets the desired buffer size. (Default is 2)
        /// </summary>
        public int Buffers
        {
            get { return waveOut.NumberOfBuffers; }
            set { waveOut.NumberOfBuffers = value; }
        }

        public AudioProvider() : this(3) {}

        /// <summary>
        /// Constructor for creating new audioprovider including
        /// mixer, oscillators and envelope
        /// </summary>
        /// <param name="oscNum">Number of oscillators</param>
        public AudioProvider(int oscNum)
        {
            // Fill mixer with oscillators
            List<MySignalGenerator> samples = new List<MySignalGenerator>();
            for (int i = 0; i < oscNum; i++)
            {
                oscs.Add(new MySignalGenerator());
                samples.Add(oscs[i]);
            }
            mixer = new MyMixingSampleProvider(samples);

            // Default envelope
            mixer.env.AttackRate = (44100f);
            mixer.env.DecayRate = (44100f);
            mixer.env.SustainLevel = (.6f);
            mixer.env.ReleaseRate = (44100f);
            envOn = false;

            // Set buffers and latency
            waveOut.NumberOfBuffers = 2;
            waveOut.DesiredLatency = 150;

            // Pass mixer into waveOut
            waveOut.Init(mixer);
            if (oscNum <=0 )
            {
                throw new ArgumentException("Constructor must take at least one oscillator");
            }
        }

        /// <summary>
        /// Constructor for assigning latency and buffers
        /// </summary>
        /// <param name="oscNum">Number of oscillators</param>
        /// <param name="buffers">Number of buffers</param>
        /// <param name="latency">Desired latency in ms</param>
        public AudioProvider(int oscNum, int buffers, int latency)
        {
            if (oscNum <= 0)
            {
                throw new ArgumentException("Constructor must take at least one oscillator");
            }

            // Fill mixer with oscillators
            List<MySignalGenerator> samples = new List<MySignalGenerator>();
            for (int i = 0; i < oscNum; i++)
            {
                oscs.Add(new MySignalGenerator());
                samples.Add(oscs[i]);
            }
            mixer = new MyMixingSampleProvider(samples);

            // Set default envelope values
            mixer.env.AttackRate = (44100f);
            mixer.env.DecayRate = (44100f);
            mixer.env.SustainLevel = (.6f);
            mixer.env.ReleaseRate = (44100);
            envOn = false;

            if (buffers < 0 || buffers > 8)
            {
                throw new ArgumentException("Buffer size must be between 0 and 8");
            }
            if (latency < 0)
            {
                throw new ArgumentException("Latency must be postive");
            }
            // Set number of buffers and latency
            waveOut.NumberOfBuffers = buffers;
            waveOut.DesiredLatency = latency;
            // Pass mixer into waveOut
            waveOut.Init(mixer);
        }

        /// <summary>
        /// Resets envelope and plays waveOut if stopped
        /// </summary>
        public void Play()
        {
            if (waveOut.PlaybackState == PlaybackState.Stopped)
            {
                mixer.env.Gate(false);
                mixer.env.Gate(envOn);
                waveOut.Play();
            }
        }

        /// <summary>
        ///  Sets frequency for each oscillator in mixer
        /// </summary>
        /// <param name="_frequency">Frequency to be assigned</param>
        public void SetFrq(double _frequency)
        {
            for (int i = 0; i < oscs.Count; i++)
            {
                oscs[i].Frequency = _frequency * oscs[i].Octave * oscs[i].Tuning;
            }
        }

        /// <summary>
        /// Shifts phase through left channel
        /// </summary>
        /// <param name="state">0 = off , 1 = on</param>
        public void PhaseLeft(bool state)
        {
            for (int i = 0; i < oscs.Count; i++)
            {
                oscs[i].PhaseReverse[0] = state;
            }
        }

        /// <summary>
        /// Shifts phase through right channel
        /// </summary>
        /// <param name="state">0 = off , 1 = on</param>
        public void PhaseRight(bool state)
        {
            for (int i = 0; i < oscs.Count; i++)
            {
                oscs[i].PhaseReverse[1] = state;
            }
        }

        /// <summary>
        /// Returns true if waveOut is stopped
        /// </summary>
        public bool IsStopped()
        {
            return (waveOut.PlaybackState == PlaybackState.Stopped);
        }
    }
}
