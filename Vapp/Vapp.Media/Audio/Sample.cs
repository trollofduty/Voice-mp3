using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Core;

namespace Vapp.Media.Audio
{
    public class Sample : IDeepCloneable<Sample>
    {
        #region Constructor

        public Sample(int sampleSize)
        {
            this.Data = new byte[sampleSize];
        }

        #endregion

        #region Properties

        public byte[] Data { get; private set; }

        #endregion

        #region Methods

        public Sample DeepClone()
        {
            Sample clone = new Sample(this.Data.Length);

            for (int index = 0; index < this.Data.Length; index++)
                clone.Data[index] = this.Data[index];

            return clone;
        }

        #endregion
    }
}
