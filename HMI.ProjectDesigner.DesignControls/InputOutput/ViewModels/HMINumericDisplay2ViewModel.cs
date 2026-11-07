// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using System.Windows;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMINumericDisplay2ViewModel : HMIStaticStatesControlBaseViewModel
	{
		private int digits = 6;
		private int digitsMaxValue = 16;
		private int digitsMinValue = 4;
		private HMINumericDisplay2DisplayType displayType = HMINumericDisplay2DisplayType.HexagonalSevenSegments;
		private int precision = 2;
		private int maximumPrecisionValue = 4;
		private string signalName;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		public event System.EventHandler<ValueChangedEventArgs<int>> DigitsChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> DigitsMaxValueChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> DigitsMinValueChanged;
		public event System.EventHandler<ValueChangedEventArgs<HMINumericDisplay2DisplayType>> DisplayTypeChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> PrecisionChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> MaximumPrecisionValueChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(300.0, 50.0);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MaximumValue("DigitsMaxValue"), MinimumValue("DigitsMinValue")]
		public int Digits
		{
			get
			{
				return this.digits;
			}
			set
			{
				if (this.digits != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.digits, value);
					base.RaisePropertyChanging<int>(() => this.Digits);
					this.digits = value;
					this.DigitsChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.Digits);
					this.UpdatePrecisionMaximumValue();
				}
			}
		}
		[Browsable(false)]
		public int DigitsMaxValue
		{
			get
			{
				return this.digitsMaxValue;
			}
			set
			{
				if (this.digitsMaxValue != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.digitsMaxValue, value);
					base.RaisePropertyChanging<int>(() => this.DigitsMaxValue);
					this.digitsMaxValue = value;
					this.DigitsMaxValueChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.DigitsMaxValue);
				}
			}
		}
		[Browsable(false)]
		public int DigitsMinValue
		{
			get
			{
				return this.digitsMinValue;
			}
			set
			{
				if (this.digitsMinValue != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.digitsMinValue, value);
					base.RaisePropertyChanging<int>(() => this.DigitsMinValue);
					this.digitsMinValue = value;
					this.DigitsMinValueChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.DigitsMinValue);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public HMINumericDisplay2DisplayType DisplayType
		{
			get
			{
				return this.displayType;
			}
			set
			{
				if (this.displayType != value)
				{
					ValueChangedEventArgs<HMINumericDisplay2DisplayType> e = new ValueChangedEventArgs<HMINumericDisplay2DisplayType>(this.displayType, value);
					base.RaisePropertyChanging<HMINumericDisplay2DisplayType>(() => this.DisplayType);
					this.displayType = value;
					this.DisplayTypeChanged.Raise(this, e);
					base.RaisePropertyChanged<HMINumericDisplay2DisplayType>(() => this.DisplayType);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MaximumValue("MaximumPrecisionValue"), MinimumValue(0.0)]
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
					this.UpdateDigitsMinimumValue();
					this.UpdateDigitsMaximumValue();
				}
			}
		}
		[Browsable(false)]
		public int MaximumPrecisionValue
		{
			get
			{
				return this.maximumPrecisionValue;
			}
			set
			{
				if (this.maximumPrecisionValue != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.maximumPrecisionValue, value);
					base.RaisePropertyChanging<int>(() => this.MaximumPrecisionValue);
					this.maximumPrecisionValue = value;
					this.MaximumPrecisionValueChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.MaximumPrecisionValue);
				}
			}
		}
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
		[Browsable(true), FontFamilyEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontFamily
		{
			get
			{
				return this.fontFamily;
			}
			set
			{
				if (this.fontFamily != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.fontFamily, value);
					base.RaisePropertyChanging<string>(() => this.FontFamily);
					this.fontFamily = value;
					this.FontFamilyChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.FontFamily);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MinimumValue(4.0)]
		public double FontSize
		{
			get
			{
				return this.fontSize;
			}
			set
			{
				if (this.fontSize != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.fontSize, value);
					base.RaisePropertyChanging<double>(() => this.FontSize);
					this.fontSize = value;
					this.FontSizeChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.FontSize);
				}
			}
		}
		[Browsable(true), FontStretchEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontStretch
		{
			get
			{
				return this.fontStretch;
			}
			set
			{
				if (this.fontStretch != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.fontStretch, value);
					base.RaisePropertyChanging<string>(() => this.FontStretch);
					this.fontStretch = value;
					this.FontStretchChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.FontStretch);
				}
			}
		}
		[Browsable(true), FontStyleEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontStyle
		{
			get
			{
				return this.fontStyle;
			}
			set
			{
				if (this.fontStyle != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.fontStyle, value);
					base.RaisePropertyChanging<string>(() => this.FontStyle);
					this.fontStyle = value;
					this.FontStyleChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.FontStyle);
				}
			}
		}
		[Browsable(true), FontWeightEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontWeight
		{
			get
			{
				return this.fontWeight;
			}
			set
			{
				if (this.fontWeight != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.fontWeight, value);
					base.RaisePropertyChanging<string>(() => this.FontWeight);
					this.fontWeight = value;
					this.FontWeightChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.FontWeight);
				}
			}
		}
		public HMINumericDisplay2ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMINumericDisplay2";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Digits", this.Digits);
			base.AddXamlAttribute("DisplayType", this.DisplayType);
			base.AddXamlAttribute("Precision", this.Precision);
			base.AddXamlAttribute("SignalName", this.SignalName);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
		}
		private void UpdateDigitsMaximumValue()
		{
			this.DigitsMaxValue = 1.7976931348623157E+308.ToString("F0").Length;
		}
		private void UpdateDigitsMinimumValue()
		{
			int val = (this.Precision < 1) ? 1 : (this.Precision + 2);
			this.DigitsMinValue = System.Math.Min(this.DigitsMinValue, val);
			this.Digits = System.Math.Max(this.Digits, this.DigitsMinValue);
		}
		private void UpdatePrecisionMaximumValue()
		{
			int val = this.Digits - 2;
			this.MaximumPrecisionValue = System.Math.Max(this.MaximumPrecisionValue, val);
			this.Precision = System.Math.Min(this.Precision, this.MaximumPrecisionValue);
		}
	}
}
