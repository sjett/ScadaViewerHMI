// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Media;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.ViewModels
{
	public class AlarmTypeDisplay : CollectionItem
	{
		private string symbolicName;
		private string alarmOnSymbolicName;
		private string alarmOffSymbolicName;
		private string alarmAcknowledgedSymbolicName;
		private string alarmActivatedSymbolicName;
		private string alarmDeactivatedSymbolicName;
		private System.Windows.Media.Brush color;
		private readonly IContentReplacementService contentReplacementService;
		public event System.EventHandler<ValueChangedEventArgs<string>> SymbolicNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmOnSymbolicNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmOffSymbolicNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmAcknowledgedSymbolicNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmActivatedSymbolicNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> AlarmDeactivatedSymbolicNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ColorChanged;
		[Browsable(true), AlarmTypeBrowser]
		public string SymbolicName
		{
			get
			{
				return this.symbolicName;
			}
			set
			{
				if (this.symbolicName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.symbolicName, value);
					base.RaisePropertyChanging<string>(() => this.SymbolicName);
					this.symbolicName = value;
					this.SymbolicNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SymbolicName);
				}
			}
		}
		[Browsable(true), AlarmTypeBrowser]
		public string AlarmOnSymbolicName
		{
			get
			{
				return this.alarmOnSymbolicName;
			}
			set
			{
				if (this.alarmOnSymbolicName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmOnSymbolicName, value);
					base.RaisePropertyChanging<string>(() => this.AlarmOnSymbolicName);
					this.alarmOnSymbolicName = value;
					this.AlarmOnSymbolicNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmOnSymbolicName);
				}
			}
		}
		[Browsable(true), AlarmTypeBrowser]
		public string AlarmOffSymbolicName
		{
			get
			{
				return this.alarmOffSymbolicName;
			}
			set
			{
				if (this.alarmOffSymbolicName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmOffSymbolicName, value);
					base.RaisePropertyChanging<string>(() => this.AlarmOffSymbolicName);
					this.alarmOffSymbolicName = value;
					this.AlarmOffSymbolicNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmOffSymbolicName);
				}
			}
		}
		[Browsable(true), AlarmTypeBrowser]
		public string AlarmAcknowledgedSymbolicName
		{
			get
			{
				return this.alarmAcknowledgedSymbolicName;
			}
			set
			{
				if (this.alarmAcknowledgedSymbolicName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmAcknowledgedSymbolicName, value);
					base.RaisePropertyChanging<string>(() => this.AlarmAcknowledgedSymbolicName);
					this.alarmAcknowledgedSymbolicName = value;
					this.AlarmAcknowledgedSymbolicNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmAcknowledgedSymbolicName);
				}
			}
		}
		[Browsable(true), AlarmTypeBrowser]
		public string AlarmActivatedSymbolicName
		{
			get
			{
				return this.alarmActivatedSymbolicName;
			}
			set
			{
				if (this.alarmActivatedSymbolicName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmActivatedSymbolicName, value);
					base.RaisePropertyChanging<string>(() => this.AlarmActivatedSymbolicName);
					this.alarmActivatedSymbolicName = value;
					this.AlarmActivatedSymbolicNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmActivatedSymbolicName);
				}
			}
		}
		[Browsable(true), AlarmTypeBrowser]
		public string AlarmDeactivatedSymbolicName
		{
			get
			{
				return this.alarmDeactivatedSymbolicName;
			}
			set
			{
				if (this.alarmDeactivatedSymbolicName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.alarmDeactivatedSymbolicName, value);
					base.RaisePropertyChanging<string>(() => this.AlarmDeactivatedSymbolicName);
					this.alarmDeactivatedSymbolicName = value;
					this.AlarmDeactivatedSymbolicNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.AlarmDeactivatedSymbolicName);
				}
			}
		}
		[Browsable(true)]
		public System.Windows.Media.Brush Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (this.color != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.color, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.Color);
					this.color = value;
					this.ColorChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.Color);
				}
			}
		}
		public AlarmTypeDisplay(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
			this.contentReplacementService = container.Resolve<IContentReplacementService>();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			AlarmTypeDisplay alarmTypeDisplay = target as AlarmTypeDisplay;
			alarmTypeDisplay.SymbolicName = this.SymbolicName;
			alarmTypeDisplay.AlarmOnSymbolicName = this.AlarmOnSymbolicName;
			alarmTypeDisplay.AlarmOffSymbolicName = this.AlarmOffSymbolicName;
			alarmTypeDisplay.AlarmAcknowledgedSymbolicName = this.AlarmAcknowledgedSymbolicName;
			alarmTypeDisplay.AlarmActivatedSymbolicName = this.AlarmActivatedSymbolicName;
			alarmTypeDisplay.AlarmDeactivatedSymbolicName = this.AlarmDeactivatedSymbolicName;
			alarmTypeDisplay.Color = this.Color;
		}
		public override ICollectionItem CreateClone()
		{
			return new AlarmTypeDisplay(base.Services, base.Parent);
		}
		public override string GetXaml()
		{
			string text = string.Format("<{0}:{1} SymbolicName=\"[SymbolicName]\" AlarmOnSymbolicName=\"[AlarmOnSymbolicName]\" AlarmOffSymbolicName=\"[AlarmOffSymbolicName]\" AlarmAcknowledgedSymbolicName=\"[AlarmAcknowledgedSymbolicName]\" AlarmActivatedSymbolicName=\"[AlarmActivatedSymbolicName]\" AlarmDeactivatedSymbolicName=\"[AlarmDeactivatedSymbolicName]\" Color=\"[Color]\"></{0}:{1}>", "HMIOperationDiary", "AlarmTypeDisplay");
			return this.contentReplacementService.ReplaceStringContent(text, this);
		}
	}
}
