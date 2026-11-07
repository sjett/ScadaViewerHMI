// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Reporting.ViewModels
{
	public class HMISqlReportViewerViewModel : CommonControlViewModel
	{
		private string reportDefinitionsUrl;
		private System.DateTime startDate = System.DateTime.Now.AddDays(-1.0);
		private System.DateTime endDate = System.DateTime.Now;
		private bool showReportPreview = true;
		private bool useLocalColors;

		public event System.EventHandler<ValueChangedEventArgs<string>> ReportDefinitionsUrlChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.DateTime>> StartDateChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.DateTime>> EndDateChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ShowReportPreviewChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;

		[PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
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
					base.RaisePropertyChanging<string>(() => this.ReportDefinitionsUrl);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.reportDefinitionsUrl, value);
					this.reportDefinitionsUrl = value;
					this.ReportDefinitionsUrlChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ReportDefinitionsUrl);
				}
			}
		}
		[Browsable(false)]
		public System.DateTime StartDate
		{
			get
			{
				return this.startDate;
			}
			set
			{
				if (this.startDate != value)
				{
					base.RaisePropertyChanging<System.DateTime>(() => this.StartDate);
					ValueChangedEventArgs<System.DateTime> e = new ValueChangedEventArgs<System.DateTime>(this.startDate, value);
					this.startDate = value;
					this.StartDateChanged.Raise(this, e);
					base.RaisePropertyChanged<System.DateTime>(() => this.StartDate);
				}
			}
		}
		[Browsable(false)]
		public System.DateTime EndDate
		{
			get
			{
				return this.endDate;
			}
			set
			{
				if (this.endDate != value)
				{
					base.RaisePropertyChanging<System.DateTime>(() => this.EndDate);
					ValueChangedEventArgs<System.DateTime> e = new ValueChangedEventArgs<System.DateTime>(this.endDate, value);
					this.endDate = value;
					this.EndDateChanged.Raise(this, e);
					base.RaisePropertyChanged<System.DateTime>(() => this.EndDate);
				}
			}
		}
		[PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public bool ShowReportPreview
		{
			get
			{
				return this.showReportPreview;
			}
			set
			{
				if (this.showReportPreview != value)
				{
					base.RaisePropertyChanging<bool>(() => this.ShowReportPreview);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.showReportPreview, value);
					this.showReportPreview = value;
					this.ShowReportPreviewChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ShowReportPreview);
				}
			}
		}
		[Browsable(true), EditorOrder(-253.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool UseLocalColors
		{
			get
			{
				return this.useLocalColors;
			}
			set
			{
				if (this.useLocalColors != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.useLocalColors, value);
					base.RaisePropertyChanging<bool>(() => this.UseLocalColors);
					this.useLocalColors = value;
					this.UseLocalColorsChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.UseLocalColors);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(500.0, 350.0);
			}
		}
		public HMISqlReportViewerViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMISqlReportViewer";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMISqlReport=\"clr-namespace:HMI.RuntimeControls.Reporting;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIReporting;
		//}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = true;
			base.IsForegroundVisible = false;
			base.IsTemplateEnabled = false;
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Background", this.UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("ReportDefinitionsUrl", this.ReportDefinitionsUrl);
			base.AddXamlAttribute("ShowReportPreview", this.ShowReportPreview);
		}
		//public override System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> GetSilverlightObjectParameters()
		//{
		//	return new System.Collections.Generic.KeyValuePair<string, string>[]
		//	{
		//		new System.Collections.Generic.KeyValuePair<string, string>("windowless", "true")
		//	};
		//}
	}
}
