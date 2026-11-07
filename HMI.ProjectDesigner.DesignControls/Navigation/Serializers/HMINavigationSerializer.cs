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
	public class HMINavigationSerializer : CommonControlSerializer
	{
		public HMINavigationSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMINavigationViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMINavigationViewModel? node = target as HMINavigationViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SecurityAuthorizationName = ((string?)properties!["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

			node.SecurityDenyAccessBehaviour = properties!["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

			node.FontFamily = ((string?)properties!["FontFamily"]) ?? node.FontFamily;

			node.FontSize = ((double?)properties!["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties!["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties!["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties!["FontWeight"]) ?? node.FontWeight;

			if(properties!["Foreground"] != null)
				node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(((string)properties!["Foreground"]!))).GetAsFrozen();

            node.SymbolicText = ((string?)properties!["SymbolicText"]) ?? node.SymbolicText;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			HMINavigationViewModel wFNavigationViewModel = target as HMINavigationViewModel;

			base.DeserializeCollections(propertyCollectionsElement, wFNavigationViewModel, version);

			this.DeserializeNavigationItem(propertyCollectionsElement, wFNavigationViewModel, wFNavigationViewModel.Navigations, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMINavigationViewModel wFNavigationViewModel = source as HMINavigationViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFNavigationViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("SecurityAuthorizationName", wFNavigationViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFNavigationViewModel.SecurityDenyAccessBehaviour),
				new JProperty("FontFamily", wFNavigationViewModel.FontFamily),
				new JProperty("FontSize", wFNavigationViewModel.FontSize),
				new JProperty("FontStretch", wFNavigationViewModel.FontStretch),
				new JProperty("FontStyle", wFNavigationViewModel.FontStyle),
				new JProperty("FontWeight", wFNavigationViewModel.FontWeight),
				new JProperty("Foreground", wFNavigationViewModel.Foreground.ToString()),
				new JProperty("SymbolicText", wFNavigationViewModel.SymbolicText)
			});
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMINavigationViewModel wFNavigationViewModel = source as HMINavigationViewModel;

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedNavigationItems(wFNavigationViewModel.Navigations)
			});
		}
	}
}
