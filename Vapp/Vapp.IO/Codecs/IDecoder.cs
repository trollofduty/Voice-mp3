using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vapp.IO.Codecs
{
    public interface IDecoder<T>
    {
        bool TryDecode(Stream stream, out T output);

        T Decode(Stream stream);
    }
}
