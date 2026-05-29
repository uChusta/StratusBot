using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NAudio.Wave;

    internal class Sound
    {
        public void PlaySound()
        {
            try
            {
                using var audio = new AudioFileReader("Welcome.wav") { Volume = 0.25f }; // 25% volume
                using var output = new WaveOutEvent();
                output.Init(audio);
                output.Play();
                while (output.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
            catch
            {
                // fail silently if audio unavailable
            }
        }
    }
