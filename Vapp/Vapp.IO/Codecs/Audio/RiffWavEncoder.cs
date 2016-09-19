using Vapp.Media.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vapp.IO.Codecs.Audio.Wav;

namespace Vapp.IO.Codecs.Audio
{
    public class RiffWavEncoder : EncoderBase<Waveformat>
    {
        public void Encode(Stream stream, Waveformat input)
        {
            WaveFile wav = new WaveFile();

            wav.FmtHeader = new FmtChunk();
            wav.RiffHeader = new RiffChunk();
            wav.DataChunk = new WaveDataChunk();

            wav.FmtHeader.AudioFormat = input.AudioFormat;
            wav.FmtHeader.SampleRate = input.SampleRate;
            wav.FmtHeader.ByteRate = input.ByteRate;
            wav.FmtHeader.BlockAlign = input.BlockAlign;
            wav.FmtHeader.BitsPerSample = input.BitsPerSample;
            wav.FmtHeader.NumChannels = (short) input.Channels.Length;
            wav.FmtHeader.Size = 16;

            int bytesPerSample = wav.FmtHeader.BitsPerSample / 8;

            List<byte> data = new List<byte>();
            for (int index = 0; index < input.Channels[0].Samples.Count; index++)
            {
                for (int cIndex = 0; cIndex < input.Channels.Length; cIndex++)
                {
                    if (index < input.Channels[cIndex].Samples.Count)
                    {
                        Sample sample = input.Channels[cIndex].Samples[index];
                        data.AddRange(sample.Data);
                    }
                }
            }

            wav.DataChunk.Data = data.ToArray();
            wav.DataChunk.Size = data.Count;

            wav.RiffHeader.Size = 36 + data.Count;
            wav.RiffHeader.Format = BitConverter.ToInt32(Encoding.UTF8.GetBytes("WAVE"), 0);
            wav.Export(stream);
        }
    }
}
