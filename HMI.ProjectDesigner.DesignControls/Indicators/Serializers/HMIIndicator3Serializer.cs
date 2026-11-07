// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.ProjectDesigner.DesignControls.Indicators.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Indicators.Serializers
{
	public class HMIIndicator3Serializer : HMIIndicatorBaseSerializer
	{
		public HMIIndicator3Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIIndicator3ViewModel(base.Container, parentNode, controlType);
		}
	}
}
