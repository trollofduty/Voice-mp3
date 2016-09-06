using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vapp.Extensions
{
    public static class StreamExtension
    {
        public static void WriteInt(this Stream host, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            host.Write(bytes, 0, 4);
        }

        public static int ReadInt(this Stream host)
        {
            byte[] bytes = new byte[4];
            host.Read(bytes, 0, 4);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void WriteShort(this Stream host, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            host.Write(bytes, 0, 2);
        }

        public static short ReadShort(this Stream host)
        {
            byte[] bytes = new byte[2];
            host.Read(bytes, 0, 2);
            return BitConverter.ToInt16(bytes, 0);
        }
    }
}
