// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;

namespace HMI.ProjectDesigner.DesignControls.Sliders.ViewModels
{
	public class HMISlider1ViewModel : CommonControlViewModel
	{
		private int? enableMask;
		private string enableSignalName;
		private bool isDirectionReversed;
		private double largeChange = 1.0;
		private double maximum = 100.0;
		private double minimum;
		private string objectID;
		private System.Windows.Controls.Orientation orientation;
		private int precision;
		private bool sliderScale;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private string stateSignalName;
		private string writeSignalName;
		private SignalPrefix signalPrefix;
		private double smallChange = 0.1;
		private bool useLocalColors;
		private int? visibilityMask;
		private string visibilitySignalName;
		public event System.EventHandler<ValueChangedEventArgs<int?>> EnableMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> EnableSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsDirectionReversedChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> LargeChangeChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> MaximumChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> MinimumChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Controls.Orientation>> OrientationChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> PrecisionChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ScaleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> WriteSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> SmallChangeChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> VisibilityMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> VisibilitySignalNameChanged;
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public int? EnableMask
		{
			get
			{
				return this.enableMask;
			}
			set
			{
				if (this.enableMask != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.enableMask, value);
					base.RaisePropertyChanging<int?>(() => this.EnableMask);
					this.enableMask = value;
					this.EnableMaskChanged.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.EnableMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories)), SignalBrowser]
		public string EnableSignalName
		{
			get
			{
				return this.enableSignalName;
			}
			set
			{
				if (this.enableSignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.enableSignalName, value);
					base.RaisePropertyChanging<string>(() => this.EnableSignalName);
					this.enableSignalName = value;
					this.EnableSignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.EnableSignalName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool IsDirectionReversed
		{
			get
			{
				return this.isDirectionReversed;
			}
			set
			{
				if (this.isDirectionReversed != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isDirectionReversed, value);
					base.RaisePropertyChanging<bool>(() => this.IsDirectionReversed);
					this.isDirectionReversed = value;
					this.IsDirectionReversedChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsDirectionReversed);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue(4.94065645841247E-324)]
		public double LargeChange
		{
			get
			{
				return this.largeChange;
			}
			set
			{
				if (this.largeChange != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.largeChange, value);
					base.RaisePropertyChanging<double>(() => this.LargeChange);
					this.largeChange = value;
					this.LargeChangeChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.LargeChange);
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue("Minimum")]
		public double Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (this.maximum != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.maximum, value);
					base.RaisePropertyChanging<double>(() => this.Maximum);
					this.maximum = value;
					this.MaximumChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Maximum);
				}
			}
		}
		public double Medium
		{
			get
			{
				return (this.Minimum + this.Maximum) / 2.0;
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue("Maximum")]
		public double Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				if (this.minimum != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.minimum, value);
					base.RaisePropertyChanging<double>(() => this.Minimum);
					this.minimum = value;
					this.MinimumChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Minimum);
				}
			}
		}
		[Browsable(true), DisplayName("ObjectName"), EditorOrder(-248.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public string ObjectID
		{
			get
			{
				return this.objectID;
			}
			set
			{
				if (this.objectID != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.objectID, value);
					base.RaisePropertyChanging<string>(() => this.ObjectID);
					this.objectID = value;
					this.ObjectIDChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ObjectID);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public System.Windows.Controls.Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				if (this.orientation != value)
				{
					ValueChangedEventArgs<System.Windows.Controls.Orientation> e = new ValueChangedEventArgs<System.Windows.Controls.Orientation>(this.orientation, value);
					base.RaisePropertyChanging<System.Windows.Controls.Orientation>(() => this.Orientation);
					this.orientation = value;
					this.OrientationChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Controls.Orientation>(() => this.Orientation);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(100.0), MinimumValue(0.0)]
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
		[Browsable(true), DisplayName("Scale"), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool SliderScale
		{
			get
			{
				return this.sliderScale;
			}
			set
			{
				if (this.sliderScale != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.sliderScale, value);
					base.RaisePropertyChanging<bool>(() => this.SliderScale);
					this.sliderScale = value;
					this.ScaleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.SliderScale);
				}
			}
		}
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
		[Browsable(true), EditorOrder(-250.0), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
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
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName);
					this.stateSignalName = value;
					this.StateSignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName);
				}
			}
		}
		[Browsable(true), EditorOrder(-250.0), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string WriteSignalName
		{
			get
			{
				return this.writeSignalName;
			}
			set
			{
				if (this.writeSignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.writeSignalName, value);
					base.RaisePropertyChanging<string>(() => this.WriteSignalName);
					this.writeSignalName = value;
					this.WriteSignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.WriteSignalName);
				}
			}
		}
		[Browsable(true), EditorOrder(-249.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public SignalPrefix SignalPrefix
		{
			get
			{
				return this.signalPrefix;
			}
			set
			{
				if (this.signalPrefix != value)
				{
					ValueChangedEventArgs<SignalPrefix> e = new ValueChangedEventArgs<SignalPrefix>(this.signalPrefix, value);
					base.RaisePropertyChanging<SignalPrefix>(() => this.SignalPrefix);
					this.signalPrefix = value;
					this.SignalPrefixChanged.Raise(this, e);
					base.RaisePropertyChanged<SignalPrefix>(() => this.SignalPrefix);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue(4.94065645841247E-324)]
		public double SmallChange
		{
			get
			{
				return this.smallChange;
			}
			set
			{
				if (this.smallChange != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.smallChange, value);
					base.RaisePropertyChanging<double>(() => this.SmallChange);
					this.smallChange = value;
					this.SmallChangeChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.SmallChange);
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
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public int? VisibilityMask
		{
			get
			{
				return this.visibilityMask;
			}
			set
			{
				if (this.visibilityMask != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.visibilityMask, value);
					base.RaisePropertyChanging<int?>(() => this.VisibilityMask);
					this.visibilityMask = value;
					this.VisibilityMaskChanged.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.VisibilityMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories)), SignalBrowser]
		public string VisibilitySignalName
		{
			get
			{
				return this.visibilitySignalName;
			}
			set
			{
				if (this.visibilitySignalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.visibilitySignalName, value);
					base.RaisePropertyChanging<string>(() => this.VisibilitySignalName);
					this.visibilitySignalName = value;
					this.VisibilitySignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.VisibilitySignalName);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(100.0, 25.0);
			}
		}
		public HMISlider1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMISlider1";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMISliders=\"clr-namespace:HMI.RuntimeControls.Sliders;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMISliders;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Background", this.UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("Foreground", this.UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
			base.AddXamlAttribute("EnableMask", this.EnableMask);
			base.AddXamlAttribute("EnableSignalName", this.EnableSignalName);
			base.AddXamlAttribute("IsDirectionReversed", this.IsDirectionReversed);
			base.AddXamlAttribute("LargeChange", this.LargeChange);
			base.AddXamlAttribute("Maximum", this.Maximum);
			base.AddXamlAttribute("Minimum", this.Minimum);
			base.AddXamlAttribute("ObjectID", this.ObjectID);
			base.AddXamlAttribute("Orientation", this.Orientation);
			base.AddXamlAttribute("Precision", this.Precision);
			base.AddXamlAttribute("Scale", this.SliderScale);
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("StateSignalName", this.StateSignalName);
			base.AddXamlAttribute("WriteSignalName", this.WriteSignalName);
			base.AddXamlAttribute("SignalPrefix", base.GetSignalPrefix());
			base.AddXamlAttribute("SmallChange", this.SmallChange);
			base.AddXamlAttribute("VisibilityMask", this.VisibilityMask);
			base.AddXamlAttribute("VisibilitySignalName", this.VisibilitySignalName);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = true;
			base.IsForegroundVisible = true;
		}
	}
}
