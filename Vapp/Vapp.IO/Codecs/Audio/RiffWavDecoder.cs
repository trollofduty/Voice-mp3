using Vapp.Media.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.IO.Codecs.Audio.Wav;

namespace Vapp.IO.Codecs.Audio
{
    public class RiffWavDecoder : DecoderBase<Waveformat>
    {
        private AudioFormat GetFormat(int input)
        {
            switch (input)
            {
                default:
                    return AudioFormat.Unknown;
                case 1:
                    return AudioFormat.Pcm;
            }
        }

        public override Waveformat Decode(Stream stream)
        {
            WaveFile wav = WaveFile.Import(stream);
            Waveformat sound = new Waveformat();

            sound.AudioFormat = this.GetFormat(wav.FmtHeader.AudioFormat);
            sound.SampleRate = wav.FmtHeader.SampleRate;
            sound.BitsPerSample = wav.FmtHeader.BitsPerSample;

            if (sound.ByteRate != wav.FmtHeader.ByteRate || sound.BlockAlign != wav.FmtHeader.BlockAlign)
                throw new FormatException();

            int bytesPerSample = wav.FmtHeader.BitsPerSample / 8;

            Channel[] channels = new Channel[wav.FmtHeader.NumChannels];
            for (int index = 0; index < channels.Length; index++)
                channels[index] = new Channel(index);

            int chnlIndex = 0;
            int chnlCount = 0;

            for (int index = 0; index < wav.DataChunk.Size; index += bytesPerSample)
            {
                Sample sample = new Sample((SampleSize) bytesPerSample);

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
