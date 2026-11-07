// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HMI.Common.Xml
{
    public static class LinqToXmlExtensions
    {
        public static XElement SafeElement(this XContainer container, XName name, object defaultValue = null)
        {
            return container.Element(name) ?? new XElement(name, defaultValue);
        }
        public static IEnumerable<XElement> SafeElements(this XContainer container, XName name)
        {
            return container.Elements(name);
        }
        public static XAttribute SafeAttribute(this XElement container, XName name, object defaultValue = null)
        {
            return container.Attribute(name) ?? new XAttribute(name, defaultValue);
        }
        public static void PreserveWhitespace(this XElement element)
        {
        }
        public static void PreserveWhitespace(this XDocument document)
        {
            document.Root.PreserveWhitespace();
        }
    }
}
