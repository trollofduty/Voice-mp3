﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO
{
    public class EncoderRegisterService<T> : RegisterService<string, IEncoder<T>>
    {
        #region Methods

        public bool TryEncode(T data, Stream stream, string format = "")
        {
            if (format != null && format.Length > 0)
            {
                if (!this.Cache.ContainsKey(format))
                    return false;
                else
                    return this.Cache[format].TryEncode(stream, data);
            }
            else
            {
                foreach (IEncoder<T> encoder in this.Cache.Select(t => t.Value))
                {
                    if (encoder.TryEncode(stream, data))
                        return true;
                }
            }
            return false;
        }

        #endregion
    }
}
