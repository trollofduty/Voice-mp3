using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vapp.Core
{
    /// <summary>
    /// Data level cloneable object type interface
    /// </summary>
    /// <typeparam name="T">Cloned object return type</typeparam>
    public interface IDeepCloneable<T>
    {
        /// <summary>
        /// Creates clone down to data level
        /// </summary>
        /// <returns>Cloned object return type</returns>
        T DeepClone();
    }
}