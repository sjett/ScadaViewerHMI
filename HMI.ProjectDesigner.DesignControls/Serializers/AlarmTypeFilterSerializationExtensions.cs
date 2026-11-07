// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public static class AlarmTypeFilterSerializationExtensions
	{
		public static JProperty GetSerializedAlarmTypeFilter(this BaseSerializer serializer, System.Collections.Generic.ICollection<AlarmTypeFilter> alarmTypeFilters)
		{
			var xElement = new List<JObject>();
			foreach (AlarmTypeFilter current in alarmTypeFilters)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("TypeFilters", xElement);
		}
		public static void DeserializeAlarmTypeFilter(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<AlarmTypeFilter> alarmTypeFilter, System.Version version)
		{
			var xElement = propertyCollectionsElement["TypeFilters"]?.ToArray();
			if (xElement == null)
				return;

			alarmTypeFilter.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<AlarmTypeFilter>();
			foreach (var current in xElement)
			{
                if (((string?)current["ItemElementName"]) != "AlarmTypeFilter")
                    continue;

                AlarmTypeFilter? alarmTypeFilter2 = serializer2.Deserialize(node, (JObject)current, version) as AlarmTypeFilter;
				if (alarmTypeFilter2 != null)
				{
					alarmTypeFilter.Add(alarmTypeFilter2);
				}
			}
		}
	}
}
