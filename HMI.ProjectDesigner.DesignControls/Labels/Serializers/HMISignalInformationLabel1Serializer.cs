// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Labels.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Labels.Serializers
{
	public class HMISignalInformationLabel1Serializer : CommonControlSerializer
	{
		public HMISignalInformationLabel1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMISignalInformationLabel1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMISignalInformationLabel1ViewModel item = target as HMISignalInformationLabel1ViewModel;
			base.DeserializeProperties(properties, item, version);

			item.SignalName = ((string?)properties["SignalName"]) ?? item.SignalName;

            item.ObjectID = ((string?)properties["ObjectID"]) ?? item.ObjectID;

            item.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? item.SignalPrefix;

            item.SignalInformation = properties["SignalInformation"]?.ToObject<SignalInformationField?>() ?? item.SignalInformation;

            item.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? item.UseLocalColors;

            item.FontFamily = ((string?)properties["FontFamily"]) ?? item.FontFamily;

            item.FontSize = ((double?)properties["FontSize"]) ?? item.FontSize;

            item.FontStretch = ((string?)properties["FontStretch"]) ?? item.FontStretch;

            item.FontStyle = ((string?)properties["FontStyle"]) ?? item.FontStyle;

            item.FontWeight = ((string?)properties["FontWeight"]) ?? item.FontWeight;

            item.HorizontalContentAlignment = properties["HorizontalContentAlignment"]?.ToObject<HorizontalAlignment?>() ?? item.HorizontalContentAlignment;

            item.VerticalContentAlignment = properties["VerticalContentAlignment"]?.ToObject<VerticalAlignment?>() ?? item.VerticalContentAlignment;

			if (((string?)properties["BorderBrush"]) != null)
				item.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

            if (((string?)properties["Foreground"]) != null)
                item.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            if (((string?)properties["Background"]) != null)
                item.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if (((string?)properties["BorderThickness"]) != null)
                item.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMISignalInformationLabel1ViewModel wFSignalInformationLabel1ViewModel = source as HMISignalInformationLabel1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFSignalInformationLabel1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SignalName", wFSignalInformationLabel1ViewModel.SignalName),
				new JProperty("ObjectID", wFSignalInformationLabel1ViewModel.ObjectID),
				new JProperty("SignalPrefix", wFSignalInformationLabel1ViewModel.SignalPrefix),
				new JProperty("SignalInformation", wFSignalInformationLabel1ViewModel.SignalInformation),
				new JProperty("UseLocalColors", wFSignalInformationLabel1ViewModel.UseLocalColors),
				new JProperty("FontFamily", wFSignalInformationLabel1ViewModel.FontFamily),
				new JProperty("FontSize", wFSignalInformationLabel1ViewModel.FontSize),
				new JProperty("FontStretch", wFSignalInformationLabel1ViewModel.FontStretch),
				new JProperty("FontStyle", wFSignalInformationLabel1ViewModel.FontStyle),
				new JProperty("FontWeight", wFSignalInformationLabel1ViewModel.FontWeight),
				new JProperty("HorizontalContentAlignment", wFSignalInformationLabel1ViewModel.HorizontalContentAlignment),
				new JProperty("VerticalContentAlignment", wFSignalInformationLabel1ViewModel.VerticalContentAlignment),
				new JProperty("BorderBrush", wFSignalInformationLabel1ViewModel.BorderBrush?.ToString()),
				new JProperty("Foreground", wFSignalInformationLabel1ViewModel.Foreground?.ToString()),
				new JProperty("Background", wFSignalInformationLabel1ViewModel.Background?.ToString()),
				new JProperty("BorderThickness", wFSignalInformationLabel1ViewModel.BorderThickness.ToString())
			});
		}
	}
}
