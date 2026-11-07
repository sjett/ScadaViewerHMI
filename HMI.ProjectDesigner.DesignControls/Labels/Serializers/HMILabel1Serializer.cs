// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Labels.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Labels.Serializers
{
	public class HMILabel1Serializer : HMIStaticStatesControlBaseSerializer
	{
		public HMILabel1Serializer(ISerializerFactory serializerFactory, IControlFactory controlFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMILabel1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMILabel1ViewModel node = target as HMILabel1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SymbolicText = ((string?)properties["SymbolicText"]) ?? node.SymbolicText;

            node.TextAlignment = properties["TextAlignment"]?.ToObject<TextAlignment?>() ?? node.TextAlignment;

			if (((string?)properties["BorderThickness"]) != null)
				node.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

			if ((string?)properties["BorderBrush"] != null)
				node.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

            node.HorizontalContentAlignment = properties["HorizontalContentAlignment"]?.ToObject<HorizontalAlignment?>() ?? node.HorizontalContentAlignment;

            node.VerticalContentAlignment = properties["VerticalContentAlignment"]?.ToObject<VerticalAlignment?>() ?? node.VerticalContentAlignment;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMILabel1ViewModel wFLabel1ViewModel = source as HMILabel1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFLabel1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SymbolicText", wFLabel1ViewModel.SymbolicText),
				new JProperty("TextAlignment", wFLabel1ViewModel.TextAlignment),
				new JProperty("BorderThickness", wFLabel1ViewModel.BorderThickness.ToString()),
				new JProperty("BorderBrush", wFLabel1ViewModel.BorderBrush?.ToString()),
				new JProperty("FontFamily", wFLabel1ViewModel.FontFamily),
				new JProperty("FontSize", wFLabel1ViewModel.FontSize),
				new JProperty("FontStretch", wFLabel1ViewModel.FontStretch),
				new JProperty("FontStyle", wFLabel1ViewModel.FontStyle),
				new JProperty("FontWeight", wFLabel1ViewModel.FontWeight),
				new JProperty("HorizontalContentAlignment", wFLabel1ViewModel.HorizontalContentAlignment),
				new JProperty("VerticalContentAlignment", wFLabel1ViewModel.VerticalContentAlignment)
			});
		}
	}
}
