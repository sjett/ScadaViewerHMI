// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public static class AlarmTypeDisplaySerializerExtensions
	{
		public static void DeserializeAlarmTypeDisplay(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<AlarmTypeDisplay> alarmTypeDisplays, System.Version version)
		{
			var xElement = propertyCollectionsElement["AlarmTypesInformation"]?.ToArray();
			if (xElement == null)
				return;

			alarmTypeDisplays.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<AlarmTypeDisplay>();
			foreach (var current in xElement)
			{
                if (((string?)current["ItemElementName"]) != "AlarmTypeDisplay")
                    continue;

                AlarmTypeDisplay? alarmTypeDisplay = serializer2.Deserialize(node, (JObject)current, version) as AlarmTypeDisplay;
				if (alarmTypeDisplay != null)
				{
					alarmTypeDisplays.Add(alarmTypeDisplay);
				}
			}
		}
		public static JProperty GetSerializedAlarmTypeDisplays(this BaseSerializer serializer, System.Collections.Generic.ICollection<AlarmTypeDisplay> alarmTypeDisplays)
		{
			var xElement = new List<JObject>();
			foreach (AlarmTypeDisplay current in alarmTypeDisplays)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}

			return new JProperty("AlarmTypesInformation", xElement);
		}
	}
}
