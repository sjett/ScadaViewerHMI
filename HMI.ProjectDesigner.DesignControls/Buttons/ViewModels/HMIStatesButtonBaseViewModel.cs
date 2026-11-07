// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using System.ComponentModel;

namespace HMI.ProjectDesigner.DesignControls.Buttons.ViewModels
{
	public class HMIStatesButtonBaseViewModel : HMIStaticStatesControlBaseViewModel
	{
		private System.Windows.Media.Brush borderBrush;
		private System.Windows.Media.Brush chromeFrontBrush;
		private System.Windows.Media.Brush mouseOverBrush;
		private System.Windows.Thickness borderThickness = new System.Windows.Thickness(1.0, 1.0, 1.0, 2.0);
		private System.Windows.CornerRadius cornerRadius = new System.Windows.CornerRadius(2.0);
		private System.Windows.Thickness padding = new System.Windows.Thickness(3.0);
		private System.Windows.Thickness iconMargin;
		private int iconMaxSize = 24;
		private double iconOpacity = 100.0;
		private ButtonIconPlacement iconPlacement;
		private System.Windows.Media.Stretch iconStretch = System.Windows.Media.Stretch.Uniform;
		private int? enableMask;
		private string enableSignalName;
		private int? visibilityMask;
		private string visibilitySignalName;
		private string signalName;
		private string symbolicText;
		private bool writeSecure;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> BorderBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ChromeFrontBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> MouseOverBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Thickness>> BorderThicknessChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.CornerRadius>> CornerRadiusChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Thickness>> PaddingChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Thickness>> IconMarginChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> IconMaxSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> IconOpacityChanged;
		public event System.EventHandler<ValueChangedEventArgs<ButtonIconPlacement>> IconPlacementChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Stretch>> IconStretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> EnableMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> EnableSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> VisibilityMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> VisibilitySignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SymbolicTextChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> WriteSecureChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush BorderBrush
		{
			get
			{
				return this.borderBrush;
			}
			set
			{
				if (this.borderBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.BorderBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.borderBrush, value);
					this.borderBrush = value;
					this.BorderBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.BorderBrush);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush ChromeFrontBrush
		{
			get
			{
				return this.chromeFrontBrush;
			}
			set
			{
				if (this.chromeFrontBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.ChromeFrontBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.chromeFrontBrush, value);
					this.chromeFrontBrush = value;
					this.ChromeFrontBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.ChromeFrontBrush);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush MouseOverBrush
		{
			get
			{
				return this.mouseOverBrush;
			}
			set
			{
				if (this.mouseOverBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.MouseOverBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.mouseOverBrush, value);
					this.mouseOverBrush = value;
					this.MouseOverBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.MouseOverBrush);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Thickness BorderThickness
		{
			get
			{
				return this.borderThickness;
			}
			set
			{
				if (this.borderThickness != value)
				{
					base.RaisePropertyChanging<System.Windows.Thickness>(() => this.BorderThickness);
					ValueChangedEventArgs<System.Windows.Thickness> e = new ValueChangedEventArgs<System.Windows.Thickness>(this.borderThickness, value);
					this.borderThickness = value;
					this.BorderThicknessChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Thickness>(() => this.BorderThickness);
				}
			}
		}
		public System.Windows.CornerRadius CornerRadius
		{
			get
			{
				return this.cornerRadius;
			}
			set
			{
				if (!object.Equals(this.cornerRadius, value))
				{
					base.RaisePropertyChanging<System.Windows.CornerRadius>(() => this.CornerRadius);
					ValueChangedEventArgs<System.Windows.CornerRadius> e = new ValueChangedEventArgs<System.Windows.CornerRadius>(this.cornerRadius, value);
					this.cornerRadius = value;
					this.CornerRadiusChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.CornerRadius>(() => this.CornerRadius);
				}
			}
		}
		[EditorOrder(-231.0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public System.Windows.Thickness Padding
		{
			get
			{
				return this.padding;
			}
			set
			{
				if (this.padding != value)
				{
					base.RaisePropertyChanging<System.Windows.Thickness>(() => this.Padding);
					ValueChangedEventArgs<System.Windows.Thickness> e = new ValueChangedEventArgs<System.Windows.Thickness>(this.padding, value);
					this.padding = value;
					this.PaddingChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Thickness>(() => this.Padding);
				}
			}
		}
		[PropertyCategory("Icons", typeof(StandardPropertyCategories))]
		public System.Windows.Thickness IconMargin
		{
			get
			{
				return this.iconMargin;
			}
			set
			{
				if (!object.Equals(this.iconMargin, value))
				{
					base.RaisePropertyChanging<System.Windows.Thickness>(() => this.IconMargin);
					ValueChangedEventArgs<System.Windows.Thickness> e = new ValueChangedEventArgs<System.Windows.Thickness>(this.iconMargin, value);
					this.iconMargin = value;
					this.IconMarginChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Thickness>(() => this.IconMargin);
				}
			}
		}
		[PropertyCategory("Icons", typeof(StandardPropertyCategories)), MinimumValue(0.0)]
		public int IconMaxSize
		{
			get
			{
				return this.iconMaxSize;
			}
			set
			{
				if (this.iconMaxSize != value)
				{
					base.RaisePropertyChanging<int>(() => this.IconMaxSize);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.iconMaxSize, value);
					this.iconMaxSize = value;
					this.IconMaxSizeChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.IconMaxSize);
				}
			}
		}
		[PropertyCategory("Icons", typeof(StandardPropertyCategories)), MaximumValue(100.0), MinimumValue(0.0)]
		public double IconOpacity
		{
			get
			{
				return this.iconOpacity;
			}
			set
			{
				if (!object.Equals(this.iconOpacity, value))
				{
					base.RaisePropertyChanging<double>(() => this.IconOpacity);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.iconOpacity, value);
					this.iconOpacity = value;
					this.IconOpacityChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.IconOpacity);
				}
			}
		}
		[PropertyCategory("Icons", typeof(StandardPropertyCategories))]
		public ButtonIconPlacement IconPlacement
		{
			get
			{
				return this.iconPlacement;
			}
			set
			{
				if (this.iconPlacement != value)
				{
					base.RaisePropertyChanging<ButtonIconPlacement>(() => this.IconPlacement);
					ValueChangedEventArgs<ButtonIconPlacement> e = new ValueChangedEventArgs<ButtonIconPlacement>(this.iconPlacement, value);
					this.iconPlacement = value;
					this.IconPlacementChanged.Raise(this, e);
					base.RaisePropertyChanged<ButtonIconPlacement>(() => this.IconPlacement);
				}
			}
		}
		[PropertyCategory("Icons", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Stretch IconStretch
		{
			get
			{
				return this.iconStretch;
			}
			set
			{
				if (this.iconStretch != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Stretch> e = new ValueChangedEventArgs<System.Windows.Media.Stretch>(this.iconStretch, value);
					base.RaisePropertyChanging<System.Windows.Media.Stretch>(() => this.IconStretch);
					this.iconStretch = value;
					this.IconStretchChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Stretch>(() => this.IconStretch);
				}
			}
		}
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
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicText
		{
			get
			{
				return this.symbolicText;
			}
			set
			{
				if (this.symbolicText != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.symbolicText, value);
					base.RaisePropertyChanging<string>(() => this.SymbolicText);
					this.symbolicText = value;
					this.SymbolicTextChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SymbolicText);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public bool WriteSecure
		{
			get
			{
				return this.writeSecure;
			}
			set
			{
				if (this.writeSecure != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.writeSecure, value);
					base.RaisePropertyChanging<bool>(() => this.WriteSecure);
					this.writeSecure = value;
					this.WriteSecureChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.WriteSecure);
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
				if (!object.Equals(this.fontSize, value))
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
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(100.0, 25.0);
			}
		}
		public HMIStatesButtonBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			base.Background = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 214, 214, 214)).GetAsFrozen();
			this.BorderBrush = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 170, 170, 170)).GetAsFrozen();
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIButtons=\"clr-namespace:HMI.RuntimeControls.Buttons;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIButtons;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("BorderBrush", this.BorderBrush);
			base.AddXamlAttribute("ChromeFrontBrush", this.ChromeFrontBrush);
			base.AddXamlAttribute("MouseOverBrush", this.MouseOverBrush);
			base.AddXamlAttribute("BorderThickness", this.BorderThickness);
			base.AddXamlAttribute("CornerRadius", this.CornerRadius);
			base.AddXamlAttribute("Padding", this.Padding);
			base.AddXamlAttribute("IconMargin", this.IconMargin);
			base.AddXamlAttribute("IconMaxSize", this.IconMaxSize);
			base.AddXamlAttribute("IconOpacity", this.IconOpacity);
			base.AddXamlAttribute("IconPlacement", this.IconPlacement);
			base.AddXamlAttribute("IconStretch", this.IconStretch);
			base.AddXamlAttribute("EnableMask", this.EnableMask);
			base.AddXamlAttribute("EnableSignalName", this.EnableSignalName);
			base.AddXamlAttribute("VisibilityMask", this.VisibilityMask);
			base.AddXamlAttribute("VisibilitySignalName", this.VisibilitySignalName);
			base.AddXamlAttribute("SignalName", this.SignalName);
			base.AddXamlAttribute("SymbolicText", this.SymbolicText);
			base.AddXamlAttribute("WriteSecure", this.WriteSecure);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
		}
	}
}
