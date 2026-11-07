// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HMI.Runtime.Core.Helper
{
	internal class Cryptography
	{
		private static readonly Type[] SupportedXmlTypes = new Type[]
		{
			typeof(object[]),
			typeof(sbyte[]),
			typeof(byte[]),
			typeof(short[]),
			typeof(ushort[]),
			typeof(int[]),
			typeof(uint[]),
			typeof(long[]),
			typeof(ulong[]),
			typeof(float[]),
			typeof(double[]),
			typeof(decimal[]),
			typeof(bool[]),
			typeof(DateTime[]),
			typeof(string[])
		};
		internal static string Encrypt(string input, string password = "a45e6a42-1bfa-425e-9d35-dc4d4a3d0179")
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			byte[] salt = Encoding.UTF8.GetBytes("6bd93557-3925-4b91-a577-5807b696df4f");
			AesManaged aesManaged = new AesManaged();
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt);
			aesManaged.BlockSize =(aesManaged.LegalBlockSizes[0].MaxSize);
			aesManaged.KeySize=(aesManaged.LegalKeySizes[0].MaxSize);
			aesManaged.Key=(rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 8));
			aesManaged.IV =(rfc2898DeriveBytes.GetBytes(aesManaged.BlockSize / 8));
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesManaged.CreateEncryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.FlushFinalBlock();
					cryptoStream.Close();
					result = Convert.ToBase64String(memoryStream.ToArray());
				}
			}
			return result;
		}
		internal static string Decrypt(string base64Input, string password = "a45e6a42-1bfa-425e-9d35-dc4d4a3d0179")
		{
			byte[] array = Convert.FromBase64String(base64Input);
			byte[] salt = Encoding.UTF8.GetBytes("6bd93557-3925-4b91-a577-5807b696df4f");
			AesManaged aesManaged = new AesManaged();
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt);
			aesManaged.BlockSize =(aesManaged.LegalBlockSizes[0].MaxSize);
			aesManaged.KeySize=(aesManaged.LegalKeySizes[0].MaxSize);
			aesManaged.Key=(rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 8));
			aesManaged.IV =(rfc2898DeriveBytes.GetBytes(aesManaged.BlockSize / 8));
			string @string;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesManaged.CreateDecryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(array, 0, array.Length);
					cryptoStream.Flush();
					cryptoStream.Close();
					byte[] decryptedBytes = memoryStream.ToArray();
					@string = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
				}
			}
			return @string;
		}
		internal static string EncryptXml(object input, string password = "a45e6a42-1bfa-425e-9d35-dc4d4a3d0179")
		{
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
				{
					object[] array = new object[]
					{
						input
					};
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(object[]), Cryptography.SupportedXmlTypes);
					xmlSerializer.Serialize(xmlWriter, array);
					result = Cryptography.Encrypt(stringWriter.ToString(), password);
				}
			}
			return result;
		}
		internal static object DecryptXml(string input, string password = "a45e6a42-1bfa-425e-9d35-dc4d4a3d0179")
		{
			input = Cryptography.Decrypt(input, password);
			object result;
			using (StringReader stringReader = new StringReader(input))
			{
				using (XmlReader xmlReader = XmlReader.Create(stringReader))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(object[]), Cryptography.SupportedXmlTypes);
					result = Enumerable.First<object>(xmlSerializer.Deserialize(xmlReader) as object[]);
				}
			}
			return result;
		}
	}
}
