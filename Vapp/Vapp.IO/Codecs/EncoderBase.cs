using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO.Codecs
{
    public abstract class EncoderBase<T>
    {
        public bool TryEncode(Stream stream, T input)
        {
            try
            {
                this.Encode(stream, input);
                return true;
            }
            catch
            {
                // Skip
            }
            return false;
        }

        public abstract void Encode(Stream stream, T input);
    }
}
