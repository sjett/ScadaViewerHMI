// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public class HMILinkButton1Serializer : CommonControlSerializer
	{
		public HMILinkButton1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMILinkButton1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMILinkButton1ViewModel node = target as HMILinkButton1ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.EnableMask = ((int?)properties!["EnableMask"]) ?? node.EnableMask;

            node.EnableSignalName = ((string?)properties["EnableSignalName"]) ?? node.EnableSignalName;

            node.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? node.VisibilityMask;

            node.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? node.VisibilitySignalName;

            node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

			node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

            node.PopupWidth = ((int?)properties["PopupWidth"]) ?? node.PopupWidth;

            node.PopupHeight = ((int?)properties["PopupHeight"]) ?? node.PopupHeight;

            node.PopupModality = properties["PopupModality"]?.ToObject<DialogModality?>() ?? node.PopupModality;

            node.SymbolicText = ((string?)properties["SymbolicText"]) ?? node.SymbolicText;

            node.Url = ((string?)properties["Url"]) ?? node.Url;

            if ((string?)properties["Foreground"] != null)
                node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            if ((string?)properties["Background"] != null)
                node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            node.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? node.UseLocalColors;

            node.FontFamily = ((string?)properties["FontFamily"]) ?? node.FontFamily;

            node.FontSize = ((double?)properties["FontSize"]) ?? node.FontSize;

            node.FontStretch = ((string?)properties["FontStretch"]) ?? node.FontStretch;

            node.FontStyle = ((string?)properties["FontStyle"]) ?? node.FontStyle;

            node.FontWeight = ((string?)properties["FontWeight"]) ?? node.FontWeight;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMILinkButton1ViewModel wFLinkButton1ViewModel = source as HMILinkButton1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFLinkButton1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Foreground", wFLinkButton1ViewModel.Foreground?.ToString()),
				new JProperty("Background", wFLinkButton1ViewModel.Background?.ToString()),
				new JProperty("UseLocalColors", wFLinkButton1ViewModel.UseLocalColors),
				new JProperty("EnableMask", wFLinkButton1ViewModel.EnableMask),
				new JProperty("EnableSignalName", wFLinkButton1ViewModel.EnableSignalName),
				new JProperty("VisibilityMask", wFLinkButton1ViewModel.VisibilityMask),
				new JProperty("VisibilitySignalName", wFLinkButton1ViewModel.VisibilitySignalName),
				new JProperty("SecurityAuthorizationName", wFLinkButton1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFLinkButton1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("PopupWidth", wFLinkButton1ViewModel.PopupWidth),
				new JProperty("PopupHeight", wFLinkButton1ViewModel.PopupHeight),
				new JProperty("PopupModality", wFLinkButton1ViewModel.PopupModality),
				new JProperty("SymbolicText", wFLinkButton1ViewModel.SymbolicText),
				new JProperty("Url", wFLinkButton1ViewModel.Url),
				new JProperty("FontFamily", wFLinkButton1ViewModel.FontFamily),
				new JProperty("FontSize", wFLinkButton1ViewModel.FontSize),
				new JProperty("FontStretch", wFLinkButton1ViewModel.FontStretch),
				new JProperty("FontStyle", wFLinkButton1ViewModel.FontStyle),
				new JProperty("FontWeight", wFLinkButton1ViewModel.FontWeight)
			});
		}
	}
}
