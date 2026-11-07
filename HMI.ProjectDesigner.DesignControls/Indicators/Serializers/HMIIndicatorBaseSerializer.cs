// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.Indicators.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Indicators.Serializers
{
	public abstract class HMIIndicatorBaseSerializer : CommonControlSerializer
	{
		public HMIIndicatorBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIIndicatorBaseViewModel node = target as HMIIndicatorBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

            node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

			node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

			node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.BinaryMask = ((bool?)properties["BinaryMask"]) ?? node.BinaryMask;

            node.ConditionRule1 = ((string?)properties["ConditionRule1"]) ?? node.ConditionRule1;
            node.ConditionRule2 = ((string?)properties["ConditionRule2"]) ?? node.ConditionRule2;
            node.ConditionRule3 = ((string?)properties["ConditionRule3"]) ?? node.ConditionRule3;
            node.ConditionRule4 = ((string?)properties["ConditionRule4"]) ?? node.ConditionRule4;
            node.ConditionRule5 = ((string?)properties["ConditionRule5"]) ?? node.ConditionRule5;
            node.ConditionRule6 = ((string?)properties["ConditionRule6"]) ?? node.ConditionRule6;
            node.ConditionRule7 = ((string?)properties["ConditionRule7"]) ?? node.ConditionRule7;
            node.ConditionRule8 = ((string?)properties["ConditionRule8"]) ?? node.ConditionRule8;

            node.MaskSignal1 = ((int?)properties["MaskSignal1"]) ?? node.MaskSignal1;
            node.MaskSignal2 = ((int?)properties["MaskSignal2"]) ?? node.MaskSignal2;
            node.MaskSignal3 = ((int?)properties["MaskSignal3"]) ?? node.MaskSignal3;
            node.MaskSignal4 = ((int?)properties["MaskSignal4"]) ?? node.MaskSignal4;
            node.MaskSignal5 = ((int?)properties["MaskSignal5"]) ?? node.MaskSignal5;
            node.MaskSignal6 = ((int?)properties["MaskSignal6"]) ?? node.MaskSignal6;
            node.MaskSignal7 = ((int?)properties["MaskSignal7"]) ?? node.MaskSignal7;
            node.MaskSignal8 = ((int?)properties["MaskSignal8"]) ?? node.MaskSignal8;

            node.StateSignalName1 = ((string?)properties["StateSignalName1"]) ?? node.StateSignalName1;
            node.StateSignalName2 = ((string?)properties["StateSignalName2"]) ?? node.StateSignalName2;
            node.StateSignalName3 = ((string?)properties["StateSignalName3"]) ?? node.StateSignalName3;
            node.StateSignalName4 = ((string?)properties["StateSignalName4"]) ?? node.StateSignalName4;
            node.StateSignalName5 = ((string?)properties["StateSignalName5"]) ?? node.StateSignalName5;
            node.StateSignalName6 = ((string?)properties["StateSignalName6"]) ?? node.StateSignalName6;
            node.StateSignalName7 = ((string?)properties["StateSignalName7"]) ?? node.StateSignalName7;
            node.StateSignalName8 = ((string?)properties["StateSignalName8"]) ?? node.StateSignalName8;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			HMIIndicatorBaseViewModel wFIndicatorBaseViewModel = target as HMIIndicatorBaseViewModel;
			base.DeserializeCollections(propertyCollectionsElement, wFIndicatorBaseViewModel, version);
			this.DeserializeConditionSignals(propertyCollectionsElement, wFIndicatorBaseViewModel, wFIndicatorBaseViewModel.ConditionSignals, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIIndicatorBaseViewModel wFIndicatorBaseViewModel = source as HMIIndicatorBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFIndicatorBaseViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ObjectID", wFIndicatorBaseViewModel.ObjectID),
				new JProperty("SignalPrefix", wFIndicatorBaseViewModel.SignalPrefix),
				new JProperty("SecurityAuthorizationName", wFIndicatorBaseViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFIndicatorBaseViewModel.SecurityDenyAccessBehaviour),
				new JProperty("VisibilityMask", wFIndicatorBaseViewModel.VisibilityMask),
				new JProperty("VisibilitySignalName", wFIndicatorBaseViewModel.VisibilitySignalName),
				new JProperty("BinaryMask", wFIndicatorBaseViewModel.BinaryMask),

				new JProperty("ConditionRule1", wFIndicatorBaseViewModel.ConditionRule1),
				new JProperty("ConditionRule2", wFIndicatorBaseViewModel.ConditionRule2),
				new JProperty("ConditionRule3", wFIndicatorBaseViewModel.ConditionRule3),
				new JProperty("ConditionRule4", wFIndicatorBaseViewModel.ConditionRule4),
				new JProperty("ConditionRule5", wFIndicatorBaseViewModel.ConditionRule5),
				new JProperty("ConditionRule6", wFIndicatorBaseViewModel.ConditionRule6),
				new JProperty("ConditionRule7", wFIndicatorBaseViewModel.ConditionRule7),
				new JProperty("ConditionRule8", wFIndicatorBaseViewModel.ConditionRule8),

				new JProperty("MaskSignal1", wFIndicatorBaseViewModel.MaskSignal1),
				new JProperty("MaskSignal2", wFIndicatorBaseViewModel.MaskSignal2),
				new JProperty("MaskSignal3", wFIndicatorBaseViewModel.MaskSignal3),
				new JProperty("MaskSignal4", wFIndicatorBaseViewModel.MaskSignal4),
				new JProperty("MaskSignal5", wFIndicatorBaseViewModel.MaskSignal5),
				new JProperty("MaskSignal6", wFIndicatorBaseViewModel.MaskSignal6),
				new JProperty("MaskSignal7", wFIndicatorBaseViewModel.MaskSignal7),
				new JProperty("MaskSignal8", wFIndicatorBaseViewModel.MaskSignal8),

				new JProperty("StateSignalName1", wFIndicatorBaseViewModel.StateSignalName1),
				new JProperty("StateSignalName2", wFIndicatorBaseViewModel.StateSignalName2),
				new JProperty("StateSignalName3", wFIndicatorBaseViewModel.StateSignalName3),
				new JProperty("StateSignalName4", wFIndicatorBaseViewModel.StateSignalName4),
				new JProperty("StateSignalName5", wFIndicatorBaseViewModel.StateSignalName5),
				new JProperty("StateSignalName6", wFIndicatorBaseViewModel.StateSignalName6),
				new JProperty("StateSignalName7", wFIndicatorBaseViewModel.StateSignalName7),
				new JProperty("StateSignalName8", wFIndicatorBaseViewModel.StateSignalName8)
			});
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMIIndicatorBaseViewModel wFIndicatorBaseViewModel = source as HMIIndicatorBaseViewModel;

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedConditionSignals(wFIndicatorBaseViewModel.ConditionSignals)
			});
		}
	}
}
