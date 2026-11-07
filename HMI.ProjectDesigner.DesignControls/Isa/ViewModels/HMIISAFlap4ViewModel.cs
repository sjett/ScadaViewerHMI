// SPDX-License-Identifier: GPL-3.0-only

using System;

using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.Isa.ViewModels
{
	
	public class HMIISAFlap4ViewModel : HMIISABaseViewModel
	{
		private string signalName;
		private double maxAngle = 180.0;
		private double maxValue = 100.0;
		private double minAngle;
		private double minValue;
		private double offsetAngle;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> MaxAngleChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> MaxValueChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> MinAngleChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> MinValueChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> OffsetAngleChanged;
		[Browsable(true), EditorOrder(-250.0), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string SignalName
		{
			get
			{
				return this.signalName;
			}
			set
			{
				if (this.signalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.signalName, value);
					base.RaisePropertyChanging<string>(() => this.SignalName);
					this.signalName = value;
					this.SignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SignalName);
				}
			}
		}
		[Browsable(false)]
		public string ResolvedSignalName
		{
			get
			{
				return base.GetFullSignalName(this.SignalName);
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double MaxAngle
		{
			get
			{
				return this.maxAngle;
			}
			set
			{
				if (this.maxAngle != value)
				{
					base.RaisePropertyChanging<double>(() => this.MaxAngle);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.maxAngle, value);
					this.maxAngle = value;
					this.MaxAngleChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.MaxAngle);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				if (this.maxValue != value)
				{
					base.RaisePropertyChanging<double>(() => this.MaxValue);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.maxValue, value);
					this.maxValue = value;
					this.MaxValueChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.MaxValue);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double MinAngle
		{
			get
			{
				return this.minAngle;
			}
			set
			{
				if (this.minAngle != value)
				{
					base.RaisePropertyChanging<double>(() => this.MinAngle);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.minAngle, value);
					this.minAngle = value;
					this.MinAngleChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.MinAngle);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				if (this.minValue != value)
				{
					base.RaisePropertyChanging<double>(() => this.MinValue);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.minValue, value);
					this.minValue = value;
					this.MinValueChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.MinValue);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public double OffsetAngle
		{
			get
			{
				return this.offsetAngle;
			}
			set
			{
				if (this.offsetAngle != value)
				{
					base.RaisePropertyChanging<double>(() => this.OffsetAngle);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.offsetAngle, value);
					this.offsetAngle = value;
					this.OffsetAngleChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.OffsetAngle);
				}
			}
		}
		public HMIISAFlap4ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIISAFlap4";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("SignalName", this.SignalName);
			base.AddXamlAttribute("MinValue", this.MinValue);
			base.AddXamlAttribute("MaxValue", this.MaxValue);
			base.AddXamlAttribute("MinAngle", this.MinAngle);
			base.AddXamlAttribute("MaxAngle", this.MaxAngle);
			base.AddXamlAttribute("OffsetAngle", this.OffsetAngle);
		}
	}
}
