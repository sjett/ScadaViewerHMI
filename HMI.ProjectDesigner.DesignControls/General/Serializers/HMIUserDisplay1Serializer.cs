// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
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
	public class HMIUserDisplay1Serializer : CommonControlSerializer
	{
		public HMIUserDisplay1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIUserDisplay1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIUserDisplay1ViewModel node = target as HMIUserDisplay1ViewModel;
			base.DeserializeProperties(properties, node, version);

            node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((int?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

            node.HorizontalContentAlignment = properties["HorizontalContentAlignment"]?.ToObject<HorizontalAlignment?>() ?? node.HorizontalContentAlignment;

            node.VerticalContentAlignment = properties["VerticalContentAlignment"]?.ToObject<VerticalAlignment?>() ?? node.VerticalContentAlignment;

            if ((string?)properties["Background"] != null)
                node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if ((string?)properties["Foreground"] != null)
                node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIUserDisplay1ViewModel wFUserDisplay1ViewModel = source as HMIUserDisplay1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFUserDisplay1ViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SecurityAuthorizationName", wFUserDisplay1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFUserDisplay1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("FontFamily", wFUserDisplay1ViewModel.FontFamily),
				new JProperty("FontSize", wFUserDisplay1ViewModel.FontSize),
				new JProperty("FontStretch", wFUserDisplay1ViewModel.FontStretch),
				new JProperty("FontStyle", wFUserDisplay1ViewModel.FontStyle),
				new JProperty("FontWeight", wFUserDisplay1ViewModel.FontWeight),
				new JProperty("HorizontalContentAlignment", wFUserDisplay1ViewModel.HorizontalContentAlignment),
				new JProperty("VerticalContentAlignment", wFUserDisplay1ViewModel.VerticalContentAlignment),
				new JProperty("Background", wFUserDisplay1ViewModel.Background?.ToString()),
				new JProperty("Foreground", wFUserDisplay1ViewModel.Foreground?.ToString())
			});
		}
	}
}
