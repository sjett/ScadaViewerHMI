// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.Serializers
{
	public class HMITextBox1Serializer : HMITextBoxBaseSerializer
	{
		public HMITextBox1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMITextBox1ViewModel(base.Container, parentNode, controlType);
		}
	}
}
