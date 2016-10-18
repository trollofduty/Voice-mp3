using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Codecs.Text
{
    public class Bible
    {
        public Dictionary<string, Book> BookList { get; set; } = new Dictionary<string, Book>();
    }
}
