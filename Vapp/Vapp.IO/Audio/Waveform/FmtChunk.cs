using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Extensions;

namespace Vapp.IO.Audio.Waveform
{
    internal class FmtChunk : GenericChunk
    {
        #region Properties

        public short AudioFormat { get; set; }

        public short NumChannels { get; set; }

        public int SampleRate { get; set; }

        public int ByteRate { get; set; }

        public short BlockAlign { get; set; }

        public short BitsPerSample { get; set; }

        public short ExtraParamSize { get; set; }

        public byte[] ExtraParams { get; set; }

        #endregion

        #region Methods

        public static new FmtChunk Import(Stream stream)
        {
            FmtChunk chunk = new FmtChunk();
            chunk.Size = stream.ReadInt();
            chunk.AudioFormat = stream.ReadShort();
            chunk.NumChannels = stream.ReadShort();
            chunk.SampleRate = stream.ReadInt();
            chunk.ByteRate = stream.ReadInt();
            chunk.BlockAlign = stream.ReadShort();
            chunk.BitsPerSample = stream.ReadShort();

            if (chunk.Size > 16)
            {
                chunk.ExtraParamSize = stream.ReadShort();
                chunk.ExtraParams = new byte[chunk.ExtraParamSize];
                stream.Read(chunk.ExtraParams, 0, chunk.ExtraParamSize);
            }

            return chunk;
        }

        public void Export(Stream stream)
        {
            stream.WriteInt(BitConverter.ToInt32(Encoding.UTF8.GetBytes("fmt "), 0));
            stream.WriteInt(this.Size);
            stream.WriteShort(this.AudioFormat);
            stream.WriteShort(this.NumChannels);
            stream.WriteInt(this.SampleRate);
            stream.WriteInt(this.ByteRate);
            stream.WriteShort(this.BlockAlign);
            stream.WriteShort(this.BitsPerSample);

            if (this.Size > 16)
            {
                stream.WriteShort(this.ExtraParamSize);
                stream.Write(this.ExtraParams, 0, this.ExtraParamSize);
            }
        }

        #endregion
    }
}
