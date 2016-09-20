using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Core;

namespace Vapp.Media.Audio
{
    /// <summary>
    /// PCM Class wrapper
    /// </summary>
    public class Waveformat : IDeepCloneable<Waveformat>
    {
        #region Properties

        /// <summary>
        /// Array of PCM audio channels
        /// </summary>
        public Channel[] Channels { get; set; }


        /// <summary>
        /// Format of PCM audio
        /// </summary>
        public short AudioFormat { get; set; }

        /// <summary>
        /// Samples per second
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// Bytes per second (SampleRate * BlockAlign)
        /// </summary>
        public int ByteRate { get; set; }

        /// <summary>
        /// Size of Sample block in bytes
        /// </summary>
        public short BlockAlign { get; set; }

        /// <summary>
        /// Bits per sample
        /// </summary>
        public short BitsPerSample { get; set; }

        #endregion

        #region Methods


        /// <summary>
        /// Duals multiple channel pcm's to mono channel by taking
        /// the averages of all channels.
        /// </summary>
        public void ConvertToMono()
        {
            Channel channel = new Channel(0);
            int max = this.Channels.Max(c => c.Samples.Count);
            
            for (int index = 0; index < max; index++)
            {
                int count = 0;
                double total = 0;

                foreach (Channel iChannel in this.Channels)
                {
                    if (index < iChannel.Samples.Count())
                    {
                        count++;
                        total += iChannel.Samples[index].Value;
                    }
                }

                Sample sample = new Sample((SampleSize) (this.BitsPerSample / 8));
                sample.Value = total / count;
                channel.Samples.Add(sample);
            }
            this.Channels = new Channel[1] { channel };
        }

        /// <summary>
        /// Returns a data clone of the Waveformat class object
        /// </summary>
        /// <returns>Cloned Waveformat class instance</returns>
        public Waveformat DeepClone()
        {
            Waveformat clone = new Waveformat();
            clone.Channels = new Channel[this.Channels.Length];

            for (int index = 0; index < this.Channels.Length; index++)
                clone.Channels[index] = this.Channels[index].DeepClone();

            clone.AudioFormat = this.AudioFormat;
            clone.SampleRate = this.SampleRate;
            clone.ByteRate = this.ByteRate;
            clone.BlockAlign = this.BlockAlign;
            clone.BitsPerSample = this.BitsPerSample;

            return clone;
        }

        #endregion
    }
}