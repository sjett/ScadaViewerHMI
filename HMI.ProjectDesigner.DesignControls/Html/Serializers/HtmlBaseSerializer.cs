// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using HMI.Infrastructure.Converters;
using HMI.ProjectDesigner.DesignControls.Html.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Html.Serializers
{
	public class HtmlBaseSerializer : CommonControlSerializer
	{
		public HtmlBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HtmlBaseViewModel(Container, parentNode, controlType);
		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HtmlBaseViewModel htmlBaseViewModel = source as HtmlBaseViewModel;
			IEnumerable<JProperty> first = base.SerializeProperties(htmlBaseViewModel);

			return first.Concat(new List<JProperty>
			{
				new JProperty("BorderThickness", htmlBaseViewModel.BorderThickness.ToString()),
				new JProperty("BorderBrush", htmlBaseViewModel.BorderBrush?.ToString()),
				new JProperty("Background", htmlBaseViewModel.Background?.ToString())
			});
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HtmlBaseViewModel node = target as HtmlBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			if((string?)properties["BorderThickness"] != null)
				node.BorderThickness = (Thickness)new ThicknessToStringConverter().ConvertBack((string)properties["BorderThickness"], null, null, null);

			if ((string?)properties["BorderBrush"] != null)
				node.BorderBrush = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["BorderThickness"]!)).GetAsFrozen();

            if ((string?)properties["Background"] != null)
                node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

		}
		//protected override XElement UpgradeXml(XElement root, System.Version version)
		//{
		//	if (version < new System.Version(3, 3, 11, 7))
		//	{
		//		root = base.UpgradeXml(root, version);
		//		string htmlBaseUpdate_v1_0__to__v3_3_11_ = UpgradeTemplates.HtmlBaseUpdate_v1_0__to__v3_3_11_5;
		//		root = base.TransformXmlElement(root, htmlBaseUpdate_v1_0__to__v3_3_11_, version);
		//	}
		//	return base.UpgradeXml(root, version);
		//}
	}
}
