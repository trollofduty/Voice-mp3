using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Core;

namespace Vapp.Media.Audio
{
    public class Channel : IDeepCloneable<Channel>
    {
        #region Constructor

        public Channel(int id)
        {
            this.Id = id;
        }

        #endregion

        #region Properties

        public int Id { get; }

        public List<Sample> Samples { get; set; } = new List<Sample>();

        #endregion

        #region Methods

        public Channel DeepClone()
        {
            Channel clone = new Channel(this.Id);
            clone.Samples.AddRange(this.Samples.Select(s => s.DeepClone()));
            return clone;
        }

        #endregion
    }
}
