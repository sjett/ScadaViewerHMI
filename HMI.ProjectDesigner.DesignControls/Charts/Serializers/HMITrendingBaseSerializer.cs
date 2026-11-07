// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Charts.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Charts.Serializers
{
	public class HMITrendingBaseSerializer : CommonControlSerializer
	{
		public HMITrendingBaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMITrendingBaseViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMITrendingBaseViewModel node = target as HMITrendingBaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ClientSideConfiguration = ((bool?)properties["ClientsideConfiguration"]) ?? node.ClientSideConfiguration;

            node.ConfigurationNamespace = ((string?)properties["ConfigurationNamespace"]) ?? node.ConfigurationNamespace;

            node.InitialConfiguration = ((string?)properties["InitialConfiguration"]) ?? node.InitialConfiguration;
			node.ServerName = ((string?)properties["ServerName"]) ?? node.ServerName;
			node.Signals = ((string?)properties["Signals"]) ?? node.Signals;
			node.RangeHours = ((int?)properties["RangeHours"]) ?? node.RangeHours;
			node.ArchiveBit = ((int?)properties["ArchiveBit"]) ?? node.ArchiveBit;

			node.PopupModality = properties["PopupModality"]?.ToObject<DialogModality?>() ?? node.PopupModality;

            node.TimeZone = properties["TimeZone"]?.ToObject<HMITrendingBase.TrendingTimeZone?>() ?? node.TimeZone;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMITrendingBaseViewModel wFTrendingBaseViewModel = source as HMITrendingBaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFTrendingBaseViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ClientsideConfiguration", wFTrendingBaseViewModel.ClientSideConfiguration),
				new JProperty("ConfigurationNamespace", wFTrendingBaseViewModel.ConfigurationNamespace),
				new JProperty("InitialConfiguration", wFTrendingBaseViewModel.InitialConfiguration),
				new JProperty("ServerName", wFTrendingBaseViewModel.ServerName),
				new JProperty("Signals", wFTrendingBaseViewModel.Signals),
				new JProperty("RangeHours", wFTrendingBaseViewModel.RangeHours),
				new JProperty("ArchiveBit", wFTrendingBaseViewModel.ArchiveBit),
				new JProperty("PopupModality", wFTrendingBaseViewModel.PopupModality),
				new JProperty("TimeZone", wFTrendingBaseViewModel.TimeZone)
			});
		}
	}
}
