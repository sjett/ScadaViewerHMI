// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Converters;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Primitives.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Primitives.Serializers
{
	public class HMIBorder1Serializer : CommonControlSerializer
	{
		public HMIBorder1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) 
			: base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIBorder1ViewModel(base.Container, parentNode, controlType);
		}
		//protected override XElement UpgradeXml(XElement root, Version version)
		//{
		//	if (version < new Version(3, 3, 11, 7))
		//	{
		//		root = base.UpgradeXml(root, version);
		//		string wFBorder1Update_v1_0__to__v3_3_11_ = UpgradeTemplates.HMIBorder1Update_v1_0__to__v3_3_11_5;
		//		root = base.TransformXmlElement(root, wFBorder1Update_v1_0__to__v3_3_11_, version);
		//	}
		//	return base.UpgradeXml(root, version);
		//}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HMIBorder1ViewModel node = target as HMIBorder1ViewModel;
			base.DeserializeProperties(properties, node, version);

            if ((string?)properties!["CornerRadius"] != null)
                node.CornerRadius = (CornerRadius)new CornerRadiusToStringConverter().ConvertBack((string)properties["CornerRadius"]!,null,null,null);

            if ((string?)properties["BorderThickness"] != null)
                node.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"]!, null, null, null);

            if ((string?)properties["TopBackgroundColor"] != null)
                node.TopBackgroundColor = (Color)ColorConverter.ConvertFromString((string)properties["TopBackgroundColor"]!);

            if ((string?)properties["BottomBackgroundColor"] != null)
                node.BottomBackgroundColor = (Color)ColorConverter.ConvertFromString((string)properties["BottomBackgroundColor"]!);

            if ((string?)properties["BorderBrush"] != null)
				node.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderBrush"]!)).GetAsFrozen();

		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIBorder1ViewModel wFBorder1ViewModel = source as HMIBorder1ViewModel;
            IEnumerable<JProperty> first = base.SerializeProperties(wFBorder1ViewModel);

			return first.Concat(new List<JProperty>
            {
				new JProperty("CornerRadius", wFBorder1ViewModel.CornerRadius.ToString()),
				new JProperty("BorderThickness", wFBorder1ViewModel.BorderThickness.ToString()),
				new JProperty("TopBackgroundColor", wFBorder1ViewModel.TopBackgroundColor.ToString()),
				new JProperty("BottomBackgroundColor", wFBorder1ViewModel.BottomBackgroundColor.ToString()),
				new JProperty("BorderBrush", wFBorder1ViewModel.BorderBrush?.ToString())
			});
		}
	}
}
