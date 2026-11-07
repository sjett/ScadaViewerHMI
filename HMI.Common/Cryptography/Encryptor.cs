// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HMI.Common.Cryptography
{
    public static class Encryptor
    {
        private const int LegacyKeyCycleLength = 259;
        private const string LegacySalt = "6bd93557-3925-4b91-a577-5807b696df4f";

        /// <summary>
        /// 解码使用旧版 XOR 格式和 A-P 十六进制字符集保存的字符串。
        /// </summary>
        /// <param name="encodedString">旧格式编码后的字符串。</param>
        /// <param name="obfuscationKey">与编码时相同的密钥字节。</param>
        /// <returns>解码后的字符串。</returns>
        public static string Decode(string encodedString, byte[] obfuscationKey)
        {
            ArgumentNullException.ThrowIfNull(encodedString);
            ArgumentNullException.ThrowIfNull(obfuscationKey);

            var encodedBytes = DecodeLegacyHex(encodedString);
            var decodedBytes = ApplyLegacyXorTransform(encodedBytes, obfuscationKey);
            return Encoding.Unicode.GetString(decodedBytes);
        }

        /// <summary>
        /// 使用旧版 XOR 格式和 A-P 十六进制字符集编码字符串。
        /// </summary>
        /// <param name="decodedString">需要编码的字符串。</param>
        /// <param name="obfuscationKey">解码时使用的密钥字节。</param>
        /// <returns>旧格式编码后的字符串。</returns>
        public static string Encode(string decodedString, byte[] obfuscationKey)
        {
            ArgumentNullException.ThrowIfNull(decodedString);
            ArgumentNullException.ThrowIfNull(obfuscationKey);

            var sourceBytes = Encoding.Unicode.GetBytes(decodedString);
            var encodedBytes = ApplyLegacyXorTransform(sourceBytes, obfuscationKey);
            return EncodeLegacyHex(encodedBytes);
        }

        private static byte[] ApplyLegacyXorTransform(byte[] sourceBytes, byte[] obfuscationKey)
        {
            var transformedBytes = new byte[sourceBytes.Length];
            for (var index = 0; index < sourceBytes.Length; index++)
            {
                var keyIndex = index % LegacyKeyCycleLength;
                if (keyIndex == 0)
                {
                    keyIndex = LegacyKeyCycleLength;
                }

                transformedBytes[index] = (byte)(sourceBytes[index] ^ obfuscationKey[keyIndex - 1]);
            }

            // 保留旧版编码对相邻零字节的特殊处理，确保历史项目数据可以继续读取。
            for (var pairIndex = 0; pairIndex < sourceBytes.Length / 2 - 1; pairIndex++)
            {
                var byteIndex = pairIndex * 2;
                if (transformedBytes[byteIndex] == 0 && transformedBytes[byteIndex + 1] == 0)
                {
                    transformedBytes[byteIndex] = sourceBytes[byteIndex];
                    transformedBytes[byteIndex + 1] = sourceBytes[byteIndex + 1];
                }
            }

            return transformedBytes;
        }

        private static string EncodeLegacyHex(byte[] bytes)
        {
            var characters = new char[bytes.Length * 2];
            for (var index = 0; index < bytes.Length; index++)
            {
                characters[index * 2] = (char)((bytes[index] >> 4) + 'A');
                characters[index * 2 + 1] = (char)((bytes[index] & 0x0F) + 'A');
            }

            return new string(characters);
        }

        private static byte[] DecodeLegacyHex(string encodedString)
        {
            var bytes = new byte[encodedString.Length / 2];
            for (var index = 0; index < bytes.Length; index++)
            {
                var highNibble = encodedString[index * 2] - 'A';
                var lowNibble = encodedString[index * 2 + 1] - 'A';
                bytes[index] = (byte)(highNibble * 16 + lowNibble);
            }

            return bytes;
        }

        /// <summary>
        /// 使用项目现有 AES 格式加密字符串。盐值和密钥派生设置为兼容旧数据而固定。
        /// </summary>
        public static string Encrypt(string plainText, string password)
        {
            ArgumentNullException.ThrowIfNull(plainText);
            ArgumentNullException.ThrowIfNull(password);

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using var aes = new AesManaged();
            using var keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(LegacySalt));
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
            aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);

            using var encryptedStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            return Convert.ToBase64String(encryptedStream.ToArray());
        }

        /// <summary>
        /// 解密由 <see cref="Encrypt"/> 生成的 Base64 字符串。
        /// </summary>
        public static string Decrypt(string encryptedBase64, string password)
        {
            ArgumentNullException.ThrowIfNull(encryptedBase64);
            ArgumentNullException.ThrowIfNull(password);

            var encryptedBytes = Convert.FromBase64String(encryptedBase64);
            using var aes = new AesManaged();
            using var keyDerivation = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(LegacySalt));
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
            aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);

            using var decryptedStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(decryptedStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                cryptoStream.Flush();
            }

            return Encoding.UTF8.GetString(decryptedStream.ToArray());
        }
    }
}
