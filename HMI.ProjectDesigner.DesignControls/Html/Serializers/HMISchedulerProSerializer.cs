// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.ProjectDesigner.DesignControls.Html.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.ProjectDesigner.DesignControls.Html.Serializers
{
	public class HMISchedulerProSerializer : HtmlBaseSerializer
	{
		public HMISchedulerProSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMISchedulerProViewModel(Container, parentNode, controlType);
		}
	}
}
