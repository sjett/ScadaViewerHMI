// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.OpenDialog.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.OpenDialog.Serializers
{
	public class HMIOpenDialogSerializer : CommonControlSerializer
	{
		public HMIOpenDialogSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIOpenDialogViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIOpenDialogViewModel item = target as HMIOpenDialogViewModel;
			base.DeserializeProperties(properties, item, version);

			item.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? item.UseLocalColors;

			if (((string?)properties["Background"]) != null)
				item.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if (((string?)properties["Foreground"]) != null)
                item.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            if (((string?)properties["BorderBrush"]) != null)
                item.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

			if (((string?)properties["BorderThickness"]) != null)
				item.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

            item.FontFamily = ((string?)properties["FontFamily"]) ?? item.FontFamily;

            item.FontSize = ((int?)properties["FontSize"]) ?? item.FontSize;

            item.FontStretch = ((string?)properties["FontStretch"]) ?? item.FontStretch;

            item.FontStyle = ((string?)properties["FontStyle"]) ?? item.FontStyle;

            item.FontWeight = ((string?)properties["FontWeight"]) ?? item.FontWeight;

            item.SymbolicText = ((string?)properties["SymbolicText"]) ?? item.SymbolicText;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIOpenDialogViewModel wFOpenDialogViewModel = source as HMIOpenDialogViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFOpenDialogViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("UseLocalColors", wFOpenDialogViewModel.UseLocalColors),
				new JProperty("Background", wFOpenDialogViewModel.Background?.ToString()),
				new JProperty("Foreground", wFOpenDialogViewModel.Foreground?.ToString()),
				new JProperty("BorderBrush", wFOpenDialogViewModel.BorderBrush?.ToString()),
				new JProperty("BorderThickness", wFOpenDialogViewModel.BorderThickness.ToString()),
				new JProperty("FontFamily", wFOpenDialogViewModel.FontFamily),
				new JProperty("FontSize", wFOpenDialogViewModel.FontSize),
				new JProperty("FontStretch", wFOpenDialogViewModel.FontStretch),
				new JProperty("FontStyle", wFOpenDialogViewModel.FontStyle),
				new JProperty("FontWeight", wFOpenDialogViewModel.FontWeight),
				new JProperty("SymbolicText", wFOpenDialogViewModel.SymbolicText)
			});
		}
	}
}
