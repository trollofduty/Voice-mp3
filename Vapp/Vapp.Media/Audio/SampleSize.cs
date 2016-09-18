namespace Vapp.Media.Audio
{
    /// <summary>
    /// SampleSize type
    /// </summary>
    public enum SampleSize
    {
        /// <summary>
        /// Unspecfied
        /// </summary>
        Null,

        /// <summary>
        /// 8 Bits (1 Byte)
        /// </summary>
        Format8 = 1,

        /// <summary>
        /// 16 Bits (2 Bytes)
        /// </summary>
        Format16 = 2,

        /// <summary>
        /// 24 Bits (3 Bytes)
        /// </summary>
        Format24 = 3,

        /// <summary>
        /// 32 Bits (4 Bytes)
        /// </summary>
        Format32 = 4,

        /// <summary>
        /// 64 Bits (8 Bytes)
        /// </summary>
        Format64 = 8
    }
}