using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Extensions;

namespace Vapp.IO.Audio.Waveform
{
    internal class WaveFile
    {
        #region Properties

        public RiffChunk RiffHeader { get; set; }

        public FmtChunk FmtHeader { get; set; }

        public WaveDataChunk DataChunk { get; set; }

        #endregion

        #region Methods

        public static WaveFile Import(Stream stream)
        {
            WaveFile wav = new WaveFile();

            while(stream.Position < stream.Length)
            {
                byte[] id = new byte[4];
                stream.Read(id, 0, 4);
                string idn = Encoding.UTF8.GetString(id, 0, 4);
                switch (idn)
                {
                    default:
                        GenericChunk.Import(stream);
                        break;
                    case "RIFF":
                        wav.RiffHeader = RiffChunk.Import(stream);
                        break;
                    case "fmt ":
                        wav.FmtHeader = FmtChunk.Import(stream);
                        break;
                    case "data":
                        wav.DataChunk = WaveDataChunk.Import(stream);
                        break;
                }
            }

            return wav;
        }

        public bool TryExport(Stream stream)
        {
            try
            {
                this.Export(stream);
                return true;
            }
            catch
            {
                // Skip
            }
            return false;
        }

        public void Export(Stream stream)
        {
            this.RiffHeader.Export(stream);
            this.FmtHeader.Export(stream);
            this.DataChunk.Export(stream);
        }

        #endregion
    }
}
