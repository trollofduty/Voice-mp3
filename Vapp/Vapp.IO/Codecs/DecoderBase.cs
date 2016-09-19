using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vapp.IO.Codecs
{
    public abstract class DecoderBase<T>
    {
        public bool TryDecode(Stream stream, out T output)
        {
            try
            {
                output = this.Decode(stream);
                return true;
            }
            catch
            {
                output = default(T);
            }
            return false;
        }

        public abstract T Decode(Stream stream);
    }
}
