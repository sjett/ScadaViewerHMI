// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Alarms.Serializers
{
	public class HMIAlarmDuration1Serializer : HMIHighCountAlarmStatisticBaseSerializer
	{
		public HMIAlarmDuration1Serializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIAlarmDuration1ViewModel(base.Container, parentNode, controlType);
		}
	}
}
