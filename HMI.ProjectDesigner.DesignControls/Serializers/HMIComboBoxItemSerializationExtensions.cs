// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public static class HMIComboBoxItemSerializationExtensions
	{
		public static void DeserializeComboBoxItem(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<HMIComboBoxItem> comboBoxItems, System.Version version)
		{
			var xElement = propertyCollectionsElement["WriteItems"];
			if (xElement == null)
				return;

			comboBoxItems.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<HMIComboBoxItem>();
			foreach (var current in xElement)
			{
				if (((string?)current["ItemElementName"]) != "HMIComboBoxItem")
					continue;

				HMIComboBoxItem? wFComboBoxItem = serializer2.Deserialize(node, (JObject)current, version) as HMIComboBoxItem;
				if (wFComboBoxItem != null)
				{
					comboBoxItems.Add(wFComboBoxItem);
				}
			}
		}
		public static JProperty GetSerializedComboBoxItem(this BaseSerializer serializer, System.Collections.Generic.ICollection<HMIComboBoxItem> comboBoxItems)
		{
			var xElement = new List<JObject>();
			foreach (HMIComboBoxItem current in comboBoxItems)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}

			return new JProperty("WriteItems", xElement);
		}
	}
}
