// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Sliders.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Sliders.Serializers
{
	public class HMISlider1Serializer : CommonControlSerializer
	{
		public HMISlider1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMISlider1ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMISlider1ViewModel item = target as HMISlider1ViewModel;
			base.DeserializeProperties(properties, item, version);

            if (((string?)properties["Background"]) != null)
                item.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

            if (((string?)properties["Foreground"]) != null)
                item.Foreground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Foreground"]!)).GetAsFrozen();

            item.EnableMask = ((int?)properties["EnableMask"]) ?? item.EnableMask;

            item.EnableSignalName = ((string?)properties["EnableSignalName"]) ?? item.EnableSignalName;

            item.IsDirectionReversed = ((bool?)properties["IsDirectionReversed"]) ?? item.IsDirectionReversed;

            item.LargeChange = ((double?)properties["LargeChange"]) ?? item.LargeChange;

            item.Maximum = ((double?)properties["Maximum"]) ?? item.Maximum;

            item.Minimum = ((double?)properties["Minimum"]) ?? item.Minimum;

            item.ObjectID = ((string?)properties["ObjectID"]) ?? item.ObjectID;

            item.Orientation = properties["Orientation"]?.ToObject<Orientation?>() ?? item.Orientation;

            item.Precision = ((int?)properties["Precision"]) ?? item.Precision;

            item.SliderScale = ((bool?)properties["Scale"]) ?? item.SliderScale;

            item.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? item.SecurityAuthorizationName;

            item.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? item.SecurityDenyAccessBehaviour;

            item.StateSignalName = ((string?)properties["StateSignalName"]) ?? item.StateSignalName;

            item.WriteSignalName = ((string?)properties["WriteSignalName"]) ?? item.WriteSignalName;

            item.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? item.SignalPrefix;

            item.SmallChange = ((double?)properties["SmallChange"]) ?? item.SmallChange;

            item.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? item.UseLocalColors;

            item.VisibilityMask = ((int?)properties["VisibilityMask"]) ?? item.VisibilityMask;

            item.VisibilitySignalName = ((string?)properties["VisibilitySignalName"]) ?? item.VisibilitySignalName;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMISlider1ViewModel wFSlider1ViewModel = source as HMISlider1ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFSlider1ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Background", wFSlider1ViewModel.Background?.ToString()),
				new JProperty("Foreground", wFSlider1ViewModel.Foreground?.ToString()),
				new JProperty("EnableMask", wFSlider1ViewModel.EnableMask),
				new JProperty("EnableSignalName", wFSlider1ViewModel.EnableSignalName),
				new JProperty("IsDirectionReversed", wFSlider1ViewModel.IsDirectionReversed),
				new JProperty("LargeChange", wFSlider1ViewModel.LargeChange),
				new JProperty("Maximum", wFSlider1ViewModel.Maximum),
				new JProperty("Minimum", wFSlider1ViewModel.Minimum),
				new JProperty("ObjectID", wFSlider1ViewModel.ObjectID),
				new JProperty("Orientation", wFSlider1ViewModel.Orientation),
				new JProperty("Precision", wFSlider1ViewModel.Precision),
				new JProperty("Scale", wFSlider1ViewModel.SliderScale),
				new JProperty("SecurityAuthorizationName", wFSlider1ViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", wFSlider1ViewModel.SecurityDenyAccessBehaviour),
				new JProperty("StateSignalName", wFSlider1ViewModel.StateSignalName),
				new JProperty("WriteSignalName", wFSlider1ViewModel.WriteSignalName),
				new JProperty("SignalPrefix", wFSlider1ViewModel.SignalPrefix),
				new JProperty("SmallChange", wFSlider1ViewModel.SmallChange),
				new JProperty("UseLocalColors", wFSlider1ViewModel.UseLocalColors),
				new JProperty("VisibilityMask", wFSlider1ViewModel.VisibilityMask),
				new JProperty("VisibilitySignalName", wFSlider1ViewModel.VisibilitySignalName)
			});
		}
	}
}
