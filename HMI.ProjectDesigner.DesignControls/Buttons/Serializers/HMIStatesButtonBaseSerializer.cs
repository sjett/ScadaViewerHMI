// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public class HMIStatesButtonBaseSerializer : HMIStaticStatesControlBaseSerializer
	{
		public HMIStatesButtonBaseSerializer(ISerializerFactory serializerFactory, IControlFactory controlFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIStatesButtonBaseViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIStatesButtonBaseViewModel node = target as HMIStatesButtonBaseViewModel;
			base.DeserializeProperties(properties, node, version);

            if ((string?)properties["BorderBrush"] != null)
                node.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

            if ((string?)properties["ChromeFrontBrush"] != null)
                node.ChromeFrontBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ChromeFrontBrush"]!)).GetAsFrozen();

            if ((string?)properties["MouseOverBrush"] != null)
                node.MouseOverBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["MouseOverBrush"]!)).GetAsFrozen();

            if ((string?)properties["BorderThickness"] != null)
                node.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

            if ((string?)properties["CornerRadius"] != null)
                node.CornerRadius = (CornerRadius)new CornerRadiusToStringConverter().ConvertBack((string)properties["CornerRadius"]!, null, null, null);

            if ((string?)properties["Padding"] != null)
                node.Padding = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["Padding"]!, null, null, null);

            if ((string?)properties["IconMargin"] != null)
                node.IconMargin = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["IconMargin"]!, null, null, null);

			node.IconMaxSize = ((int?)properties["IconMaxSize"]) ?? node.IconMaxSize;

            node.IconOpacity = ((double?)properties["IconOpacity"]) ?? node.IconOpacity;

			node.IconPlacement = properties["IconPlacement"]?.ToObject<ButtonIconPlacement?>() ?? node.IconPlacement;

            node.IconStretch = properties["IconStretch"]?.ToObject<Stretch?>() ?? node.IconStretch;

            node.EnableMask = ((int?)properties["EnableMask"]) ?? node.EnableMask;

            node.EnableSignalName = ((string?)properties["EnableSignalName"]) ?? node.EnableSignalName;

            node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.SymbolicText = ((string?)properties["SymbolicText"]) ?? node.SymbolicText;

            node.WriteSecure = ((bool?)properties["WriteSecure"]) ?? node.WriteSecure;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIStatesButtonBaseViewModel wFStatesButtonBaseViewModel = source as HMIStatesButtonBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFStatesButtonBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("BorderBrush", wFStatesButtonBaseViewModel.BorderBrush?.ToString()),
				new JProperty("ChromeFrontBrush", wFStatesButtonBaseViewModel.ChromeFrontBrush?.ToString()),
				new JProperty("MouseOverBrush", wFStatesButtonBaseViewModel.MouseOverBrush?.ToString()),
				new JProperty("BorderThickness", wFStatesButtonBaseViewModel.BorderThickness.ToString()),
				new JProperty("CornerRadius", wFStatesButtonBaseViewModel.CornerRadius.ToString()),
				new JProperty("Padding", wFStatesButtonBaseViewModel.Padding.ToString()),
				new JProperty("IconMargin", wFStatesButtonBaseViewModel.IconMargin.ToString()),
				new JProperty("IconMaxSize", wFStatesButtonBaseViewModel.IconMaxSize),
				new JProperty("IconOpacity", wFStatesButtonBaseViewModel.IconOpacity),
				new JProperty("IconPlacement", wFStatesButtonBaseViewModel.IconPlacement),
				new JProperty("IconStretch", wFStatesButtonBaseViewModel.IconStretch),
				new JProperty("EnableMask", wFStatesButtonBaseViewModel.EnableMask),
				new JProperty("EnableSignalName", wFStatesButtonBaseViewModel.EnableSignalName),
				new JProperty("VisibilityMask", wFStatesButtonBaseViewModel.VisibilityMask),
				new JProperty("VisibilitySignalName", wFStatesButtonBaseViewModel.VisibilitySignalName),
				new JProperty("SignalName", wFStatesButtonBaseViewModel.SignalName),
				new JProperty("SymbolicText", wFStatesButtonBaseViewModel.SymbolicText),
				new JProperty("WriteSecure", wFStatesButtonBaseViewModel.WriteSecure),
				new JProperty("FontFamily", wFStatesButtonBaseViewModel.FontFamily),
				new JProperty("FontSize", wFStatesButtonBaseViewModel.FontSize),
				new JProperty("FontStretch", wFStatesButtonBaseViewModel.FontStretch),
				new JProperty("FontStyle", wFStatesButtonBaseViewModel.FontStyle),
				new JProperty("FontWeight", wFStatesButtonBaseViewModel.FontWeight)
			});
		}
	}
}
