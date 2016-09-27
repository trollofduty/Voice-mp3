using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Core;

namespace Vapp.IO.Codecs
{
    public class DecoderRegisterService<T> : RegisterService<string, DecoderBase<T>>
    {
        #region Methods

        public bool TryDecode(out T result, Stream stream, string format = "")
        {
            result = default(T);
            if (format != null && format.Length > 0)
            {
                if (!this.Cache.ContainsKey(format))
                    return false;
                else
                    return this.Cache[format].TryDecode(stream, out result);
            }
            else
            {
                foreach (DecoderBase<T> decoder in this.Cache.Select(t => t.Value))
                {
                    if (decoder.TryDecode(stream, out result))
                        return true;
                }
            }
            return false;
        }

        #endregion
    }
}
