using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Core
{
    public class RegisterService<K, V>
    {
        #region Properties

        protected Dictionary<K, V> Cache { get; set; } = new Dictionary<K, V>();

        public V this[K key]
        {
            get { return this.GetValue(key); }
            set { this.SetValue(key, value); }
        }

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

        public void Unhook(params K[] keys)
        {
            foreach (K key in keys)
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

        public V GetValue(K key)
        {
            return this.Cache[key];
        }

        public void SetValue(K key, V value)
        {
            this.Cache[key] = value;
        }

        public void Clear()
        {
            this.Cache.Clear();
        }

        public IEnumerable<KeyValuePair<K, V>> AsEnumerable()
        {
            return this.Cache.AsEnumerable();
        }

        #endregion
    }
}
