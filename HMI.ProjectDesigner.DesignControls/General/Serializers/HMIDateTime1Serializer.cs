// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.General.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;
namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class HMIDateTime1Serializer : CommonControlSerializer
	{
		public HMIDateTime1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIDateTime1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIDateTime1ViewModel item = target as HMIDateTime1ViewModel;
			base.DeserializeProperties(properties, item, version);

			if ((string?)properties["Background"] != null)
				item.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if ((string?)properties["Foreground"] != null)
                item.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

			item.FontFamily = ((string?)properties["FontFamily"]) ?? item.FontFamily;

            item.FontSize = ((int?)properties["FontSize"]) ?? item.FontSize;

            item.FontStretch = ((string?)properties["FontStretch"]) ?? item.FontStretch;

            item.FontStyle = ((string?)properties["FontStyle"]) ?? item.FontStyle;

            item.FontWeight = ((string?)properties["FontWeight"]) ?? item.FontWeight;

            item.Format = ((string?)properties["Format"]) ?? item.Format;

            item.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? item.SecurityAuthorizationName;

            item.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? item.SecurityDenyAccessBehaviour;

            item.ServerName = ((string?)properties["ServerName"]) ?? item.ServerName;

            item.Source = properties["Source"]?.ToObject<HMIDateTime1ViewModel.DateTimeSource?>() ?? item.Source;

            item.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? item.UseLocalColors;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIDateTime1ViewModel wFDateTime1ViewModel = source as HMIDateTime1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFDateTime1ViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Background", wFDateTime1ViewModel.Background?.ToString()),
				new JProperty("Foreground", wFDateTime1ViewModel.Foreground?.ToString()),
				new JProperty("FontFamily", wFDateTime1ViewModel.FontFamily),
				new JProperty("FontSize", wFDateTime1ViewModel.FontSize),
				new JProperty("FontStretch", wFDateTime1ViewModel.FontStretch),
				new JProperty("FontStyle", wFDateTime1ViewModel.FontStyle),
				new JProperty("FontWeight", wFDateTime1ViewModel.FontWeight),
				new JProperty("Format", wFDateTime1ViewModel.Format),
				new JProperty("SecurityAuthorizationName", wFDateTime1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFDateTime1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("ServerName", wFDateTime1ViewModel.ServerName),
				new JProperty("Source", wFDateTime1ViewModel.Source),
				new JProperty("UseLocalColors", wFDateTime1ViewModel.UseLocalColors)
			});
		}
	}
}
