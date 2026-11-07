// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Alarms.Serializers
{
	public class HMIAlarmStateManagerSerializer : CommonControlSerializer
	{
		public HMIAlarmStateManagerSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIAlarmStateManagerViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIAlarmStateManagerViewModel node = target as HMIAlarmStateManagerViewModel;
			base.DeserializeProperties(properties, node, version);

			node.UseColumnWidthProperties = ((bool?)properties["UseColumnWidthProperties"]) ?? node.UseColumnWidthProperties;

            node.ConditionNameWidth = ((double?)properties["ConditionNameWidth"]) ?? node.ConditionNameWidth;

            node.GroupWidth = ((double?)properties["GroupWidth"]) ?? node.GroupWidth;

            node.OPCItemNameWidth = ((double?)properties["OPCItemNameWidth"]) ?? node.OPCItemNameWidth;

            node.ReactivationDateWidth = ((double?)properties["ReactivationDateWidth"]) ?? node.ReactivationDateWidth;

            node.SignalDescriptionWidth = ((double?)properties["SignalDescriptionWidth"]) ?? node.SignalDescriptionWidth;

            node.SignalNameWidth = ((double?)properties["SignalNameWidth"]) ?? node.SignalNameWidth;

            node.StatusWidth = ((double?)properties["StatusWidth"]) ?? node.StatusWidth;

            node.TagWidth = ((double?)properties["TagWidth"]) ?? node.TagWidth;

            node.TextWidth = ((double?)properties["TextWidth"]) ?? node.TextWidth;

            node.TypeWidth = ((double?)properties["TypeWidth"]) ?? node.TypeWidth;

            node.UseColumnOrderProperties = ((bool?)properties["UseColumnOrderProperties"]) ?? node.UseColumnOrderProperties;

            node.ConditionNameOrder = ((int?)properties["ConditionNameOrder"]) ?? node.ConditionNameOrder;

            node.GroupOrder = ((int?)properties["GroupOrder"]) ?? node.GroupOrder;

            node.OPCItemNameOrder = ((int?)properties["OPCItemNameOrder"]) ?? node.OPCItemNameOrder;

            node.ReactivationDateOrder = ((int?)properties["ReactivationDateOrder"]) ?? node.ReactivationDateOrder;

            node.SignalDescriptionOrder = ((int?)properties["SignalDescriptionOrder"]) ?? node.SignalDescriptionOrder;

            node.SignalNameOrder = ((int?)properties["SignalNameOrder"]) ?? node.SignalNameOrder;

            node.StatusOrder = ((int?)properties["StatusOrder"]) ?? node.StatusOrder;

            node.TagOrder = ((int?)properties["TagOrder"]) ?? node.TagOrder;

            node.TextOrder = ((int?)properties["TextOrder"]) ?? node.TextOrder;

            node.TypeOrder = ((int?)properties["TypeOrder"]) ?? node.TypeOrder;

            node.UseColumnVisibilityProperties = ((bool?)properties["UseColumnVisibilityProperties"]) ?? node.UseColumnVisibilityProperties;

			node.ConditionNameVisibility = properties["ConditionNameVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.ConditionNameVisibility;

            node.GroupVisibility = properties["GroupVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.GroupVisibility;

            node.OPCItemNameVisibility = properties["OPCItemNameVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.OPCItemNameVisibility;

            node.ReactivationDateVisibility = properties["ReactivationDateVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.ReactivationDateVisibility;

            node.SignalDescriptionVisibility = properties["SignalDescriptionVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.SignalDescriptionVisibility;

            node.SignalNameVisibility = properties["SignalNameVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.SignalNameVisibility;

            node.StatusVisibility = properties["StatusVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.StatusVisibility;

            node.TagVisibility = properties["TagVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.TagVisibility;

            node.TextVisibility = properties["TextVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.TextVisibility;

            node.TypeVisibility = properties["TypeVisibility"]?.ToObject<SilverlightVisibility?>() ?? node.TypeVisibility;
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIAlarmStateManagerViewModel wFAlarmStateManagerViewModel = source as HMIAlarmStateManagerViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFAlarmStateManagerViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("UseColumnWidthProperties", wFAlarmStateManagerViewModel.UseColumnWidthProperties),
				new JProperty("ConditionNameWidth", wFAlarmStateManagerViewModel.ConditionNameWidth),
				new JProperty("GroupWidth", wFAlarmStateManagerViewModel.GroupWidth),
				new JProperty("OPCItemNameWidth", wFAlarmStateManagerViewModel.OPCItemNameWidth),
				new JProperty("ReactivationDateWidth", wFAlarmStateManagerViewModel.ReactivationDateWidth),
				new JProperty("SignalDescriptionWidth", wFAlarmStateManagerViewModel.SignalDescriptionWidth),
				new JProperty("SignalNameWidth", wFAlarmStateManagerViewModel.SignalNameWidth),
				new JProperty("StatusWidth", wFAlarmStateManagerViewModel.StatusWidth),
				new JProperty("TagWidth", wFAlarmStateManagerViewModel.TagWidth),
				new JProperty("TextWidth", wFAlarmStateManagerViewModel.TextWidth),
				new JProperty("TypeWidth", wFAlarmStateManagerViewModel.TypeWidth),
				new JProperty("UseColumnOrderProperties", wFAlarmStateManagerViewModel.UseColumnOrderProperties),
				new JProperty("ConditionNameOrder", wFAlarmStateManagerViewModel.ConditionNameOrder),
				new JProperty("GroupOrder", wFAlarmStateManagerViewModel.GroupOrder),
				new JProperty("OPCItemNameOrder", wFAlarmStateManagerViewModel.OPCItemNameOrder),
				new JProperty("ReactivationDateOrder", wFAlarmStateManagerViewModel.ReactivationDateOrder),
				new JProperty("SignalDescriptionOrder", wFAlarmStateManagerViewModel.SignalDescriptionOrder),
				new JProperty("SignalNameOrder", wFAlarmStateManagerViewModel.SignalNameOrder),
				new JProperty("StatusOrder", wFAlarmStateManagerViewModel.StatusOrder),
				new JProperty("TagOrder", wFAlarmStateManagerViewModel.TagOrder),
				new JProperty("TextOrder", wFAlarmStateManagerViewModel.TextOrder),
				new JProperty("TypeOrder", wFAlarmStateManagerViewModel.TypeOrder),
				new JProperty("UseColumnVisibilityProperties", wFAlarmStateManagerViewModel.UseColumnVisibilityProperties),
				new JProperty("ConditionNameVisibility", wFAlarmStateManagerViewModel.ConditionNameVisibility),
				new JProperty("GroupVisibility", wFAlarmStateManagerViewModel.GroupVisibility),
				new JProperty("OPCItemNameVisibility", wFAlarmStateManagerViewModel.OPCItemNameVisibility),
				new JProperty("ReactivationDateVisibility", wFAlarmStateManagerViewModel.ReactivationDateVisibility),
				new JProperty("SignalDescriptionVisibility", wFAlarmStateManagerViewModel.SignalDescriptionVisibility),
				new JProperty("SignalNameVisibility", wFAlarmStateManagerViewModel.SignalNameVisibility),
				new JProperty("StatusVisibility", wFAlarmStateManagerViewModel.StatusVisibility),
				new JProperty("TagVisibility", wFAlarmStateManagerViewModel.TagVisibility),
				new JProperty("TextVisibility", wFAlarmStateManagerViewModel.TextVisibility),
				new JProperty("TypeVisibility", wFAlarmStateManagerViewModel.TypeVisibility)
			});
		}
	}
}
