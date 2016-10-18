using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Codecs.Text
{
    public class BookDecoder : DecoderBase<Book>
    {
        private static void ReadVerse(Book book, string line)
        {
            VerseToken vToken = new VerseToken(int.Parse(line.Substring(0, 3)), int.Parse(line.Substring(4, 3)));
            Verse verse = new Verse(line.Substring(8, line.Length - 8));
            book.VerseList.Add(vToken, verse);
        }

        public override Book Decode(Stream stream)
        {
            Book book = null;
            using (StreamReader reader = new StreamReader(stream))
            {
                string[] split = reader.ReadLine().Split('_');
                book = new Book(split.Last(), int.Parse(split.First()));

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Length > 0)
                        ReadVerse(book, line);
                }
            }
            return book;
        }
    }
}
