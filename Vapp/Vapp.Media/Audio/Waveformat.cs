using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Core;

namespace Vapp.Media.Audio
{
    public class Waveformat : IDeepCloneable<Waveformat>
    {
        #region Properties

        public Channel[] Channels { get; set; }

        public short AudioFormat { get; set; }

        public int SampleRate { get; set; }

        public int ByteRate { get; set; }

        public short BlockAlign { get; set; }

        public short BitsPerSample { get; set; }

        #endregion

        #region Methods

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
