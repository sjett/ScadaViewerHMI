// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public static class AlarmColumnSettingSerializerExtensions
	{
		public static void DeserializeAlarmColumnSettings(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<AlarmColumnSettings> rowColors, System.Version version)
		{
			if (propertyCollectionsElement["AlarmColumnSettingDefinitions"] == null)
			{
				return;
			}
			var xElement = propertyCollectionsElement["AlarmColumnSettingDefinitions"]!.ToArray();
			rowColors.Clear();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<AlarmColumnSettings>();

			foreach (var current in xElement)
			{
                if (((string?)current["ItemElementName"]) != "AlarmColumnSettings")
                    continue;

                AlarmColumnSettings? alarmColumnSettings = serializer2.Deserialize(node, (JObject)current, version) as AlarmColumnSettings;
				if (alarmColumnSettings != null)
				{
					rowColors.Add(alarmColumnSettings);
				}
			}
		}
		public static JProperty GetSerializedAlarmColumnSettings(this BaseSerializer serializer, System.Collections.Generic.ICollection<AlarmColumnSettings> alarmColumnSettings)
		{
			var xElement = new List<JObject>();
			foreach (AlarmColumnSettings current in alarmColumnSettings)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("AlarmColumnSettingDefinitions", xElement);
		}
	}
}
