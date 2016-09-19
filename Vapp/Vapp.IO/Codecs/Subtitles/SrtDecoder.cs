using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Media;

namespace Vapp.IO.Codecs.Subtitles
{
    public class SrtDecoder : DecoderBase<IEnumerable<Subtitle>>
    {
        public override IEnumerable<Subtitle> Decode(Stream stream)
        {
            List<Subtitle> subtitles = new List<Subtitle>();
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    int sOrder;
                    if (!reader.EndOfStream && int.TryParse(line, out sOrder))
                    {
                        IEnumerable<string> sOffsets = reader.ReadLine().Replace("--", "").Split('>').Select(s => s.Trim());
                        TimeSpan start = TimeSpan.ParseExact(sOffsets.ElementAt(0), @"hh\:mm\:ss\,fff", null);
                        TimeSpan end = TimeSpan.ParseExact(sOffsets.ElementAt(1), @"hh\:mm\:ss\,fff", null);

                        bool done = false;
                        string text = "";
                        while (!reader.EndOfStream && !done)
                        {
                            string iLine = reader.ReadLine();

                            if (iLine != null && iLine.Length > 0)
                                text += iLine;
                            else
                                done = true;
                        }

                        subtitles.Add(new Subtitle(text, start, end));
                    }
                }
            }
            return subtitles;
        }
    }
}
