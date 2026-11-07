// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Indicators.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Indicators.Serializers
{
	public class HMICompass1Serializer : HMIIndicatorBaseSerializer
	{
		public HMICompass1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMICompass1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMICompass1ViewModel node = target as HMICompass1ViewModel;
			base.DeserializeProperties(properties, node, version);

			if ((string?)properties["Background"] != null)
				node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if ((string?)properties["BorderBrush"] != null)
                node.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

            if ((string?)properties["ChromeFrontBrush"] != null)
                node.ChromeFrontBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["ChromeFrontBrush"]!)).GetAsFrozen();

			node.CompassAngle = ((double?)properties["CompassAngle"]) ?? node.CompassAngle;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

            node.MajorLabelsFontSize = ((int?)properties["MajorLabelsFontSize"]) ?? node.MajorLabelsFontSize;

            if ((string?)properties["MajorLabelsForeground"] != null)
                node.MajorLabelsForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["MajorLabelsForeground"]!)).GetAsFrozen();

            node.MajorLabelsVisible = ((bool?)properties["MajorLabelsVisible"]) ?? node.MajorLabelsVisible;

            if ((string?)properties["MajorScaleFillBrush"] != null)
                node.MajorScaleFillBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["MajorScaleFillBrush"]!)).GetAsFrozen();

            node.MinorLabelsFontSize = ((int?)properties["MinorLabelsFontSize"]) ?? node.MinorLabelsFontSize;

            if ((string?)properties["MinorLabelsForeground"] != null)
                node.MinorLabelsForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["MinorLabelsForeground"]!)).GetAsFrozen();

            node.MinorLabelsVisible = ((bool?)properties["MinorLabelsVisible"]) ?? node.MinorLabelsVisible;

            if ((string?)properties["MinorScaleFillBrush"] != null)
                node.MinorScaleFillBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["MinorScaleFillBrush"]!)).GetAsFrozen();

            if ((string?)properties["PointerFillBrush"] != null)
                node.PointerFillBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["PointerFillBrush"]!)).GetAsFrozen();

            if ((string?)properties["PointerHeadFillBrush"] != null)
                node.PointerHeadFillBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["PointerHeadFillBrush"]!)).GetAsFrozen();

            node.PointerIsInverted = ((bool?)properties["PointerIsInverted"]) ?? node.PointerIsInverted;

            if ((string?)properties["PointerStrokeBrush"] != null)
                node.PointerStrokeBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["PointerStrokeBrush"]!)).GetAsFrozen();

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? node.UseLocalColors;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMICompass1ViewModel wFCompass1ViewModel = source as HMICompass1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFCompass1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Background", wFCompass1ViewModel.Background?.ToString()),
				new JProperty("BorderBrush", wFCompass1ViewModel.BorderBrush?.ToString()),
				new JProperty("ChromeFrontBrush", wFCompass1ViewModel.ChromeFrontBrush?.ToString()),
				new JProperty("CompassAngle", wFCompass1ViewModel.CompassAngle),
				new JProperty("FontFamily", wFCompass1ViewModel.FontFamily),
				new JProperty("FontStretch", wFCompass1ViewModel.FontStretch),
				new JProperty("FontStyle", wFCompass1ViewModel.FontStyle),
				new JProperty("FontWeight", wFCompass1ViewModel.FontWeight),
				new JProperty("MajorLabelsFontSize", wFCompass1ViewModel.MajorLabelsFontSize),
				new JProperty("MajorLabelsForeground", wFCompass1ViewModel.MajorLabelsForeground?.ToString()),
				new JProperty("MajorLabelsVisible", wFCompass1ViewModel.MajorLabelsVisible),
				new JProperty("MajorScaleFillBrush", wFCompass1ViewModel.MajorScaleFillBrush?.ToString()),
				new JProperty("MinorLabelsFontSize", wFCompass1ViewModel.MinorLabelsFontSize),
				new JProperty("MinorLabelsForeground", wFCompass1ViewModel.MinorLabelsForeground?.ToString()),
				new JProperty("MinorLabelsVisible", wFCompass1ViewModel.MinorLabelsVisible),
				new JProperty("MinorScaleFillBrush", wFCompass1ViewModel.MinorScaleFillBrush?.ToString()),
				new JProperty("PointerFillBrush", wFCompass1ViewModel.PointerFillBrush?.ToString()),
				new JProperty("PointerHeadFillBrush", wFCompass1ViewModel.PointerHeadFillBrush?.ToString()),
				new JProperty("PointerIsInverted", wFCompass1ViewModel.PointerIsInverted),
				new JProperty("PointerStrokeBrush", wFCompass1ViewModel.PointerStrokeBrush?.ToString()),
				new JProperty("SignalName", wFCompass1ViewModel.SignalName),
				new JProperty("UseLocalColors", wFCompass1ViewModel.UseLocalColors)
			});
		}
	}
}
