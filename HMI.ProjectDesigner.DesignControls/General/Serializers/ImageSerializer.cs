// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using HMI.ProjectDesigner.DesignControls.General.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class ImageSerializer : CommonControlSerializer
	{
		public ImageSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new ImageViewModel(base.Container, parentNode, controlType, base.ProjectManagerService);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
			ImageViewModel node = target as ImageViewModel;
			base.DeserializeProperties(properties, node, version);

			node.Source = ((string?)properties["Source"]) ?? node.Source;

            node.Stretch = properties["Stretch"]?.ToObject<Stretch?>() ?? node.Stretch;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			ImageViewModel imageViewModel = source as ImageViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(imageViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Source", imageViewModel.Source),
				new JProperty("Stretch", imageViewModel.Stretch)
			});
		}
	}
}
