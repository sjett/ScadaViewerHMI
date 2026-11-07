// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public class HMITextDisplay1Serializer : HMIIOControlBaseSerializer
	{
		public HMITextDisplay1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMITextDisplay1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMITextDisplay1ViewModel node = target as HMITextDisplay1ViewModel;
			base.DeserializeProperties(properties, node, version);

            node.BinaryMask = ((bool?)properties["BinaryMask"]) ?? node.BinaryMask;

            node.ConditionRule1 = ((string?)properties["ConditionRule1"]) ?? node.ConditionRule1;
            node.ConditionRule2 = ((string?)properties["ConditionRule2"]) ?? node.ConditionRule2;
            node.ConditionRule3 = ((string?)properties["ConditionRule3"]) ?? node.ConditionRule3;
            node.ConditionRule4 = ((string?)properties["ConditionRule4"]) ?? node.ConditionRule4;

            node.MaskSignal1 = ((int?)properties["MaskSignal1"]) ?? node.MaskSignal1;
            node.MaskSignal2 = ((int?)properties["MaskSignal2"]) ?? node.MaskSignal2;
            node.MaskSignal3 = ((int?)properties["MaskSignal3"]) ?? node.MaskSignal3;
            node.MaskSignal4 = ((int?)properties["MaskSignal4"]) ?? node.MaskSignal4;

            if (((string?)properties["Foreground"]) != null)
                node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            node.LimitSignalName1 = ((string?)properties["LimitSignalName1"]) ?? node.LimitSignalName1;
            node.LimitSignalName2 = ((string?)properties["LimitSignalName2"]) ?? node.LimitSignalName2;
            node.LimitSignalName3 = ((string?)properties["LimitSignalName3"]) ?? node.LimitSignalName3;
            node.LimitSignalName4 = ((string?)properties["LimitSignalName4"]) ?? node.LimitSignalName4;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			HMITextDisplay1ViewModel wFTextDisplay1ViewModel = target as HMITextDisplay1ViewModel;
			base.DeserializeCollections(propertyCollectionsElement, wFTextDisplay1ViewModel, version);
			this.DeserializeConditionSignals(propertyCollectionsElement, wFTextDisplay1ViewModel, wFTextDisplay1ViewModel.ConditionSignals, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMITextDisplay1ViewModel wFTextDisplay1ViewModel = source as HMITextDisplay1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFTextDisplay1ViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("BinaryMask", wFTextDisplay1ViewModel.BinaryMask),
				new JProperty("ConditionRule1", wFTextDisplay1ViewModel.ConditionRule1),
				new JProperty("ConditionRule2", wFTextDisplay1ViewModel.ConditionRule2),
				new JProperty("ConditionRule3", wFTextDisplay1ViewModel.ConditionRule3),
				new JProperty("ConditionRule4", wFTextDisplay1ViewModel.ConditionRule4),
				new JProperty("MaskSignal1", wFTextDisplay1ViewModel.MaskSignal1),
				new JProperty("MaskSignal2", wFTextDisplay1ViewModel.MaskSignal2),
				new JProperty("MaskSignal3", wFTextDisplay1ViewModel.MaskSignal3),
				new JProperty("MaskSignal4", wFTextDisplay1ViewModel.MaskSignal4),
				new JProperty("LimitSignalName1", wFTextDisplay1ViewModel.LimitSignalName1),
				new JProperty("LimitSignalName2", wFTextDisplay1ViewModel.LimitSignalName2),
				new JProperty("LimitSignalName3", wFTextDisplay1ViewModel.LimitSignalName3),
				new JProperty("LimitSignalName4", wFTextDisplay1ViewModel.LimitSignalName4),
				new JProperty("Foreground", wFTextDisplay1ViewModel.Foreground?.ToString())
			});
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMITextDisplay1ViewModel wFTextDisplay1ViewModel = source as HMITextDisplay1ViewModel;

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedConditionSignals(wFTextDisplay1ViewModel.ConditionSignals)
			});
		}
	}
}
