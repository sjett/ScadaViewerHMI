// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
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
	public class HMIUserInformation1Serializer : CommonControlSerializer
	{
		public HMIUserInformation1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIUserInformation1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIUserInformation1ViewModel item = target as HMIUserInformation1ViewModel;
			base.DeserializeProperties(properties, item, version);

            item.UserInformation = properties["UserInformation"]?.ToObject<UserInformationField?>() ?? item.UserInformation;

            item.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? item.UseLocalColors;

            item.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? item.SecurityAuthorizationName;

            item.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? item.SecurityDenyAccessBehaviour;

            item.FontFamily = ((string?)properties["FontFamily"]) ?? item.FontFamily;

            item.FontSize = ((double?)properties["FontSize"]) ?? item.FontSize;

            item.FontStretch = ((string?)properties["FontStretch"]) ?? item.FontStretch;

            item.FontStyle = ((string?)properties["FontStyle"]) ?? item.FontStyle;

            item.FontWeight = ((string?)properties["FontWeight"]) ?? item.FontWeight;

            item.HorizontalContentAlignment = properties["HorizontalContentAlignment"]?.ToObject<HorizontalAlignment?>() ?? item.HorizontalContentAlignment;

            item.VerticalContentAlignment = properties["VerticalContentAlignment"]?.ToObject<VerticalAlignment?>() ?? item.VerticalContentAlignment;

            if ((string?)properties["Background"] != null)
                item.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if ((string?)properties["Foreground"] != null)
                item.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            if ((string?)properties["BorderBrush"] != null)
                item.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

            if ((string?)properties["BorderThickness"] != null)
                item.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIUserInformation1ViewModel wFUserInformation1ViewModel = source as HMIUserInformation1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFUserInformation1ViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("UserInformation", wFUserInformation1ViewModel.UserInformation),
				new JProperty("UseLocalColors", wFUserInformation1ViewModel.UseLocalColors),
				new JProperty("Background", wFUserInformation1ViewModel.Background?.ToString()),
				new JProperty("Foreground", wFUserInformation1ViewModel.Foreground?.ToString()),
				new JProperty("BorderBrush", wFUserInformation1ViewModel.BorderBrush?.ToString()),
				new JProperty("BorderThickness", wFUserInformation1ViewModel.BorderThickness.ToString()),
				new JProperty("FontFamily", wFUserInformation1ViewModel.FontFamily),
				new JProperty("FontSize", wFUserInformation1ViewModel.FontSize),
				new JProperty("FontStretch", wFUserInformation1ViewModel.FontStretch),
				new JProperty("FontStyle", wFUserInformation1ViewModel.FontStyle),
				new JProperty("FontWeight", wFUserInformation1ViewModel.FontWeight),
				new JProperty("HorizontalContentAlignment", wFUserInformation1ViewModel.HorizontalContentAlignment),
				new JProperty("VerticalContentAlignment", wFUserInformation1ViewModel.VerticalContentAlignment),
				new JProperty("SecurityAuthorizationName", wFUserInformation1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFUserInformation1ViewModel.SecurityDenyAccessBehaviour)
			});
		}
	}
}
