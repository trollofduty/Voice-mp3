using Vapp.Audio;
using Vapp.IO.Audio.Waveform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Audio
{
    public class RiffWavDecoder : IDecoder<Track>
    {
        public bool TryDecode(Stream stream, out Track output)
        {
            try
            {
                output = this.Decode(stream);
                return true;
            }
            catch
            {
                output = null;
            }
            return false;
        }

        public Track Decode(Stream stream)
        {
            WaveFile wav = WaveFile.Import(stream);
            Track sound = new Track();

            sound.AudioFormat = wav.FmtHeader.AudioFormat;
            sound.SampleRate = wav.FmtHeader.SampleRate;
            sound.ByteRate = wav.FmtHeader.ByteRate;
            sound.BlockAlign = wav.FmtHeader.BlockAlign;
            sound.BitsPerSample = wav.FmtHeader.BitsPerSample;

            int bytesPerSample = wav.FmtHeader.BitsPerSample / 8;

            Channel[] channels = new Channel[wav.FmtHeader.NumChannels];
            for (int index = 0; index < channels.Length; index++)
                channels[index] = new Channel(index);

            int chnlIndex = 0;
            int chnlCount = 0;

            for (int index = 0; index < wav.DataChunk.Size; index += bytesPerSample)
            {
                Sample sample = new Sample();
                sample.Data = new byte[bytesPerSample];

                for (int dataIndex = 0; dataIndex < sample.Data.Length; dataIndex++)
                    sample.Data[dataIndex] = wav.DataChunk.Data[index + dataIndex];

                channels[chnlIndex].Samples.Add(sample);
                chnlCount++;
                if (chnlCount >= wav.FmtHeader.BlockAlign)
                {
                    chnlCount = 0;
                    chnlIndex = (chnlIndex + 1) % channels.Length;
                }
            }

            sound.Channels = channels;
            return sound;
        }
    }
}
