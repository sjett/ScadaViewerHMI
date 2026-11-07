// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public class AlarmTypeFilterSerializer : BaseSerializer
	{
		public AlarmTypeFilterSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			AlarmTypeFilter alarmTypeFilter = source as AlarmTypeFilter;

			return new JObject(
				new JProperty("ItemElementName", "AlarmTypeFilter"), 
				new JProperty("Properties", new JObject(
					new JProperty("Attributes", new JObject(
						new JProperty("TypeName", alarmTypeFilter.TypeName)
						)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"]!;
			AlarmTypeFilter alarmTypeFilter = new AlarmTypeFilter(base.Container, ancestorNode);

			alarmTypeFilter.TypeName = ((string?)xElement["TypeName"]) ?? alarmTypeFilter.TypeName;
            
			return alarmTypeFilter;
		}
	}
}
