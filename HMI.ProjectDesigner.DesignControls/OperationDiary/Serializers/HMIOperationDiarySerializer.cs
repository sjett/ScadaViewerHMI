// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.OperationDiary.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.OperationDiary.Serializers
{
	public class HMIOperationDiarySerializer : CommonControlSerializer
	{
		public HMIOperationDiarySerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIOperationDiaryViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, Version version)
		{
			HMIOperationDiaryViewModel node = target as HMIOperationDiaryViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ClientsideConfiguration = ((bool?)properties["ClientsideConfiguration"]) ?? node.ClientsideConfiguration;

            node.GroupByDate = ((bool?)properties["GroupByDate"]) ?? node.GroupByDate;

            node.ConfigurationNamespace = ((string?)properties["ConfigurationNamespace"]) ?? node.ConfigurationNamespace;

            node.InitialConfiguration = ((string?)properties["InitialConfiguration"]) ?? node.InitialConfiguration;

            node.MaxEvents = ((int?)properties["MaxEvents"]) ?? node.MaxEvents;

            node.RefreshInterval = ((int?)properties["RefreshInterval"]) ?? node.RefreshInterval;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel node, Version version)
		{
			HMIOperationDiaryViewModel wFOperationDiaryViewModel = node as HMIOperationDiaryViewModel;
			base.DeserializeCollections(propertyCollectionsElement, wFOperationDiaryViewModel, version);
			this.DeserializeAlarmTypeDisplay(propertyCollectionsElement, node, wFOperationDiaryViewModel.AlarmTypesInformation, version);
			this.DeserializeColumnSettings(propertyCollectionsElement, node, wFOperationDiaryViewModel.Columns, version);
		}
		protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIOperationDiaryViewModel wFOperationDiaryViewModel = source as HMIOperationDiaryViewModel;
			IEnumerable<JProperty> first = base.SerializeProperties(wFOperationDiaryViewModel);

			return first.Concat(new List<JProperty>
			{
				new JProperty("ClientsideConfiguration", wFOperationDiaryViewModel.ClientsideConfiguration),
				new JProperty("GroupByDate", wFOperationDiaryViewModel.GroupByDate),
				new JProperty("ConfigurationNamespace", wFOperationDiaryViewModel.ConfigurationNamespace),
				new JProperty("InitialConfiguration", wFOperationDiaryViewModel.InitialConfiguration),
				new JProperty("MaxEvents", wFOperationDiaryViewModel.MaxEvents),
				new JProperty("RefreshInterval", wFOperationDiaryViewModel.RefreshInterval)
			});
		}
		protected override IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			HMIOperationDiaryViewModel wFOperationDiaryViewModel = source as HMIOperationDiaryViewModel;
			IEnumerable<JProperty> first = base.SerializeCollections(wFOperationDiaryViewModel);
			return first.Concat(new List<JProperty>
			{
				this.GetSerializedColumnSettings(wFOperationDiaryViewModel.Columns),
				this.GetSerializedAlarmTypeDisplays(wFOperationDiaryViewModel.AlarmTypesInformation)
			});
		}
	}
}
