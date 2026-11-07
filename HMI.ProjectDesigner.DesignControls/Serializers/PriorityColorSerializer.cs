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
	public class PriorityColorSerializer : BaseSerializer
	{
		public PriorityColorSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			PriorityColor priorityColor = source as PriorityColor;
			return new JObject(
					new JProperty("ItemElementName", "PriorityColor"), 
					new JProperty("Properties", new JObject(
						new JProperty("Attributes", new JObject(
							new JProperty("ActiveAlarmColor", priorityColor.ActiveAlarmColor?.ToString()),
							new JProperty("InactiveAlarmColor", priorityColor.InactiveAlarmColor?.ToString()),
							new JProperty("AcknowledgedAndGoneAlarmColor", priorityColor.AcknowledgedAndGoneAlarmColor?.ToString()),
							new JProperty("AcknowledgedAlarmColor", priorityColor.AcknowledgedAlarmColor?.ToString()),
							new JProperty("Priority", priorityColor.Priority)
						)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"]!;
			PriorityColor priorityColor = new PriorityColor(base.Container, ancestorNode);

			if ((string?)xElement["ActiveAlarmColor"] != null)
				priorityColor.ActiveAlarmColor = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["ActiveAlarmColor"]!)).GetAsFrozen();

            if ((string?)xElement["InactiveAlarmColor"] != null)
                priorityColor.InactiveAlarmColor = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["InactiveAlarmColor"]!)).GetAsFrozen();

            if ((string?)xElement["AcknowledgedAlarmColor"] != null)
                priorityColor.AcknowledgedAlarmColor = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["AcknowledgedAlarmColor"]!)).GetAsFrozen();

            if ((string?)xElement["AcknowledgedAndGoneAlarmColor"] != null)
                priorityColor.AcknowledgedAndGoneAlarmColor = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["AcknowledgedAndGoneAlarmColor"]!)).GetAsFrozen();

			priorityColor.Priority = ((int?)xElement["Priority"]) ?? priorityColor.Priority;
            
			return priorityColor;
		}
	}
}
