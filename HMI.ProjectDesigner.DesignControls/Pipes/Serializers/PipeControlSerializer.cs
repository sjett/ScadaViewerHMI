// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Pipes.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Pipes.Serializers
{
	public class PipeControlSerializer : CommonControlSerializer
	{
		public PipeControlSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new PipeControlViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			PipeControlViewModel node = target as PipeControlViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ObjectID = ((string?)properties["ObjectID"]) ?? node.ObjectID;

            node.SignalPrefix = properties["SignalPrefix"]?.ToObject<SignalPrefix?>() ?? node.SignalPrefix;

            node.SignalName = ((string?)properties["SignalName"]) ?? node.SignalName;

            node.SecurityAuthorizationName = ((string?)properties["SecurityAuthorizationName"]) ?? node.SecurityAuthorizationName;

            node.LineThickness = ((int?)properties["LineThickness"]) ?? node.LineThickness;

            node.Mask = ((int?)properties["Mask"]) ?? node.Mask;

            node.SecurityDenyAccessBehaviour = properties["SecurityDenyAccessBehaviour"]?.ToObject<SecurityDenyAccessBehavior?>() ?? node.SecurityDenyAccessBehaviour;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			PipeControlViewModel pipeControlViewModel = source as PipeControlViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(pipeControlViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ObjectID", pipeControlViewModel.ObjectID),
				new JProperty("SignalPrefix", pipeControlViewModel.SignalPrefix),
				new JProperty("SignalName", pipeControlViewModel.SignalName),
				new JProperty("LineThickness", pipeControlViewModel.LineThickness),
				new JProperty("Mask", pipeControlViewModel.Mask),
				new JProperty("SecurityAuthorizationName", pipeControlViewModel.SecurityAuthorizationName),
				new JProperty("SecurityDenyAccessBehaviour", pipeControlViewModel.SecurityDenyAccessBehaviour)
			});
		}
	}
}
