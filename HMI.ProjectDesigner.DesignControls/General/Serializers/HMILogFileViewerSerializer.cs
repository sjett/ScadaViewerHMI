// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.General.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class HMILogFileViewerSerializer : CommonControlSerializer
	{
		public HMILogFileViewerSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMILogFileViewerViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			HMILogFileViewerViewModel item = target as HMILogFileViewerViewModel;
			base.DeserializeProperties(properties, item, version);

            if ((string?)properties["Background"] != null)
                item.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if ((string?)properties["BorderBrush"] != null)
                item.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

            if ((string?)properties["BorderThickness"] != null)
                item.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

			item.FontFamily = ((string?)properties["FontFamily"]) ?? item.FontFamily;

            item.FontSize = ((int?)properties["FontSize"]) ?? item.FontSize;

            item.FontStretch = ((string?)properties["FontStretch"]) ?? item.FontStretch;

            item.FontStyle = ((string?)properties["FontStyle"]) ?? item.FontStyle;

            item.FontWeight = ((string?)properties["FontWeight"]) ?? item.FontWeight;

            if ((string?)properties["Foreground"] != null)
                item.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

			item.PopupModality = properties["PopupModality"]?.ToObject<DialogModality?>() ?? item.PopupModality;

            item.SymbolicText = ((string?)properties["SymbolicText"]) ?? item.SymbolicText;

            item.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? item.UseLocalColors;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMILogFileViewerViewModel wFLogFileViewerViewModel = source as HMILogFileViewerViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFLogFileViewerViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Background", wFLogFileViewerViewModel.Background?.ToString()),
				new JProperty("BorderBrush", wFLogFileViewerViewModel.BorderBrush?.ToString()),
				new JProperty("BorderThickness", wFLogFileViewerViewModel.BorderThickness.ToString()),
				new JProperty("FontFamily", wFLogFileViewerViewModel.FontFamily),
				new JProperty("FontSize", wFLogFileViewerViewModel.FontSize),
				new JProperty("FontStretch", wFLogFileViewerViewModel.FontStretch),
				new JProperty("FontStyle", wFLogFileViewerViewModel.FontStyle),
				new JProperty("FontWeight", wFLogFileViewerViewModel.FontWeight),
				new JProperty("Foreground", wFLogFileViewerViewModel.Foreground?.ToString()),
				new JProperty("PopupModality", wFLogFileViewerViewModel.PopupModality),
				new JProperty("SymbolicText", wFLogFileViewerViewModel.SymbolicText),
				new JProperty("UseLocalColors", wFLogFileViewerViewModel.UseLocalColors)
			});
		}
	}
}
