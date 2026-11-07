// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Primitives.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Primitives.Serializers
{
	public class HMITrapeze1Serializer : HMIPrimitiveSerializer
	{
		public HMITrapeze1Serializer(ISerializerFactory serializerFactory, IControlFactory controlFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, controlFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMITrapeze1ViewModel(base.Container, parentNode, controlType);
		}
	}
}
