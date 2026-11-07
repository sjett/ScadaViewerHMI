// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public class AlarmTypeDisplaySerializer : BaseSerializer
	{
		public AlarmTypeDisplaySerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			AlarmTypeDisplay alarmTypeDisplay = source as AlarmTypeDisplay;
			return new JObject(
				new JProperty("ItemElementName", "AlarmTypeDisplay"), 
				new JProperty("Properties", new JObject(
					new JProperty("Attributes", new JObject(
						new JProperty("Color", alarmTypeDisplay.Color?.ToString()),
						new JProperty("SymbolicName", alarmTypeDisplay.SymbolicName),
						new JProperty("AlarmOnSymbolicName", alarmTypeDisplay.SymbolicName),
						new JProperty("AlarmOffSymbolicName", alarmTypeDisplay.AlarmOffSymbolicName),
						new JProperty("AlarmAcknowledgedSymbolicName", alarmTypeDisplay.AlarmAcknowledgedSymbolicName),
						new JProperty("AlarmActivatedSymbolicName", alarmTypeDisplay.AlarmActivatedSymbolicName),
						new JProperty("AlarmDeactivatedSymbolicName", alarmTypeDisplay.AlarmDeactivatedSymbolicName)
					)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"];
			AlarmTypeDisplay alarmTypeDisplay = new AlarmTypeDisplay(base.Container, ancestorNode);

			if ((string?)xElement!["Color"] != null)
				alarmTypeDisplay.Color = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)xElement["Color"]!)).GetAsFrozen();

			alarmTypeDisplay.SymbolicName = ((string?)xElement["SymbolicName"]) ?? alarmTypeDisplay.SymbolicName;

            alarmTypeDisplay.AlarmOnSymbolicName = ((string?)xElement["AlarmOnSymbolicName"]) ?? alarmTypeDisplay.AlarmOnSymbolicName;

            alarmTypeDisplay.AlarmOffSymbolicName = ((string?)xElement["AlarmOffSymbolicName"]) ?? alarmTypeDisplay.AlarmOffSymbolicName;

            alarmTypeDisplay.AlarmAcknowledgedSymbolicName = ((string?)xElement["AlarmAcknowledgedSymbolicName"]) ?? alarmTypeDisplay.AlarmAcknowledgedSymbolicName;

            alarmTypeDisplay.AlarmActivatedSymbolicName = ((string?)xElement["AlarmActivatedSymbolicName"]) ?? alarmTypeDisplay.AlarmActivatedSymbolicName;

            alarmTypeDisplay.AlarmDeactivatedSymbolicName = ((string?)xElement["AlarmDeactivatedSymbolicName"]) ?? alarmTypeDisplay.AlarmDeactivatedSymbolicName;
            
			return alarmTypeDisplay;
		}
	}
}
