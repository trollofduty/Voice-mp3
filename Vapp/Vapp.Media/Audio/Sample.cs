using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Core;

namespace Vapp.Media.Audio
{
    /// <summary>
    /// Audio data sample
    /// </summary>
    public class Sample : IDeepCloneable<Sample>
    {
        #region Constructor

        /// <summary>
        /// Sample constructor
        /// </summary>
        /// <param name="sampleSize">Sample size in bytes</param>
        public Sample(SampleSize sampleSize)
            : this((int) sampleSize)
        {
            // Skip
        }

        /// <summary>
        /// Sample constructor
        /// </summary>
        /// <param name="sampleSize">Sample size in bytes</param>
        protected Sample(int sampleSize)
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

        /// <summary>
        /// Raw audio data
        /// </summary>
        public byte[] Data { get; private set; }
        
        /// <summary>
        /// Value of sample ranging from +1.0 to -1.0
        /// </summary>
        public double Value
        {
            get { return this.GetValue(); }
            set { this.SetValue(value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Stores value in the form of the Data array
        /// </summary>
        /// <param name="value">Value of sample ranging from +1.0 to -1.0</param>
        protected void SetValue(double value)
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

        /// <summary>
        /// Gets stored value in the data array in the form of a double
        /// </summary>
        /// <returns>Value of sample ranging from +1.0 to -1.0</returns>
        protected double GetValue()
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

        /// <summary>
        /// Converts sample size format to specified sample size, if
        /// sample size is not supported will throw a FormatException.
        /// Good idea to surround with a try catch block if you don't
        /// know the specified sample size
        /// </summary>
        /// <param name="sampleSize">Size of sample in bytes</param>
        public void ConvertTo(SampleSize sampleSize)
        {
            if (this.Data.Length == (int) sampleSize)
                return;
            else
            {
                double value = this.GetValue();
                this.Data = new byte[(int) sampleSize];
                this.SetValue(value);
            }
            throw new FormatException();
        }

        /// <summary>
        /// Data clone of Sample class
        /// </summary>
        /// <returns>Cloned Sample class instance</returns>
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