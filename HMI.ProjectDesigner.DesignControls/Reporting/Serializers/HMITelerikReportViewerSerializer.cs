// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Reporting.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Reporting.Serializers
{
	public class HMITelerikReportViewerSerializer : CommonControlSerializer
	{
		public HMITelerikReportViewerSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMITelerikReportViewerViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMITelerikReportViewerViewModel node = target as HMITelerikReportViewerViewModel;
			base.DeserializeProperties(properties, node, version);

			node.ReportDefinitionsUrl = ((string?)properties["ReportDefinitionsUrl"]) ?? node.ReportDefinitionsUrl;
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMITelerikReportViewerViewModel wFTelerikReportViewerViewModel = source as HMITelerikReportViewerViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFTelerikReportViewerViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ReportDefinitionsUrl", wFTelerikReportViewerViewModel.ReportDefinitionsUrl)
			});
		}
	}
}
