// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.IO;

namespace HMI.Infrastructure.Common
{
    public static class StreamExtensions
    {
        public static void WriteAllBytes(this Stream source, Stream destination)
        {
            byte[] array = new byte[1048576];
            int count;
            while ((count = source.Read(array, 0, array.Length)) > 0)
            {
                destination.Write(array, 0, count);
            }
        }
    }
}
