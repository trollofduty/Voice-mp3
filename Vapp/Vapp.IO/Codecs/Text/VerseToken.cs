using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Codecs.Text
{
    public struct VerseToken
    {
        public VerseToken(int chpt, int verse)
        {
            this.ChapterNumber = chpt;
            this.VerseNumber = verse;
        }

        public int ChapterNumber { get; set; }

        public int VerseNumber { get; set; }

        public static VerseToken Empty
        {
            get { return new VerseToken(0, 0); }
        }

        public override string ToString()
        {
            return this.ChapterNumber.ToString("000") + ":" + this.VerseNumber.ToString("000");
        }
    }
}
