// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using HMI.ProjectDesigner.DesignControls.General.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.General.Serializers
{
	public class HMIConnectorStatisticsSerializer : CommonControlSerializer
	{
		public HMIConnectorStatisticsSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIConnectorStatisticsViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIConnectorStatisticsViewModel item = target as HMIConnectorStatisticsViewModel;
			base.DeserializeProperties(properties, item, version);

			item.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? item.UseLocalColors;
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIConnectorStatisticsViewModel wFConnectorStatisticsViewModel = source as HMIConnectorStatisticsViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFConnectorStatisticsViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("UseLocalColors", wFConnectorStatisticsViewModel.UseLocalColors)
			});
		}
	}
}
