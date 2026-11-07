// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Media;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public class RowColorSerializer : BaseSerializer
	{
		public RowColorSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			RowColor rowColor = source as RowColor;
			return new JObject(
				new JProperty("ItemElementName", "RowColor"), 
				new JProperty("Properties", new JObject(
					new JProperty("Attributes", new JObject(
						new JProperty("ActiveAlarmBackground", rowColor.ActiveAlarmBackground?.ToString()),
						new JProperty("ActiveAlarmForeground", rowColor.ActiveAlarmForeground?.ToString()),
						new JProperty("InactiveAlarmForeground", rowColor.InactiveAlarmForeground?.ToString()),
						new JProperty("InactiveAlarmBackground", rowColor.InactiveAlarmBackground?.ToString()),
						new JProperty("AcknowledgedAlarmBackground", rowColor.AcknowledgedAlarmBackground?.ToString()),
						new JProperty("AcknowledgedAlarmForeground", rowColor.AcknowledgedAlarmForeground?.ToString()),
						new JProperty("AlarmGroup", rowColor.AlarmGroup),
						new JProperty("AlarmType", rowColor.AlarmType),
						new JProperty("Priority", rowColor.Priority),
						new JProperty("HornPath", rowColor.HornPath),
						new JProperty("Loop", rowColor.Loop)
					)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"];
			RowColor rowColor = new RowColor(base.Container, ancestorNode);

			if ((string?)xElement!["ActiveAlarmBackground"] != null)
				rowColor.ActiveAlarmBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["ActiveAlarmBackground"]!)).GetAsFrozen();

            if ((string?)xElement!["ActiveAlarmForeground"] != null)
                rowColor.ActiveAlarmForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["ActiveAlarmForeground"]!)).GetAsFrozen();

            if ((string?)xElement!["InactiveAlarmForeground"] != null)
                rowColor.InactiveAlarmForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["InactiveAlarmForeground"]!)).GetAsFrozen();


            if ((string?)xElement!["InactiveAlarmBackground"] != null)
                rowColor.InactiveAlarmBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["InactiveAlarmBackground"]!)).GetAsFrozen();

            if ((string?)xElement!["AcknowledgedAlarmBackground"] != null)
                rowColor.AcknowledgedAlarmBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["AcknowledgedAlarmBackground"]!)).GetAsFrozen();

            if ((string?)xElement!["AcknowledgedAlarmForeground"] != null)
                rowColor.AcknowledgedAlarmForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["AcknowledgedAlarmForeground"]!)).GetAsFrozen();

			rowColor.Priority = ((int?)xElement["Priority"]) ?? rowColor.Priority;

			rowColor.HornPath = ((string?)xElement["HornPath"]) ?? rowColor.HornPath;

            rowColor.Loop = ((bool?)xElement["Loop"]) ?? rowColor.Loop;

            rowColor.AlarmGroup = ((string?)xElement["AlarmGroup"]) ?? rowColor.AlarmGroup;

            rowColor.AlarmType = ((string?)xElement["AlarmType"]) ?? rowColor.AlarmType;
            
			return rowColor;
		}
	}
}
