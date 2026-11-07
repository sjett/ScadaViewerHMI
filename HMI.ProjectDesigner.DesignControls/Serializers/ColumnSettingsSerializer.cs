// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.Infrastructure.Serialization
{
	public class ColumnSettingsSerializer : BaseSerializer
	{
		public ColumnSettingsSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			ColumnSettings columnSettings = source as ColumnSettings;

			return new JObject(
				new JProperty("ItemElementName", "ColumnSettings"),
				new JProperty("Properties", new JObject(
					new JProperty("Attributes", new JObject(
						new JProperty("IsAutoSize", columnSettings.IsAutoSize),
						new JProperty("Width", columnSettings.Width),
						new JProperty("IsVisible", columnSettings.IsVisible),
						new JProperty("Type", columnSettings.Type)
					)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"];
			ColumnSettings columnSettings = new ColumnSettings(base.Container, ancestorNode);

			columnSettings.IsAutoSize = ((bool?)xElement["IsAutoSize"]) ?? columnSettings.IsAutoSize;

            columnSettings.Width = ((double?)xElement["Width"]) ?? columnSettings.Width;

            columnSettings.IsVisible = ((bool?)xElement["IsVisible"]) ?? columnSettings.IsVisible;

            columnSettings.Type = xElement["Type"]?.ToObject<ColumnTypes?>() ?? columnSettings.Type;

			return columnSettings;
		}
	}
}
