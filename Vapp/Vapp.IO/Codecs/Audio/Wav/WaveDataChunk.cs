using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Extensions;

namespace Vapp.IO.Codecs.Audio.Wav
{
    internal class WaveDataChunk : GenericChunk
    {
        #region Properties

        public byte[] Data { get; set; }

        #endregion

        #region Methods

        public static new WaveDataChunk Import(Stream stream)
        {
            WaveDataChunk chunk = new WaveDataChunk();
            chunk.Size = stream.ReadInt();
            chunk.Data = new byte[chunk.Size];
            stream.Read(chunk.Data, 0, chunk.Size);
            return chunk;
        }

        public void Export(Stream stream)
        {
            stream.WriteInt(BitConverter.ToInt32(Encoding.UTF8.GetBytes("data"), 0));
            stream.WriteInt(this.Size);
            stream.Write(this.Data, 0, this.Size);
        }

        #endregion
    }
}
