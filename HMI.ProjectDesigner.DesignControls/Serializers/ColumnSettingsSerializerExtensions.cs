// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public static class ColumnSettingsSerializerExtensions
	{
		public static JProperty GetSerializedColumnSettings(this BaseSerializer serializer, System.Collections.Generic.ICollection<ColumnSettings> columnSettings)
		{
			var xElement = new List<JObject>();
			foreach (ColumnSettings current in columnSettings)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("Columns", xElement);
		}
		public static void DeserializeColumnSettings(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<ColumnSettings> columnSettings, System.Version version)
		{
			var xElement = propertyCollectionsElement["Columns"];
			if (xElement == null)
				return;

			columnSettings.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<ColumnSettings>();
			foreach (var current in xElement)
            {
				if (((string?)current["ItemElementName"]) != "ColumnSettings")
					continue;

				ColumnSettings? columnSettings2 = serializer2.Deserialize(node, (JObject)current, version) as ColumnSettings;
				if (columnSettings2 != null)
				{
					columnSettings.Add(columnSettings2);
				}
			}
		}
	}
}
