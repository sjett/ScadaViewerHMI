// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Reporting.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Reporting.Serializers
{
	public class HMISqlReportViewerSerializer : CommonControlSerializer
	{
		public HMISqlReportViewerSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMISqlReportViewerViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMISqlReportViewerViewModel node = target as HMISqlReportViewerViewModel;
			base.DeserializeProperties(properties, node, version);

			if (((string?)properties["Background"]) != null)
				node.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)properties["Background"]!)).GetAsFrozen();

			node.UseLocalColors = ((bool?)properties["UseLocalColors"]) ?? node.UseLocalColors;

            node.ReportDefinitionsUrl = ((string?)properties["ReportDefinitionsUrl"]) ?? node.ReportDefinitionsUrl;

            node.ShowReportPreview = ((bool?)properties["ShowReportPreview"]) ?? node.ShowReportPreview;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMISqlReportViewerViewModel wFSqlReportViewerViewModel = source as HMISqlReportViewerViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFSqlReportViewerViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("Background", wFSqlReportViewerViewModel.Background?.ToString()),
				new JProperty("UseLocalColors", wFSqlReportViewerViewModel.UseLocalColors),
				new JProperty("ReportDefinitionsUrl", wFSqlReportViewerViewModel.ReportDefinitionsUrl),
				new JProperty("ShowReportPreview", wFSqlReportViewerViewModel.ShowReportPreview)
			});
		}
	}
}
