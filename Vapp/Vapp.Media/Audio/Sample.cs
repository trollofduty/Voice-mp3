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

        private const int Format8 = 1;
        private const int Format16 = 2;
        private const int Format24 = 3;
        private const int Format32 = 4;
        private const int Format64 = 8;

        public byte[] Data { get; private set; }
        
        public double Value
        {
            get { return this.GetValue(); }
            set { this.SetValue(value); }
        }

        #endregion

        #region Methods

        private void SetValue(double value)
        {
            switch (this.Data.Length)
            {
                default:
                    throw new FormatException();
                case Format8:
                    this.Data[0] = (byte) (((value + 1.0) / 2.0) * 0xF);
                    break;
                case Format16:
                    short sValue = (short) (value >= 0 ? value * short.MaxValue : value * short.MinValue);
                    this.Data = BitConverter.GetBytes(sValue);
                    break;
                case Format32:
                    int iValue = (int) (value >= 0 ? value * int.MaxValue : value * int.MinValue);
                    this.Data = BitConverter.GetBytes(iValue);
                    break;
                case Format64:
                    int lValue = (int) (value >= 0 ? value * long.MaxValue : value * long.MinValue);
                    this.Data = BitConverter.GetBytes(lValue);
                    break;
            }
        }

        private double GetValue()
        {
            switch (this.Data.Length)
            {
                default:
                    throw new FormatException();
                case Format8:
                    return ((this.Data[0] / (double) 0xF) * 2.0) - 1.0;
                case Format16:
                    short sValue = BitConverter.ToInt16(this.Data, 0);
                    return sValue >= 0 ? sValue / (double) short.MaxValue : sValue / (double) short.MinValue;
                case Format32:
                    int iValue = BitConverter.ToInt32(this.Data, 0);
                    return iValue >= 0 ? iValue / (double) int.MaxValue : iValue / (double) int.MinValue;
                case Format64:
                    long lValue = BitConverter.ToInt64(this.Data, 0);
                    return lValue >= 0 ? lValue / (double) long.MaxValue : lValue / (double) long.MinValue;
            }
        }

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
