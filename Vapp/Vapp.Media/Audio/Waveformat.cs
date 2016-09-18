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