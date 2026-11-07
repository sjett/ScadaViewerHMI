// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public class HMINumericDisplay2Serializer : HMIStaticStatesControlBaseSerializer
	{
		public HMINumericDisplay2Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMINumericDisplay2ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMINumericDisplay2ViewModel node = target as HMINumericDisplay2ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.Digits = ((int?)properties["Digits"]) ?? node.Digits;

			node.DisplayType = properties["DisplayType"]?.ToObject<HMINumericDisplay2DisplayType?>() ?? node.DisplayType;

            node.Precision = ((int?)properties["Precision"]) ?? node.Precision;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMINumericDisplay2ViewModel wFNumericDisplay2ViewModel = source as HMINumericDisplay2ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFNumericDisplay2ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Digits", wFNumericDisplay2ViewModel.Digits),
				new JProperty("DisplayType", wFNumericDisplay2ViewModel.DisplayType),
				new JProperty("Precision", wFNumericDisplay2ViewModel.Precision),
				new JProperty("SignalName", wFNumericDisplay2ViewModel.SignalName),
				new JProperty("FontFamily", wFNumericDisplay2ViewModel.FontFamily),
				new JProperty("FontSize", wFNumericDisplay2ViewModel.FontSize),
				new JProperty("FontStretch", wFNumericDisplay2ViewModel.FontStretch),
				new JProperty("FontStyle", wFNumericDisplay2ViewModel.FontStyle),
				new JProperty("FontWeight", wFNumericDisplay2ViewModel.FontWeight)
			});
		}
	}
}
