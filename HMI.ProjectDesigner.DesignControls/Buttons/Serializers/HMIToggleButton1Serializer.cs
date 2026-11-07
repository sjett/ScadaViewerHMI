// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public class HMIToggleButton1Serializer : HMIToggleButtonBaseSerializer
	{
		public HMIToggleButton1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIToggleButton1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMIToggleButton1ViewModel node = target as HMIToggleButton1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SymbolicText = ((string?)properties["SymbolicText"]) ?? node.SymbolicText;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

			if((string?)properties["Foreground"] != null)
				node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            if ((string?)properties["Background"] != null)
                node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            node.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? node.UseLocalColors;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIToggleButton1ViewModel wFToggleButton1ViewModel = source as HMIToggleButton1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFToggleButton1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SymbolicText", wFToggleButton1ViewModel.SymbolicText),
				new JProperty("FontFamily", wFToggleButton1ViewModel.FontFamily),
				new JProperty("FontSize", wFToggleButton1ViewModel.FontSize),
				new JProperty("FontStretch", wFToggleButton1ViewModel.FontStretch),
				new JProperty("FontStyle", wFToggleButton1ViewModel.FontStyle),
				new JProperty("FontWeight", wFToggleButton1ViewModel.FontWeight),
				new JProperty("Foreground", wFToggleButton1ViewModel.Foreground?.ToString()),
				new JProperty("Background", wFToggleButton1ViewModel.Background?.ToString()),
				new JProperty("UseLocalColors", wFToggleButton1ViewModel.UseLocalColors)
			});
		}
	}
}
