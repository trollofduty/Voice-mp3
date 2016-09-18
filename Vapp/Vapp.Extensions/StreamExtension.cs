using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vapp.Extensions
{
    /// <summary>
    /// A static class wrapper of method extensions for the generic Collection class type
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// Writes an integer to the stream and moves the stream position
        /// </summary>
        /// <param name="host">Stream being written to</param>
        /// <param name="value">Integer to write</param>
        public static void WriteInt(this Stream host, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            host.Write(bytes, 0, 4);
        }

        /// <summary>
        /// Reads an integer from the stream and moves the stream position
        /// forward by four bytes
        /// </summary>
        /// <param name="host">Stream being read from</param>
        /// <returns>Integer read from stream</returns>
        public static int ReadInt(this Stream host)
        {
            byte[] bytes = new byte[4];
            host.Read(bytes, 0, 4);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Writes an short to the stream and moves the stream position
        /// </summary>
        /// <param name="host">Stream being written to</param>
        /// <param name="value">Short to write</param>
        public static void WriteShort(this Stream host, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            host.Write(bytes, 0, 2);
        }

        /// <summary>
        /// Reads an short from the stream and moves the stream position
        /// forward by four bytes
        /// </summary>
        /// <param name="host">Stream being read from</param>
        /// <returns>Short read from stream</returns>
        public static short ReadShort(this Stream host)
        {
            byte[] bytes = new byte[2];
            host.Read(bytes, 0, 2);
            return BitConverter.ToInt16(bytes, 0);
        }
    }
}