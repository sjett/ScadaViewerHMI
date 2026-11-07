// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public static class RowColorSerializerExtensions
	{
		public static void DeserializeRowColors(this BaseSerializer serializer, JObject? propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<RowColor> rowColors, System.Version version)
		{
			var xElement = propertyCollectionsElement?["RowColorDefinitions"]?.ToArray();
			if (xElement == null)
				return;

			rowColors.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<RowColor>();
			foreach (var current in xElement)
			{
				if (((string?)current["ItemElementName"]) != "RowColor")
					continue;

                var rowColor = serializer2.Deserialize(node, (JObject)current, version) as RowColor;
				if (rowColor != null)
				{
					rowColors.Add(rowColor);
				}
			}
		}
		public static JProperty GetSerializedRowColors(this BaseSerializer serializer, System.Collections.Generic.ICollection<RowColor> rowColors)
		{
			var xElement = new List<JObject>();
			foreach (RowColor current in rowColors)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("RowColorDefinitions", xElement);
		}
	}
}
