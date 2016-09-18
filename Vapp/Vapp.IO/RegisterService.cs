using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.IO
{
    public abstract class RegisterService<K, V>
    {
        #region Properties

        protected Dictionary<K, V> Cache { get; set; } = new Dictionary<K, V>();

        #endregion

        #region Methods

        public void Hook(V value, params K[] keys)
        {
            foreach (K key in keys)
                this.Cache.Add(key, value);
        }

        public void Hook(K key, V value)
        {
            this.Cache.Add(key, value);
        }

        public void Unhook(K key)
        {
            this.Cache.Remove(key);
        }

        public bool Contains(K key)
        {
            return this.Cache.ContainsKey(key);
        }

        public bool Contains(V value)
        {
            return this.Cache.ContainsValue(value);
        }

        public void Clear()
        {
            this.Cache.Clear();
        }

        #endregion
    }
}
