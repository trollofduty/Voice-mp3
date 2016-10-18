using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Codecs.Text
{
    public class BookEncoder : EncoderBase<Book>
    {
        public override void Encode(Stream stream, Book input)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                IEnumerable<VerseToken> tokens = input.VerseList.Select(p => p.Key).OrderBy(t => t.ChapterNumber).ThenBy(t => t.VerseNumber);
                writer.WriteLine(string.Format("{0}_{1}", input.Order, input.Name));

                foreach (VerseToken token in tokens)
                    writer.WriteLine(token.ToString() + " " + input.VerseList[token].ToString());
            }
        }
    }
}
