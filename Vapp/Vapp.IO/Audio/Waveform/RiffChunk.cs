using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Extensions;

namespace Vapp.IO.Audio.Waveform
{
    internal class RiffChunk : GenericChunk
    {
        #region Properties

        public int Format { get; set; }

        #endregion

        #region Methods

        public static new RiffChunk Import(Stream stream)
        {
            RiffChunk chunk = new RiffChunk();
            chunk.Size = stream.ReadInt();
            chunk.Format = stream.ReadInt();
            return chunk;
        }

        public void Export(Stream stream)
        {
            stream.WriteInt(BitConverter.ToInt32(Encoding.UTF8.GetBytes("RIFF"), 0));
            stream.WriteInt(this.Size);
            stream.WriteInt(this.Format);
        }

        #endregion
    }
}
