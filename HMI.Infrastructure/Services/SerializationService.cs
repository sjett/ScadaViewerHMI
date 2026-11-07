// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace HMI.Infrastructure.Services
{
    public class SerializationService : ISerializationService
    {
        public string Serialize(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "item is null.");
            }
            DataContractSerializer dataContractSerializer = new DataContractSerializer(item.GetType());
            string result;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    dataContractSerializer.WriteObject(xmlTextWriter, item);
                    stringWriter.Flush();
                    result = stringWriter.ToString();
                }
            }
            return result;
        }
        public T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("xml is null or empty.", "xml");
            }
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
            T result;
            using (StringReader stringReader = new StringReader(xml))
            {
                using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
                {
                    result = (T)dataContractSerializer.ReadObject(xmlTextReader);
                }
            }
            return result;
        }
    }
}
