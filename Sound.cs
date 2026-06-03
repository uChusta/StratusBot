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
            // play the welcome sound
            try
            {
                using var audio = new AudioFileReader("Welcome.wav") { Volume = 0.25f }; // 25% volume
                using var output = new WaveOutEvent();
                output.Init(audio);
                output.Play();

                // wait for sound to finish 
                while (output.PlaybackState == PlaybackState.Playing)
                {
                    //Thread.Sleep(0);
                }
            }
            catch
            {
                // fail silently if audio unavailable
            }
        }
    }
