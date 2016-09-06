using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Extensions;

namespace Vapp.IO.Audio.Waveform
{
    internal class GenericChunk
    {
        #region Properties

        public int Size { get; set; }

        #endregion

        #region Methods

        public static GenericChunk Import(Stream stream)
        {
            GenericChunk chunk = new GenericChunk();
            chunk.Size = stream.ReadInt();
            stream.Position += chunk.Size;
            return chunk;
        }

        #endregion
    }
}
