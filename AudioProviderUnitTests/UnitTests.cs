using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynthAudio;
using NAudioExtensions;

namespace AudioProviderUnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void WaveOutInit()
        {
            AudioProvider audio = new AudioProvider();
            Assert.AreEqual(audio.Buffers, 2);
            Assert.AreEqual(audio.Latency, audio.waveOut.DesiredLatency);
            Assert.AreEqual(audio.oscs.Count, 3);
            Assert.IsTrue(audio.IsStopped());
        }

        [TestMethod]
        public void MixerInit()
        {
            AudioProvider audio = new AudioProvider(3);
            Assert.IsFalse(audio.mixer.filterOn);
            Assert.IsFalse(audio.mixer.ReadFully);
            audio.SetFrq(400);
            audio.mixer.AddMixerInput(new MySignalGenerator());
            audio.mixer.RemoveAllMixerInputs();
        }

        [TestMethod]
        public void WaveOut()
        {
            AudioProvider audio = new AudioProvider(3, 2, 50);
            Assert.AreEqual(audio.Latency, audio.waveOut.DesiredLatency);
            Assert.AreEqual(audio.Buffers, audio.waveOut.NumberOfBuffers);
            audio.waveOut.Volume = 0;
            audio.Play();
            Assert.AreEqual(audio.waveOut.PlaybackState, NAudio.Wave.PlaybackState.Playing);
            audio.waveOut.Stop();
            Assert.AreEqual(audio.waveOut.PlaybackState, NAudio.Wave.PlaybackState.Stopped);
        }
    }
}
