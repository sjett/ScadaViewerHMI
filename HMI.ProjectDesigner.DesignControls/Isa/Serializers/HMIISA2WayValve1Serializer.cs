// SPDX-License-Identifier: GPL-3.0-only

using System;

using HMI.ProjectDesigner.DesignControls.Isa.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Isa.Serializers
{
	
	public class HMIISA2WayValve1Serializer : HMIISABaseSerializer
	{
		public HMIISA2WayValve1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIISA2WayValve1ViewModel(base.Container, parentNode, controlType);
		}
	}
}
