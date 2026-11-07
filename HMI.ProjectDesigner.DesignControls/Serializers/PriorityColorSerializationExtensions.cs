// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public static class PriorityColorSerializationExtensions
	{
		public static void DeserializePriorityColor(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<PriorityColor> priorityColors, System.Version version)
		{
			var xElement = propertyCollectionsElement["PriorityColorDefinitions"]?.ToArray();
			if (xElement == null)
				return;

			priorityColors.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<PriorityColor>();
			foreach (var current in xElement)
			{
                if (((string?)current["ItemElementName"]) != "PriorityColor")
                    continue;

                PriorityColor? priorityColor = serializer2.Deserialize(node, (JObject)current, version) as PriorityColor;
				if (priorityColor != null)
				{
					priorityColors.Add(priorityColor);
				}
			}
		}
		public static JProperty GetSerializedPriorityColor(this BaseSerializer serializer, System.Collections.Generic.ICollection<PriorityColor> priorityColors)
		{
			var xElement = new List<JObject>();
			foreach (PriorityColor current in priorityColors)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}

			return new JProperty("PriorityColorDefinitions", xElement);
		}
	}
}
