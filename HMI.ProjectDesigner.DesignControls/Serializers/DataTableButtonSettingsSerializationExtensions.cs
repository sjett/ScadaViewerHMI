// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public static class DataTableButtonSettingsSerializationExtensions
	{
		public static void DeserializeDataTableButtonSettings(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<DataTableButtonSettings> dataTableButtonSettings, System.Version version)
		{
			var xElement = propertyCollectionsElement["DataTableButtonSettings"]?.ToArray();
			if (xElement == null)
				return;

			System.Collections.Generic.Dictionary<DataTableButtons, DataTableButtonSettings> dictionary = new System.Collections.Generic.Dictionary<DataTableButtons, DataTableButtonSettings>();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<DataTableButtonSettings>();
			foreach (var current in xElement)
            {
				if (((string?)current["ItemElementName"]) != "DataTableButtonSetting")
					continue;

				DataTableButtonSettings? dataTableButtonSettings2 = serializer2.Deserialize(node, (JObject)current, version) as DataTableButtonSettings;
				if (dataTableButtonSettings2 != null)
				{
					dictionary[dataTableButtonSettings2.Button] = dataTableButtonSettings2;
				}
			}
			foreach (DataTableButtonSettings existingSettings in dataTableButtonSettings)
			{
				DataTableButtonSettings? deserializedSettings;
				if (dictionary.TryGetValue(existingSettings.Button, out deserializedSettings))
				{
					deserializedSettings.DeepCopyFields(existingSettings);
				}
			}
		}
		public static JProperty GetSerializedDataTableButtonSettings(this BaseSerializer serializer, System.Collections.Generic.ICollection<DataTableButtonSettings> settings)
		{
			var xElement = new List<JObject>();
			foreach (var current in settings)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}

			return new JProperty("DataTableButtonSettings", xElement);
		}
	}
}
