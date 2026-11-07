// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public abstract class HMITextBoxBaseSerializer : HMIIOControlBaseSerializer
	{
		public HMITextBoxBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMITextBoxBaseViewModel node = target as HMITextBoxBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.WriteSecure = ((bool?)properties["WriteSecure"]) ?? node.WriteSecure;

            node.EnableSignalName = ((string?)properties["EnableSignalName"]) ?? node.EnableSignalName;

            node.EnableMask = ((int?)properties["EnableMask"]) ?? node.EnableMask;

			if ((string?)properties["EditModeHintBackground"] != null)
				node.EditModeHintBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["EditModeHintBackground"]!)).GetAsFrozen();

            if ((string?)properties["Background"] != null)
                node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if ((string?)properties["Foreground"] != null)
                node.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            node.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? node.UseLocalColors;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMITextBoxBaseViewModel wFTextBoxBaseViewModel = source as HMITextBoxBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFTextBoxBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("WriteSecure", wFTextBoxBaseViewModel.WriteSecure),
				new JProperty("EnableSignalName", wFTextBoxBaseViewModel.EnableSignalName),
				new JProperty("EnableMask", wFTextBoxBaseViewModel.EnableMask),
				new JProperty("EditModeHintBackground", wFTextBoxBaseViewModel.EditModeHintBackground?.ToString()),
				new JProperty("Background", wFTextBoxBaseViewModel.Background?.ToString()),
				new JProperty("Foreground", wFTextBoxBaseViewModel.Foreground?.ToString()),
				new JProperty("UseLocalColors", wFTextBoxBaseViewModel.UseLocalColors)
			});
		}
	}
}
