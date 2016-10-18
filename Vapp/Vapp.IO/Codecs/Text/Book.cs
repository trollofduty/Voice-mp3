using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Codecs.Text
{
    public class Book
    {
        internal Book(string name, int order)
        {
            this.Name = name;
            this.Order = order;
        }

        public string Name { get; set; }

        public int Order { get; set; }

        public Dictionary<VerseToken, Verse> VerseList { get; set; } = new Dictionary<VerseToken, Verse>();
    }
}
