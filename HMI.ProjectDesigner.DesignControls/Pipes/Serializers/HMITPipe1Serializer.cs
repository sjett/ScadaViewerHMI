// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.ProjectDesigner.DesignControls.Pipes.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Pipes.Serializers
{
	public class HMITPipe1Serializer : PipeControlSerializer
	{
		public HMITPipe1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMITPipe1ViewModel(base.Container, parentNode, controlType);
		}
	}
}
