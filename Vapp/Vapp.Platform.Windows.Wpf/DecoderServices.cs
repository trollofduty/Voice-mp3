using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.IO.Codecs;
using Vapp.IO.Codecs.Audio;
using Vapp.Media.Audio;

namespace Vapp.Platform.Windows.Wpf
{
    static class DecoderServices
    {
        private static DecoderRegisterService<Waveformat> audioDecoderRegisterService;
        public static DecoderRegisterService<Waveformat> AudioDecoderRegisterService
        {
            get
            {
                if (audioDecoderRegisterService == null)
                {
                    audioDecoderRegisterService = new DecoderRegisterService<Waveformat>();
                    audioDecoderRegisterService.Hook(new RiffWavDecoder(), "wav");
                }

                return audioDecoderRegisterService;
            }
        }
    }
}
