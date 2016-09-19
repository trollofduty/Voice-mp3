using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Media;

namespace Vapp.IO.Codecs.Subtitles
{
    public class SrtEncoder : EncoderBase<IEnumerable<Subtitle>>
    {
        public override void Encode(Stream stream, IEnumerable<Subtitle> input)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                for (int index = 0; index < input.Count(); index++)
                {
                    writer.WriteLine(index + 1);
                    Subtitle sub = input.ElementAt(index);
                    writer.WriteLine(string.Format("{0} --> {1}", sub.Start.ToString(@"hh\:mm\:ss\,fff"), sub.End.ToString()));
                    writer.WriteLine("");
                }
            }
        }
    }
}
