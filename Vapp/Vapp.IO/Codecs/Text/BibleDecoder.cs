using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Codecs.Text
{
    public class BibleDecoder : DecoderBase<Bible>
    {
        private static Book ConvertToBook(string name, int order, StreamReader reader, Stream stream)
        {
            Book book = new Book(name, order);

            string verse = null;
            VerseToken vToken = VerseToken.Empty;

            try
            {
                while (!reader.EndOfStream)
                {
                    int peek = reader.Peek();
                    if (peek == 'B' || peek == 'b')
                        return book;

                    string line = reader.ReadLine();

                    if (line.StartsWith("        ") && !line.StartsWith("         "))
                        verse += " " + line.Trim();
                    else if (line.Length > 0)
                    {
                        if (verse != null)
                            book.VerseList.Add(vToken, new Verse(verse));

                        vToken = new VerseToken(int.Parse(line.Substring(0, 3)), int.Parse(line.Substring(4, 3)));
                        verse = line.Substring(8, line.Length - 8);
                    }
                }
            }
            catch
            {
                // Skip
            }

            return book;
        }

        public override Bible Decode(Stream stream)
        {
            Bible bible = new Bible();

            using (StreamReader reader = new StreamReader(stream))
            {
                int order = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.ToLower().StartsWith("book"))
                    {
                        string name = line.Substring(8, line.Length - 8);
                        Book book = ConvertToBook(name, order++, reader, stream);
                        bible.BookList.Add(name, book);
                    }
                }
            }

            return bible;
        }
    }
}
