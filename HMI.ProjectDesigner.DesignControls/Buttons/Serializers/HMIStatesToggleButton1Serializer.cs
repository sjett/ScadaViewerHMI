// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public class HMIStatesToggleButton1Serializer : HMIStatesButtonBaseSerializer
	{
		public HMIStatesToggleButton1Serializer(ISerializerFactory serializerFactory, IControlFactory controlFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, controlFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIStatesToggleButton1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIStatesToggleButton1ViewModel node = target as HMIStatesToggleButton1ViewModel;
			base.DeserializeProperties(properties, node, version);

            if ((string?)properties["CheckedBackground"] != null)
                node.CheckedBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["CheckedBackground"]!)).GetAsFrozen();

			node.IconImageChecked = ((string?)properties["IconImageChecked"]) ?? node.IconImageChecked;

            node.IconImageUnchecked = ((string?)properties["IconImageUnchecked"]) ?? node.IconImageUnchecked;

            node.IsTipModeEnabled = ((bool?)properties["IsTipModeEnabled"]) ?? node.IsTipModeEnabled;

            node.StateSignalName = ((string?)properties["StateSignalName"]) ?? node.StateSignalName;

            node.CheckedSymbolicText = ((string?)properties["CheckedSymbolicText"]) ?? node.CheckedSymbolicText;

            node.UncheckedSymbolicText = ((string?)properties["UncheckedSymbolicText"]) ?? node.UncheckedSymbolicText;

            node.Value1 = ((double?)properties["Value1"]) ?? node.Value1;

            node.Value2 = ((double?)properties["Value2"]) ?? node.Value2;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIStatesToggleButton1ViewModel wFStatesToggleButton1ViewModel = source as HMIStatesToggleButton1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFStatesToggleButton1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("CheckedBackground", wFStatesToggleButton1ViewModel.CheckedBackground?.ToString()),
				new JProperty("IconImageChecked", wFStatesToggleButton1ViewModel.IconImageChecked),
				new JProperty("IconImageUnchecked", wFStatesToggleButton1ViewModel.IconImageUnchecked),
				new JProperty("IsTipModeEnabled", wFStatesToggleButton1ViewModel.IsTipModeEnabled),
				new JProperty("StateSignalName", wFStatesToggleButton1ViewModel.StateSignalName),
				new JProperty("CheckedSymbolicText", wFStatesToggleButton1ViewModel.CheckedSymbolicText),
				new JProperty("UncheckedSymbolicText", wFStatesToggleButton1ViewModel.UncheckedSymbolicText),
				new JProperty("Value1", wFStatesToggleButton1ViewModel.Value1),
				new JProperty("Value2", wFStatesToggleButton1ViewModel.Value2)
			});
		}
	}
}
