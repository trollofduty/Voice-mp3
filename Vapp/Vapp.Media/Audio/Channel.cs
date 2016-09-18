using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Core;

namespace Vapp.Media.Audio
{
    /// <summary>
    /// Holds a list of Samples
    /// </summary>
    public class Channel : IDeepCloneable<Channel>
    {
        #region Constructor

        /// <summary>
        /// Channel constructor
        /// </summary>
        /// <param name="id">Channel type, i.e 0 (Left or Mono), 1 (Right), etc etc</param>
        public Channel(int id)
        {
            this.Id = id;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Channel type, i.e 0 (Left or Mono), 1 (Right), etc etc
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// List of samples
        /// </summary>
        public List<Sample> Samples { get; set; } = new List<Sample>();

        #endregion

        #region Methods

        /// <summary>
        /// Data clone of Channel class
        /// </summary>
        /// <returns>Cloned Channel class instance</returns>
        public Channel DeepClone()
        {
            Channel clone = new Channel(this.Id);
            clone.Samples.AddRange(this.Samples.Select(s => s.DeepClone()));
            return clone;
        }

        #endregion
    }
}