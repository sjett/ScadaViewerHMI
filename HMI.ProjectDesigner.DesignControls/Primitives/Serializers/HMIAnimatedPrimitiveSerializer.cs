// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Primitives.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Primitives.Serializers
{
	public abstract class HMIAnimatedPrimitiveSerializer : HMIAnimatedStatesControlBaseSerializer
	{
		public HMIAnimatedPrimitiveSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HMIAnimatedPrimitiveBaseViewModel node = target as HMIAnimatedPrimitiveBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.SignalName = ((string?)properties!["SignalName"]) ?? node.SignalName;

			if ((string?)properties["LineThickness"] != null)
				node.LineThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["LineThickness"]!, null, null,null);


            if ((string?)properties["AnimatedFill"] != null)
				node.AnimatedFill = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(((string)properties["AnimatedFill"]!))).GetAsFrozen();

			node.RangeMin = ((double?)properties["RangeMin"]) ?? node.RangeMin;

			node.RangeMax = ((double?)properties["RangeMax"]) ?? node.RangeMax;

		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIAnimatedPrimitiveBaseViewModel wFAnimatedPrimitiveBaseViewModel = source as HMIAnimatedPrimitiveBaseViewModel;
            IEnumerable<JProperty> first = base.SerializeProperties(wFAnimatedPrimitiveBaseViewModel);

			return first.Concat(new List<JProperty>
            {
				new JProperty("SignalName", wFAnimatedPrimitiveBaseViewModel.SignalName),
				new JProperty("LineThickness", wFAnimatedPrimitiveBaseViewModel.LineThickness.ToString()),
                new JProperty("AnimatedFill", wFAnimatedPrimitiveBaseViewModel.AnimatedFill?.ToString()),
				new JProperty("RangeMin", wFAnimatedPrimitiveBaseViewModel.RangeMin),
				new JProperty("RangeMax", wFAnimatedPrimitiveBaseViewModel.RangeMax)
			});
		}
	}
}
