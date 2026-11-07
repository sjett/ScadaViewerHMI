// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.Alarms.ViewModels
{
	public class HMIHighCountAlarmStatisticBaseViewModel : HMIAlarmStatisticBaseViewModel
	{
		private string alarmGroupName;
		private string alarmTypeName;
		private int maximumAlarms = 5;
		private DisplayType displayType;

		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmGroupNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmTypeNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> MaximumAlarmsChanged;
		public event System.EventHandler<ValueChangedEventArgs<DisplayType>> DisplayTypeChanged;

		[Browsable(true), AlarmGroupBrowser, PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string AlarmGroupName
		{
			get
			{
				return this.alarmGroupName;
			}
			set
			{
				if (this.alarmGroupName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmGroupName, value);
					base.RaisePropertyChanging<string>(() => this.AlarmGroupName);
					this.alarmGroupName = value;
					this.AlarmGroupNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmGroupName);
				}
			}
		}
		[Browsable(true), AlarmTypeBrowser, PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string AlarmTypeName
		{
			get
			{
				return this.alarmTypeName;
			}
			set
			{
				if (this.alarmTypeName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmTypeName, value);
					base.RaisePropertyChanging<string>(() => this.AlarmTypeName);
					this.alarmTypeName = value;
					this.AlarmTypeNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmTypeName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(100.0), MinimumValue(1.0)]
		public int MaximumAlarms
		{
			get
			{
				return this.maximumAlarms;
			}
			set
			{
				if (this.maximumAlarms != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.maximumAlarms, value);
					base.RaisePropertyChanging<int>(() => this.MaximumAlarms);
					this.maximumAlarms = value;
					this.MaximumAlarmsChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.MaximumAlarms);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public DisplayType DisplayType
		{
			get
			{
				return this.displayType;
			}
			set
			{
				if (this.displayType != value)
				{
					ValueChangedEventArgs<DisplayType> e = new ValueChangedEventArgs<DisplayType>(this.displayType, value);
					base.RaisePropertyChanging<DisplayType>(() => this.DisplayType);
					this.displayType = value;
					this.DisplayTypeChanged.Raise(this, e);
					base.RaisePropertyChanged<DisplayType>(() => this.DisplayType);
				}
			}
		}
		public HMIHighCountAlarmStatisticBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("MaximumAlarms", this.MaximumAlarms);
			base.AddXamlAttribute("DisplayType", this.DisplayType);
			base.AddXamlAttribute("AlarmGroupName", this.AlarmGroupName);
			base.AddXamlAttribute("AlarmTypeName", this.AlarmTypeName);
		}
	}
}
