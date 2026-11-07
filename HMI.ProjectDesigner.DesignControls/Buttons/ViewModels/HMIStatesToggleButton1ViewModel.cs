// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Buttons.ViewModels
{
	public class HMIStatesToggleButton1ViewModel : HMIStatesButtonBaseViewModel
	{
		private System.Windows.Media.Brush checkedBackground;
		private string iconImageChecked;
		private string iconImageUnchecked;
		private bool isTipModeEnabled;
		private string stateSignalName;
		private string checkedSymbolicText;
		private string uncheckedSymbolicText;
		private double stateValue1;
		private double stateValue2;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> CheckedBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> IconImageCheckedChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> IconImageUncheckedChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsTipModeEnabledChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> CheckedSymbolicTextChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> UncheckedSymbolicTextChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> Value1Changed;
		public event System.EventHandler<ValueChangedEventArgs<double>> Value2Changed;
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush CheckedBackground
		{
			get
			{
				return this.checkedBackground;
			}
			set
			{
				if (this.checkedBackground != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.CheckedBackground);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.checkedBackground, value);
					this.checkedBackground = value;
					this.CheckedBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.CheckedBackground);
				}
			}
		}
		[Browsable(true), MediaBrowser(MediaType.Image), PropertyCategory("Icons", typeof(StandardPropertyCategories))]
		public string IconImageChecked
		{
			get
			{
				return this.iconImageChecked;
			}
			set
			{
				if (this.iconImageChecked != value)
				{
					base.RaisePropertyChanging<string>(() => this.IconImageChecked);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.iconImageChecked, value);
					this.iconImageChecked = value;
					this.IconImageCheckedChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.IconImageChecked);
				}
			}
		}
		[Browsable(true), MediaBrowser(MediaType.Image), PropertyCategory("Icons", typeof(StandardPropertyCategories))]
		public string IconImageUnchecked
		{
			get
			{
				return this.iconImageUnchecked;
			}
			set
			{
				if (this.iconImageUnchecked != value)
				{
					base.RaisePropertyChanging<string>(() => this.IconImageUnchecked);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.iconImageUnchecked, value);
					this.iconImageUnchecked = value;
					this.IconImageUncheckedChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.IconImageUnchecked);
				}
			}
		}
		[PropertyCategory("General", typeof(StandardPropertyCategories))]
		public bool IsTipModeEnabled
		{
			get
			{
				return this.isTipModeEnabled;
			}
			set
			{
				if (this.isTipModeEnabled != value)
				{
					base.RaisePropertyChanging<bool>(() => this.IsTipModeEnabled);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isTipModeEnabled, value);
					this.isTipModeEnabled = value;
					this.IsTipModeEnabledChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsTipModeEnabled);
				}
			}
		}
		[Browsable(true), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName
		{
			get
			{
				return this.stateSignalName;
			}
			set
			{
				if (this.stateSignalName != value)
				{
					base.RaisePropertyChanging<string>(() => this.StateSignalName);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName, value);
					this.stateSignalName = value;
					this.StateSignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string CheckedSymbolicText
		{
			get
			{
				return this.checkedSymbolicText;
			}
			set
			{
				if (this.checkedSymbolicText != value)
				{
					base.RaisePropertyChanging<string>(() => this.CheckedSymbolicText);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.checkedSymbolicText, value);
					this.checkedSymbolicText = value;
					this.CheckedSymbolicTextChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.CheckedSymbolicText);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string UncheckedSymbolicText
		{
			get
			{
				return this.uncheckedSymbolicText;
			}
			set
			{
				if (this.uncheckedSymbolicText != value)
				{
					base.RaisePropertyChanging<string>(() => this.UncheckedSymbolicText);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.uncheckedSymbolicText, value);
					this.uncheckedSymbolicText = value;
					this.UncheckedSymbolicTextChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.UncheckedSymbolicText);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public double Value1
		{
			get
			{
				return this.stateValue1;
			}
			set
			{
				if (!object.Equals(this.stateValue1, value))
				{
					base.RaisePropertyChanging<double>(() => this.Value1);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.stateValue1, value);
					this.stateValue1 = value;
					this.Value1Changed.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Value1);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public double Value2
		{
			get
			{
				return this.stateValue2;
			}
			set
			{
				if (!object.Equals(this.stateValue2, value))
				{
					base.RaisePropertyChanging<double>(() => this.Value2);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.stateValue2, value);
					this.stateValue2 = value;
					this.Value2Changed.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Value2);
				}
			}
		}
		public HMIStatesToggleButton1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIStatesToggleButton1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("CheckedBackground", this.CheckedBackground);
			base.AddXamlAttribute("IconImageChecked", ProjectFilesResolver.Resolve(this.IconImageChecked, MediaType.Image));
			base.AddXamlAttribute("IconImageUnchecked", ProjectFilesResolver.Resolve(this.IconImageUnchecked, MediaType.Image));
			base.AddXamlAttribute("IsTipModeEnabled", this.IsTipModeEnabled);
			base.AddXamlAttribute("StateSignalName", this.StateSignalName);
			base.AddXamlAttribute("CheckedSymbolicText", this.CheckedSymbolicText);
			base.AddXamlAttribute("UncheckedSymbolicText", this.UncheckedSymbolicText);
			base.AddXamlAttribute("Value1", this.Value1);
			base.AddXamlAttribute("Value2", this.Value2);
		}
	}
}
