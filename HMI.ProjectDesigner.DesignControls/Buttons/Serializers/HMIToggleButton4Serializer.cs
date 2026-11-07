// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Buttons.Serializers
{
	public class HMIToggleButton4Serializer : HMIToggleButtonBaseSerializer
	{
		public HMIToggleButton4Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIToggleButton4ViewModel(base.Container, parentNode, controlType);
		}
	}
}
