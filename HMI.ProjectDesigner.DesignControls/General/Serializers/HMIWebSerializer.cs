// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.General.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class HMIWebSerializer : CommonControlSerializer
	{
		public HMIWebSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIWebViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject properties, ICommonControlViewModel target, System.Version version)
		{
            HMIWebViewModel node = target as HMIWebViewModel;
			base.DeserializeProperties(properties, node, version);

			node.Source = ((string?)properties["Source"]) ?? node.Source;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
            HMIWebViewModel webViewModel = source as HMIWebViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(webViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Source", webViewModel.Source),
			});
		}
	}
}
