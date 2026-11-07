// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.Pipes.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Pipes.Serializers
{
	public class LineControlSerializer : PipeControlSerializer
	{
		public LineControlSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new LineControlViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			LineControlViewModel node = target as LineControlViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ConditionRule = ((string?)properties["ConditionRule"]) ?? node.ConditionRule;
            
		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel target, System.Version version)
		{
			LineControlViewModel lineControlViewModel = target as LineControlViewModel;
			base.DeserializeCollections(propertyCollectionsElement, lineControlViewModel, version);
			this.DeserializeConditionSignals(propertyCollectionsElement, lineControlViewModel, lineControlViewModel.ConditionSignals, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			LineControlViewModel lineControlViewModel = source as LineControlViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(lineControlViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ConditionRule", lineControlViewModel.ConditionRule)
			});
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			LineControlViewModel lineControlViewModel = source as LineControlViewModel;
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedConditionSignals(lineControlViewModel.ConditionSignals)
			});
		}
	}
}
