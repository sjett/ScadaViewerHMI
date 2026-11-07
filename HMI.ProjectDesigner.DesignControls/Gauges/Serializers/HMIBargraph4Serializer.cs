// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using HMI.ProjectDesigner.DesignControls.Gauges.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Gauges.Serializers
{
	public class HMIBargraph4Serializer : HMIBargraphBaseSerializer
	{
		public HMIBargraph4Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIBargraph4ViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIBargraph4ViewModel node = target as HMIBargraph4ViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ThresholdValue1 = ((double?)properties["ThresholdValue1"]) ?? node.ThresholdValue1;

            node.ThresholdValue1 = ((double?)properties["ThresholdValue2"]) ?? node.ThresholdValue2;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIBargraph4ViewModel wFBargraph4ViewModel = source as HMIBargraph4ViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFBargraph4ViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ThresholdValue1", wFBargraph4ViewModel.ThresholdValue1),
				new JProperty("ThresholdValue2", wFBargraph4ViewModel.ThresholdValue2)
			});
		}
	}
}
