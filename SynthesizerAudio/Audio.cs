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
        public WaveOut waveOut = new WaveOut();
        public List<MySignalGenerator> oscs = new List<MySignalGenerator>();
        public NAudioExtensions.MixingSampleProvider mixer;
        public bool envOn;

        public int Latency
        {
            get { return waveOut.DesiredLatency; }
            set { waveOut.DesiredLatency = value; }
        }

        public int Buffers
        {
            get { return waveOut.NumberOfBuffers; }
            set { waveOut.NumberOfBuffers = value; }
        }

        public AudioProvider() : this(3) {}

        public AudioProvider(int oscNum)
        {
            List<MySignalGenerator> samples = new List<MySignalGenerator>();

            for (int i = 0; i < oscNum; i++)
            {
                oscs.Add(new MySignalGenerator());
                samples.Add(oscs[i]);
            }
            mixer = new NAudioExtensions.MixingSampleProvider(samples);

            mixer.env.AttackRate = (44100f);
            mixer.env.DecayRate = (44100f);
            mixer.env.SustainLevel = (.6f);
            mixer.env.ReleaseRate = (44100);
            envOn = false;

            waveOut.NumberOfBuffers = 2;
            waveOut.DesiredLatency = 80;
            waveOut.Init(mixer);
            if (oscNum <=0 )
            {
                throw new ArgumentException("Constructor must take at least one oscillator");
            }
        }

        public AudioProvider(int oscNum, int buffers, int latency)
        {
            List<MySignalGenerator> samples = new List<MySignalGenerator>();

            if (oscNum <= 0)
            {
                throw new ArgumentException("Constructor must take at least one oscillator");
            }

            for (int i = 0; i < oscNum; i++)
            {
                oscs.Add(new MySignalGenerator());
                samples.Add(oscs[i]);
            }
            mixer = new NAudioExtensions.MixingSampleProvider(samples);

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
            waveOut.NumberOfBuffers = buffers;
            waveOut.DesiredLatency = latency;
            waveOut.Init(mixer);
        }

        public void Play()
        {
            if (waveOut.PlaybackState == PlaybackState.Stopped)
            {
                mixer.env.Gate(false);
                mixer.env.Gate(envOn);
                waveOut.Play();
            }
        }

        public void SetFrq(double _frequency)
        {
            for (int i = 0; i < oscs.Count; i++)
            {
                oscs[i].Frequency = _frequency * oscs[i].octave * oscs[i].tuning;
            }
        }

        public void PhaseLeft(bool state)
        {
            for (int i = 0; i < oscs.Count; i++)
            {
                oscs[i].PhaseReverse[0] = state;
            }
        }

        public void PhaseRight(bool state)
        {
            for (int i = 0; i < oscs.Count; i++)
            {
                oscs[i].PhaseReverse[1] = state;
            }
        }

        public bool IsStopped()
        {
            return (waveOut.PlaybackState == PlaybackState.Stopped);
        }
    }
}
