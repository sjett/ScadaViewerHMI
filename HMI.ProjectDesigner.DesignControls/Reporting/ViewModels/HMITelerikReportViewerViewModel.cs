// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Reporting.ViewModels
{
	public class HMITelerikReportViewerViewModel : CommonControlViewModel
	{
		private ReportDefinition selectedReport;
		private string reportDefinitionsUrl;
		public event System.EventHandler<ValueChangedEventArgs<ReportDefinition>> SelectedReportChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ReportDefinitionsUrlChanged;
		[Browsable(false)]
		public System.Collections.ObjectModel.ObservableCollection<ReportDefinition> Reports
		{
			get;
			private set;
		}
		[Browsable(false)]
		public ReportDefinition SelectedReport
		{
			get
			{
				return this.selectedReport;
			}
			set
			{
				if (this.selectedReport != value)
				{
					ValueChangedEventArgs<ReportDefinition> e = new ValueChangedEventArgs<ReportDefinition>(this.selectedReport, value);
					this.selectedReport = value;
					this.SelectedReportChanged.Raise(this, e);
					base.RaisePropertyChanged<ReportDefinition>(() => this.SelectedReport);
				}
			}
		}
		[Browsable(false)]
		public string AvailableReportsLabel
		{
			get
			{
				return "可用报表:";
			}
		}
		[Browsable(false)]
		public string DescriptionLabel
		{
			get
			{
				return "描述:";
			}
		}
		[Browsable(false)]
		public string StartTimeLabel
		{
			get
			{
				return "开始:";
			}
		}
		[Browsable(false)]
		public string EndTimeLabel
		{
			get
			{
				return "结束:";
			}
		}
		[Browsable(false)]
		public string GenerateText
		{
			get
			{
				return "生成";
			}
		}
		[Browsable(false)]
		public System.DateTime StartDate
		{
			get
			{
				return System.DateTime.Today;
			}
		}
		[Browsable(false)]
		public System.DateTime EndDate
		{
			get
			{
				return System.DateTime.Today.AddDays(1.0);
			}
		}
		[Browsable(false), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string ReportDefinitionsUrl
		{
			get
			{
				return this.reportDefinitionsUrl;
			}
			set
			{
				if (this.reportDefinitionsUrl != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.reportDefinitionsUrl, value);
					base.RaisePropertyChanging<string>(() => this.ReportDefinitionsUrl);
					this.reportDefinitionsUrl = value;
					this.ReportDefinitionsUrlChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ReportDefinitionsUrl);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(430.0, 350.0);
			}
		}
		public HMITelerikReportViewerViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.InitializeReports();
		}
		private void InitializeReports()
		{
			this.Reports = new System.Collections.ObjectModel.ObservableCollection<ReportDefinition>();
			this.Reports.Add(new ReportDefinition
			{
				Name = "历史通道值",
				Description = "从 RapidSCADA 查询所选通道的历史值，可导出为 PDF 或 Excel。"
			});
			this.Reports.Add(new ReportDefinition
			{
				Name = "操作事件",
				Description = "从 RapidSCADA 查询指定时间范围内的事件记录，可导出为 PDF 或 Excel。"
			});
			this.Reports.Add(new ReportDefinition
			{
				Name = "报警历史",
				Description = "从 RapidSCADA 查询指定时间范围内的报警记录，可导出为 PDF 或 Excel。"
			});
			this.SelectedReport = this.Reports.First<ReportDefinition>();
		}
		public override string GetXamlControlName()
		{
			return "HMITelerikReportViewer";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIReporting=\"clr-namespace:HMI.RuntimeControls.Reporting;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIReporting;
		//}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ReportDefinitionsUrl", this.ReportDefinitionsUrl);
		}
	}
}
