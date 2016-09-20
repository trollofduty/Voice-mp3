using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Media.Audio;

namespace Vapp.Media.Gaps
{
    public class GapFormat
    {
        #region Properties

        public List<Pause> Pauses { get; private set; } = new List<Pause>();

        #endregion

        #region Methods

        public static GapFormat CreateFrom(Waveformat pcm, double threshold = 0.0, double minPause = 0.2)
        {
            GapFormat pauseFormat = new GapFormat();

            if (pcm.Channels.Length > 1)
            {
                pcm = pcm.DeepClone();
                pcm.ConvertToMono();
            }

            bool wasAbove = pcm.Channels[0].Samples[0].Value > threshold ? true : false;
            bool wasBelow = pcm.Channels[0].Samples[0].Value < threshold ? true : false;
            bool wasThreshold = !wasAbove && !wasBelow ? true : false;

            bool wasCrossed = false;

            double sMinPause = minPause * pcm.SampleRate;
            double pauseCount = 0.0;

            double pauseStart = 0.0;

            for (int index = 1; index < pcm.Channels[0].Samples.Count; index++)
            {
                double value = pcm.Channels[0].Samples[index].Value;

                bool isAbove = value > threshold ? true : false;
                bool isBelow = value < threshold ? true : false;
                bool isThreshold = !isAbove && !isBelow ? true : false;

                if (wasCrossed)
                {
                    pauseCount++;
                    if ((wasBelow || wasThreshold) && isAbove)
                    {
                        if (pauseCount >= sMinPause)
                            pauseFormat.Pauses.Add(new Pause(TimeSpan.FromSeconds(pauseStart / pcm.SampleRate), TimeSpan.FromSeconds((index - 1) / pcm.SampleRate)));

                        wasCrossed = false;
                    }
                }
                if (wasAbove && isBelow)
                {
                    pauseCount = 0;
                    pauseStart = index;
                    wasCrossed = true;
                }

                wasAbove = isAbove;
                wasBelow = isBelow;
                wasThreshold = isThreshold;
            }

            return pauseFormat;
        }

        #endregion
    }
}
