// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public class AlarmGroupFilterSerializer : BaseSerializer
	{
		public AlarmGroupFilterSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			AlarmGroupFilter alarmGroupFilter = source as AlarmGroupFilter;

			return new JObject(
					new JProperty("ItemElementName", "AlarmGroupFilter"),
					new JProperty("Properties", new JObject( 
							new JProperty("Attributes", new JObject(
								new JProperty("TypeName", alarmGroupFilter.GroupName)
								)))));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"];
			AlarmGroupFilter AlarmGroupFilter = new AlarmGroupFilter(base.Container, ancestorNode);

			AlarmGroupFilter.GroupName = ((string?)xElement?["TypeName"]) ?? AlarmGroupFilter.GroupName;
            
			return AlarmGroupFilter;
		}
	}
}
