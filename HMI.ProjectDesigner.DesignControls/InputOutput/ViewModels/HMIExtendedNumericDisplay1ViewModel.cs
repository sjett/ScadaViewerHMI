// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using System.Windows;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMIExtendedNumericDisplay1ViewModel : HMIIOControlBaseViewModel
	{
		private System.Windows.HorizontalAlignment horizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
		private System.Windows.VerticalAlignment verticalContentAlignment = System.Windows.VerticalAlignment.Center;
		private int precision;
		private string unitSignalName;
		private int? unitSignalNameMask;
		private string unit1;
		private string unit2;
		private double unit1ScaleFactor = 1.0;
		private double unit2ScaleFactor = 1.0;

		public event System.EventHandler<ValueChangedEventArgs<System.Windows.HorizontalAlignment>> TextHorizontalAlignmentChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.VerticalAlignment>> TextVerticalAlignmentChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> PrecisionChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> UnitSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> UnitSignalNameMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> Unit1Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> Unit2Changed;
		public event System.EventHandler<ValueChangedEventArgs<double>> Unit1ScaleFactorChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> Unit2ScaleFactorChanged;

		[Browsable(true), EditorOrder(-235.0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public System.Windows.HorizontalAlignment HorizontalContentAlignment
		{
			get
			{
				return this.horizontalContentAlignment;
			}
			set
			{
				if (this.horizontalContentAlignment != value)
				{
					ValueChangedEventArgs<System.Windows.HorizontalAlignment> e = new ValueChangedEventArgs<System.Windows.HorizontalAlignment>(this.horizontalContentAlignment, value);
					base.RaisePropertyChanging<System.Windows.HorizontalAlignment>(() => this.HorizontalContentAlignment);
					this.horizontalContentAlignment = value;
					this.TextHorizontalAlignmentChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.HorizontalAlignment>(() => this.HorizontalContentAlignment);
				}
			}
		}
		[Browsable(true), EditorOrder(-234.0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public System.Windows.VerticalAlignment VerticalContentAlignment
		{
			get
			{
				return this.verticalContentAlignment;
			}
			set
			{
				if (this.verticalContentAlignment != value)
				{
					ValueChangedEventArgs<System.Windows.VerticalAlignment> e = new ValueChangedEventArgs<System.Windows.VerticalAlignment>(this.verticalContentAlignment, value);
					base.RaisePropertyChanging<System.Windows.VerticalAlignment>(() => this.VerticalContentAlignment);
					this.verticalContentAlignment = value;
					this.TextVerticalAlignmentChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.VerticalAlignment>(() => this.VerticalContentAlignment);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(30.0), MinimumValue(0.0)]
		public int Precision
		{
			get
			{
				return this.precision;
			}
			set
			{
				if (this.precision != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.precision, value);
					base.RaisePropertyChanging<int>(() => this.Precision);
					this.precision = value;
					this.PrecisionChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.Precision);
				}
			}
		}
		[Browsable(true), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string UnitSignalName
		{
			get
			{
				return this.unitSignalName;
			}
			set
			{
				if (this.unitSignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.unitSignalName, value);
					base.RaisePropertyChanging<string>(() => this.UnitSignalName);
					this.unitSignalName = value;
					this.UnitSignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.UnitSignalName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public int? UnitSignalNameMask
		{
			get
			{
				return this.unitSignalNameMask;
			}
			set
			{
				if (this.unitSignalNameMask != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.unitSignalNameMask, value);
					base.RaisePropertyChanging<int?>(() => this.UnitSignalNameMask);
					this.unitSignalNameMask = value;
					this.UnitSignalNameMaskChanged.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.UnitSignalNameMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string Unit1
		{
			get
			{
				return this.unit1;
			}
			set
			{
				if (this.unit1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.unit1, value);
					base.RaisePropertyChanging<string>(() => this.Unit1);
					this.unit1 = value;
					this.Unit1Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Unit1);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string Unit2
		{
			get
			{
				return this.unit2;
			}
			set
			{
				if (this.unit2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.unit2, value);
					base.RaisePropertyChanging<string>(() => this.Unit2);
					this.unit2 = value;
					this.Unit2Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Unit2);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public double Unit1ScaleFactor
		{
			get
			{
				return this.unit1ScaleFactor;
			}
			set
			{
				if (this.unit1ScaleFactor != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.unit1ScaleFactor, value);
					base.RaisePropertyChanging<double>(() => this.Unit1ScaleFactor);
					this.unit1ScaleFactor = value;
					this.Unit1ScaleFactorChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Unit1ScaleFactor);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public double Unit2ScaleFactor
		{
			get
			{
				return this.unit2ScaleFactor;
			}
			set
			{
				if (this.unit2ScaleFactor != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.unit2ScaleFactor, value);
					base.RaisePropertyChanging<double>(() => this.Unit2ScaleFactor);
					this.unit2ScaleFactor = value;
					this.Unit2ScaleFactorChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Unit2ScaleFactor);
				}
			}
		}
		public HMIExtendedNumericDisplay1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIExtendedNumericDisplay1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Precision", this.Precision);
			base.AddXamlAttribute("Unit1", this.Unit1);
			base.AddXamlAttribute("Unit2", this.Unit2);
			base.AddXamlAttribute("Unit1ScaleFactor", this.Unit1ScaleFactor);
			base.AddXamlAttribute("Unit2ScaleFactor", this.Unit2ScaleFactor);
			base.AddXamlAttribute("UnitSignalName", this.UnitSignalName);
			base.AddXamlAttribute("UnitSignalNameMask", this.UnitSignalNameMask);
			base.AddXamlAttribute("HorizontalContentAlignment", this.HorizontalContentAlignment);
			base.AddXamlAttribute("VerticalContentAlignment", this.VerticalContentAlignment);
			base.AddXamlAttribute("Foreground", base.Foreground);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
		}
	}
}
