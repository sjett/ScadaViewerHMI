// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public abstract class HMIStatesControlBaseSerializer : CommonControlSerializer
	{
		public HMIStatesControlBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) 
			: base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIStatesControlBaseViewModel node = target as HMIStatesControlBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ObjectID = ((string?)properties!["ObjectID"]) ?? node.ObjectID;

			node.SignalPrefix = properties!["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

			node.SecurityAuthorizationName = ((string?)properties!["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

			node.SecurityDenyAccessBehaviour = properties!["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

			node.UseLocalColors = ((bool?)properties!["UseLocalColors"]) ?? node.UseLocalColors;

			var bg = ((string?)properties!["BackgroundLocal"]);
			if (bg != null)
				node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(bg)).GetAsFrozen();

			var fg = ((string?)properties!["ForegroundLocal"]);
			if (fg != null)
				node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(fg)).GetAsFrozen();
        }
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIStatesControlBaseViewModel? wFStatesControlBaseViewModel = source as HMIStatesControlBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFStatesControlBaseViewModel!);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ObjectID", wFStatesControlBaseViewModel.ObjectID),
				new JProperty("SignalPrefix", wFStatesControlBaseViewModel.SignalPrefix),
				new JProperty("SecurityAuthorizationName", wFStatesControlBaseViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFStatesControlBaseViewModel.SecurityDenyAccessBehaviour),
				new JProperty("UseLocalColors", wFStatesControlBaseViewModel.UseLocalColors),
				new JProperty("BackgroundLocal", wFStatesControlBaseViewModel.Background?.ToString()),
				new JProperty("ForegroundLocal", wFStatesControlBaseViewModel.Foreground?.ToString())
			});
		}
	}
}
