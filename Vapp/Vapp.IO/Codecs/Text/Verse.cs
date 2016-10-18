using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Codecs.Text
{
    public class Verse
    {
        public Verse(string data)
        {
            this.Data = data;
        }

        public string Data { get; set; }

        public override string ToString()
        {
            return this.Data;
        }
    }
}
