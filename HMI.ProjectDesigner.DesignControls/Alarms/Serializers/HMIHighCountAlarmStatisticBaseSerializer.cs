// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Alarms.Serializers
{
	public class HMIHighCountAlarmStatisticBaseSerializer : HMIAlarmStatisticBaseSerializer
	{
		public HMIHighCountAlarmStatisticBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIHighCountAlarmStatisticBaseViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIHighCountAlarmStatisticBaseViewModel node = target as HMIHighCountAlarmStatisticBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.MaximumAlarms = ((int?)properties["MaximumAlarms"]) ?? node.MaximumAlarms;

			node.DisplayType = properties["DisplayType"]?.ToObject<DisplayType?>() ?? node.DisplayType;

			node.AlarmGroupName = ((string?)properties["AlarmGroupName"]) ?? node.AlarmGroupName;

			node.AlarmTypeName = ((string?)properties["AlarmTypeName"]) ?? node.AlarmTypeName;
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIHighCountAlarmStatisticBaseViewModel wFHighCountAlarmStatisticBaseViewModel = source as HMIHighCountAlarmStatisticBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFHighCountAlarmStatisticBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("MaximumAlarms", wFHighCountAlarmStatisticBaseViewModel.MaximumAlarms),
				new JProperty("DisplayType", wFHighCountAlarmStatisticBaseViewModel.DisplayType),
				new JProperty("AlarmGroupName", wFHighCountAlarmStatisticBaseViewModel.AlarmGroupName),
				new JProperty("AlarmTypeName", wFHighCountAlarmStatisticBaseViewModel.AlarmTypeName)
			});
		}
	}
}
