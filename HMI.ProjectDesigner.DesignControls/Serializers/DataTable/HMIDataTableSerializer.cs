// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Serialization;
using HMI.ProjectDesigner.DesignControls.ViewModels.DataTable;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Serializers.DataTable
{
	public class HMIDataTableSerializer : CommonControlSerializer
	{
		public HMIDataTableSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMIDataTableViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel node, System.Version version)
		{
			base.DeserializeProperties(properties, node, version);
			HMIDataTableViewModel viewModel = node as HMIDataTableViewModel;

			viewModel.PageSize = ((int?)properties["PageSize"]) ?? viewModel.PageSize;

            viewModel.ShowPager = ((bool?)properties["ShowPager"]) ?? viewModel.ShowPager;

            viewModel.InitialConfiguration = ((string?)properties["InitialConfiguration"]) ?? viewModel.InitialConfiguration;
			viewModel.ServerName = ((string?)properties["ServerName"]) ?? viewModel.ServerName;
			viewModel.Signals = ((string?)properties["Signals"]) ?? viewModel.Signals;
			viewModel.RangeHours = ((int?)properties["RangeHours"]) ?? viewModel.RangeHours;
			viewModel.ArchiveBit = ((int?)properties["ArchiveBit"]) ?? viewModel.ArchiveBit;

            viewModel.ClientSideConfiguration = ((bool?)properties["ClientSideConfiguration"]) ?? viewModel.ClientSideConfiguration;

		}
		protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel node, System.Version version)
		{
			base.DeserializeCollections(propertyCollectionsElement, node, version);
			HMIDataTableViewModel wFDataTableViewModel = node as HMIDataTableViewModel;
			this.DeserializeDataTableButtonSettings(propertyCollectionsElement, node, wFDataTableViewModel.ButtonSettings, version);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(source);
			System.Collections.Generic.List<JProperty> list = new System.Collections.Generic.List<JProperty>();
			HMIDataTableViewModel wFDataTableViewModel = source as HMIDataTableViewModel;
			list.Add(new JProperty("PageSize", wFDataTableViewModel.PageSize));
			list.Add(new JProperty("ShowPager", wFDataTableViewModel.ShowPager));
			list.Add(new JProperty("InitialConfiguration", wFDataTableViewModel.InitialConfiguration));
			list.Add(new JProperty("ServerName", wFDataTableViewModel.ServerName));
			list.Add(new JProperty("Signals", wFDataTableViewModel.Signals));
			list.Add(new JProperty("RangeHours", wFDataTableViewModel.RangeHours));
			list.Add(new JProperty("ArchiveBit", wFDataTableViewModel.ArchiveBit));
			list.Add(new JProperty("ClientSideConfiguration", wFDataTableViewModel.ClientSideConfiguration));
			return first.Concat(list);
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
		{
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeCollections(source);
			HMIDataTableViewModel wFDataTableViewModel = source as HMIDataTableViewModel;

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				this.GetSerializedDataTableButtonSettings(wFDataTableViewModel.ButtonSettings)
			});
		}
	}
}
