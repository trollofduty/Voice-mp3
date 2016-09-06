using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Audio
{
    public class Sound
    {
        #region Properties

        public Channel[] Channels { get; set; }

        public short AudioFormat { get; set; }

        public int SampleRate { get; set; }

        public int ByteRate { get; set; }

        public short BlockAlign { get; set; }

        public short BitsPerSample { get; set; }

        #endregion
    }
}
