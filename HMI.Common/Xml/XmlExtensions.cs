// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Xml;
using System.Xml.Linq;

namespace HMI.Common.Xml
{
    public static class XmlExtensions
    {
        public static XElement GetXElement(this XmlNode node)
        {
            XDocument xDocument = new XDocument();
            using (XmlWriter xmlWriter = xDocument.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }
            return xDocument.Root;
        }
        public static XmlNode GetXmlNode(this XElement element)
        {
            XmlNode result;
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlReader);
                result = xmlDocument;
            }
            return result;
        }
    }
}
