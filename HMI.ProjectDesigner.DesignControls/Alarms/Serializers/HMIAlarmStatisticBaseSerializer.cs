// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Alarms.Serializers
{
	public class HMIAlarmStatisticBaseSerializer : CommonControlSerializer
	{
		public HMIAlarmStatisticBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIAlarmStatisticBaseViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIAlarmStatisticBaseViewModel node = target as HMIAlarmStatisticBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

			node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

			node.DefaultRange = properties["DefaultRange"]?.ToObject<StatisticsTimespanRange?>() ?? node.DefaultRange;

			node.ChartTheme = properties["ChartTheme"]?.ToObject<TelerikTheme?>() ?? node.ChartTheme;

			node.PopupModality = properties["PopupModality"]?.ToObject<DialogModality?>() ?? node.PopupModality;

			node.StartDate = ((DateTime?)properties["StartDate"]) ?? node.StartDate;

			node.EndDate = ((DateTime?)properties["EndDate"]) ?? node.EndDate;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIAlarmStatisticBaseViewModel wFAlarmStatisticBaseViewModel = source as HMIAlarmStatisticBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFAlarmStatisticBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SecurityAuthorizationName", wFAlarmStatisticBaseViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFAlarmStatisticBaseViewModel.SecurityDenyAccessBehaviour),
				new JProperty("DefaultRange", wFAlarmStatisticBaseViewModel.DefaultRange),
				new JProperty("ChartTheme", wFAlarmStatisticBaseViewModel.ChartTheme),
				new JProperty("PopupModality", wFAlarmStatisticBaseViewModel.PopupModality),
				new JProperty("StartDate", wFAlarmStatisticBaseViewModel.StartDate),
				new JProperty("EndDate", wFAlarmStatisticBaseViewModel.EndDate)
			});
		}
	}
}
