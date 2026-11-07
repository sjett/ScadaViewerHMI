// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public class AlarmColumnSettingsSerializer : BaseSerializer
	{
		public AlarmColumnSettingsSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			AlarmColumnSettings alarmColumnSettings = source as AlarmColumnSettings;
			return new JObject(
				new JProperty("ItemElementName", "AlarmColumnSettings"), 
				new JProperty("Properties", new JObject(
					new JProperty("Attributes", new JObject(
						new JProperty("IsAutoSize", alarmColumnSettings.IsAutoSize),
						new JProperty("IsVisible", alarmColumnSettings.IsVisible),
						new JProperty("Type", alarmColumnSettings.Type),
						new JProperty("IsFilterable", alarmColumnSettings.IsFilterable),
						new JProperty("Width", alarmColumnSettings.Width)
					)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"]!;

			AlarmColumnSettings settings = new AlarmColumnSettings(base.Container, ancestorNode);

			settings.IsAutoSize = ((bool?)xElement["IsAutoSize"]) ?? settings.IsAutoSize;

            settings.IsVisible = ((bool?)xElement["IsVisible"]) ?? settings.IsVisible;

            settings.IsFilterable = ((bool?)xElement["IsFilterable"]) ?? settings.IsFilterable;

            settings.Type = xElement["Type"]?.ToObject<AlarmColumnTypes?>() ?? settings.Type;

            settings.Width = ((double?)xElement["Width"]) ?? settings.Width;

			return settings;
		}
	}
}
