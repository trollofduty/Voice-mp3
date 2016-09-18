using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vapp.Core
{
    public interface IDeepCloneable<T>
    {
        T DeepClone();
    }
}
