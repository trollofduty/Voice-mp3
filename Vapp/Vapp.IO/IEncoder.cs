using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO
{
    public interface IEncoder<T>
    {
        bool TryEncode(Stream stream, T input);

        void Encode(Stream stream, T input);
    }
}
