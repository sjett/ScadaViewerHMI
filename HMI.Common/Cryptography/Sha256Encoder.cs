// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Security.Cryptography;
using System.Text;

namespace HMI.Common.Cryptography
{
    public static class Sha256Encoder
    {
        public static string EncodeText(string text)
		{
			byte[] bytes = new UnicodeEncoding().GetBytes(text);
			byte[] hash = new SHA256Managed().ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			int index = 0;
			while (index < hash.Length)
			{
				byte hashByte = hash[index];
				stringBuilder.Append(string.Format("{0:x2}", hashByte));
				index++;
			}
			return stringBuilder.ToString();
		}
    }
}
