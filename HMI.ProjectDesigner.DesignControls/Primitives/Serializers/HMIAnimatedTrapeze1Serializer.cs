// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Primitives.ViewModels;

namespace HMI.ProjectDesigner.DesignControls.Primitives.Serializers
{
	public class HMIAnimatedTrapeze1Serializer : HMIAnimatedPrimitiveSerializer
	{
		public HMIAnimatedTrapeze1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) 
			: base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIAnimatedTrapeze1ViewModel(base.Container, parentNode, controlType);
		}
	}
}
