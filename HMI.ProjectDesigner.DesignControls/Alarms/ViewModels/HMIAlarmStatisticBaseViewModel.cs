// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Alarms.ViewModels
{
	public class HMIAlarmStatisticBaseViewModel : CommonControlViewModel
	{
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private StatisticsTimespanRange defaultRange;
		private TelerikTheme chartTheme;
		private DialogModality popupModality;
		private System.DateTime? startDate;
		private System.DateTime? endDate;
		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event System.EventHandler<ValueChangedEventArgs<StatisticsTimespanRange>> DefaultRangeChanged;
		public event System.EventHandler<ValueChangedEventArgs<TelerikTheme>> ChartThemeChanged;
		public event System.EventHandler<ValueChangedEventArgs<DialogModality>> PopupModalityChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.DateTime?>> StartDateChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.DateTime?>> EndDateChanged;
		[Browsable(true), ProjectAuthorizationsBrowser, PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public string SecurityAuthorizationName
		{
			get
			{
				return this.securityAuthorizationName;
			}
			set
			{
				if (this.securityAuthorizationName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.securityAuthorizationName, value);
					base.RaisePropertyChanging<string>(() => this.SecurityAuthorizationName);
					this.securityAuthorizationName = value;
					this.SecurityAuthorizationNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SecurityAuthorizationName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public SecurityDenyAccessBehavior SecurityDenyAccessBehaviour
		{
			get
			{
				return this.securityDenyAccessBehavior;
			}
			set
			{
				if (this.securityDenyAccessBehavior != value)
				{
					ValueChangedEventArgs<SecurityDenyAccessBehavior> e = new ValueChangedEventArgs<SecurityDenyAccessBehavior>(this.securityDenyAccessBehavior, value);
					base.RaisePropertyChanging<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehaviour);
					this.securityDenyAccessBehavior = value;
					this.SecurityDenyAccessBehaviorChanged.Raise(this, e);
					base.RaisePropertyChanged<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehaviour);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public StatisticsTimespanRange DefaultRange
		{
			get
			{
				return this.defaultRange;
			}
			set
			{
				if (this.defaultRange != value)
				{
					ValueChangedEventArgs<StatisticsTimespanRange> e = new ValueChangedEventArgs<StatisticsTimespanRange>(this.defaultRange, value);
					base.RaisePropertyChanging<StatisticsTimespanRange>(() => this.DefaultRange);
					this.defaultRange = value;
					this.DefaultRangeChanged.Raise(this, e);
					base.RaisePropertyChanged<StatisticsTimespanRange>(() => this.DefaultRange);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public TelerikTheme ChartTheme
		{
			get
			{
				return this.chartTheme;
			}
			set
			{
				if (this.chartTheme != value)
				{
					ValueChangedEventArgs<TelerikTheme> e = new ValueChangedEventArgs<TelerikTheme>(this.chartTheme, value);
					base.RaisePropertyChanging<TelerikTheme>(() => this.ChartTheme);
					this.chartTheme = value;
					this.ChartThemeChanged.Raise(this, e);
					base.RaisePropertyChanged<TelerikTheme>(() => this.ChartTheme);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public DialogModality PopupModality
		{
			get
			{
				return this.popupModality;
			}
			set
			{
				if (this.popupModality != value)
				{
					ValueChangedEventArgs<DialogModality> e = new ValueChangedEventArgs<DialogModality>(this.popupModality, value);
					base.RaisePropertyChanging<DialogModality>(() => this.PopupModality);
					this.popupModality = value;
					this.PopupModalityChanged.Raise(this, e);
					base.RaisePropertyChanged<DialogModality>(() => this.PopupModality);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue("EndDate")]
		public System.DateTime? StartDate
		{
			get
			{
				return this.startDate;
			}
			set
			{
				if (this.startDate != value)
				{
					ValueChangedEventArgs<System.DateTime?> e = new ValueChangedEventArgs<System.DateTime?>(this.startDate, value);
					base.RaisePropertyChanging<System.DateTime?>(() => this.StartDate);
					this.startDate = value;
					this.StartDateChanged.Raise(this, e);
					base.RaisePropertyChanged<System.DateTime?>(() => this.StartDate);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue("StartDate")]
		public System.DateTime? EndDate
		{
			get
			{
				return this.endDate;
			}
			set
			{
				if (this.endDate != value)
				{
					ValueChangedEventArgs<System.DateTime?> e = new ValueChangedEventArgs<System.DateTime?>(this.endDate, value);
					base.RaisePropertyChanging<System.DateTime?>(() => this.EndDate);
					this.endDate = value;
					this.EndDateChanged.Raise(this, e);
					base.RaisePropertyChanged<System.DateTime?>(() => this.EndDate);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(500.0, 300.0);
			}
		}
		public HMIAlarmStatisticBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIAlarms=\"clr-namespace:HMI.RuntimeControls.Alarms;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIAlarms;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("DefaultRange", this.DefaultRange);
			base.AddXamlAttribute("ChartTheme", this.ChartTheme);
			base.AddXamlAttribute("PopupModality", this.PopupModality);
			base.AddXamlAttribute("StartDate", this.StartDate.HasValue ? this.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
			base.AddXamlAttribute("EndDate", this.EndDate.HasValue ? this.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
