// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.Navigation.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Navigation.Serializers
{
	public class HMINavigationToggle1Serializer : CommonControlSerializer
	{
		public HMINavigationToggle1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMINavigationToggle1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMINavigationToggle1ViewModel node = target as HMINavigationToggle1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SecurityAuthorizationName = ((string?)properties!["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

			node.SecurityDenyAccessBehaviour = properties!["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.FontFamily = ((string?)properties!["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties!["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties!["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties!["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties!["FontWeight"]) ?? node.FontWeight;

			if (properties!["Foreground"] != null)
				node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties!["Foreground"]!)).GetAsFrozen();

			node.IsChecked = ((bool?)properties!["IsChecked"]) ?? node.IsChecked;

            node.MenuGroup = ((string?)properties!["MenuGroup"]) ?? node.MenuGroup;

            node.SymbolicText = ((string?)properties!["SymbolicText"]) ?? node.SymbolicText;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			HMINavigationToggle1ViewModel wFNavigationToggle1ViewModel = target as HMINavigationToggle1ViewModel;

			base.DeserializeCollections(propertyCollectionsElement, wFNavigationToggle1ViewModel, version);

			this.DeserializeNavigationItem(propertyCollectionsElement, wFNavigationToggle1ViewModel, wFNavigationToggle1ViewModel.Navigations, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMINavigationToggle1ViewModel wFNavigationToggle1ViewModel = source as HMINavigationToggle1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFNavigationToggle1ViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SecurityAuthorizationName", wFNavigationToggle1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFNavigationToggle1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("FontFamily", wFNavigationToggle1ViewModel.FontFamily),
				new JProperty("FontSize", wFNavigationToggle1ViewModel.FontSize),
				new JProperty("FontStretch", wFNavigationToggle1ViewModel.FontStretch),
				new JProperty("FontStyle", wFNavigationToggle1ViewModel.FontStyle),
				new JProperty("FontWeight", wFNavigationToggle1ViewModel.FontWeight),
				new JProperty("Foreground", wFNavigationToggle1ViewModel.Foreground.ToString()),
				new JProperty("IsChecked", wFNavigationToggle1ViewModel.IsChecked),
				new JProperty("MenuGroup", wFNavigationToggle1ViewModel.MenuGroup),
				new JProperty("SymbolicText", wFNavigationToggle1ViewModel.SymbolicText)
			});
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMINavigationToggle1ViewModel wFNavigationToggle1ViewModel = source as HMINavigationToggle1ViewModel;
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedNavigationItems(wFNavigationToggle1ViewModel.Navigations)
			});
		}
	}
}
