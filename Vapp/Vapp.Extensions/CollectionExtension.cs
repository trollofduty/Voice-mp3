using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Extensions
{
    /// <summary>
    /// A static class wrapper of method extensions for the generic Collection class type
    /// </summary>
    public static class CollectionExtension
    {

        /// <summary>
        /// Adds a list of generic items to the Collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="host">Collection object</param>
        /// <param name="items">List to add to the collection</param>
        public static void AddRange<T>(this Collection<T> host, IEnumerable<T> items)
        {
            foreach (T item in items)
                host.Add(item);
        }
    }
}