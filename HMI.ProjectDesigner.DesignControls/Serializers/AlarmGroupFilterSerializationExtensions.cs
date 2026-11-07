// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public static class AlarmGroupFilterSerializationExtensions
	{
		public static void DeserializeAlarmGroupFilter(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<AlarmGroupFilter> alarmGroupFilter, System.Version version)
		{
			var xElement = propertyCollectionsElement["GroupFilters"]?.ToArray();
			if (xElement == null)
				return;

			alarmGroupFilter.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<AlarmGroupFilter>();
			foreach (var current in xElement)
			{
                if (((string?)current["ItemElementName"]) != "AlarmGroupFilter")
                    continue;

                AlarmGroupFilter? alarmGroupFilter2 = serializer2.Deserialize(node, (JObject)current, version) as AlarmGroupFilter;
				if (alarmGroupFilter2 != null)
				{
					alarmGroupFilter.Add(alarmGroupFilter2);
				}
			}
		}
		public static JProperty GetSerializedAlarmGroupFilter(this BaseSerializer serializer, System.Collections.Generic.ICollection<AlarmGroupFilter> alarmGroupFilters)
		{
			var xElement = new List<JObject>();
			foreach (AlarmGroupFilter current in alarmGroupFilters)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}

			return new JProperty("GroupFilters", xElement);
		}
	}
}
