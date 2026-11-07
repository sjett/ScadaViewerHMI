// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Labels.ViewModels
{
	public class HMILabel1ViewModel : HMIStaticStatesControlBaseViewModel
	{
		private string symbolicText;
		private System.Windows.TextAlignment textAlignment = System.Windows.TextAlignment.Center;
		private System.Windows.Media.Brush borderBrush;
		private System.Windows.Thickness borderThickness;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private System.Windows.HorizontalAlignment horizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
		private System.Windows.VerticalAlignment verticalContentAlignment = System.Windows.VerticalAlignment.Center;

		public event System.EventHandler<ValueChangedEventArgs<string>> SymbolicTextChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.TextAlignment>> TextAlignmentChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> BorderBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Thickness>> BorderThicknessChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.HorizontalAlignment>> TextHorizontalAlignmentChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.VerticalAlignment>> TextVerticalAlignmentChanged;

		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(100.0, 25.0);
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
		[PropertyCategory("Layout", typeof(StandardPropertyCategories)), TextAlignmentEditor]
		public System.Windows.TextAlignment TextAlignment
		{
			get
			{
				return this.textAlignment;
			}
			set
			{
				if (this.textAlignment != value)
				{
					base.RaisePropertyChanging<System.Windows.TextAlignment>(() => this.TextAlignment);
					ValueChangedEventArgs<System.Windows.TextAlignment> e = new ValueChangedEventArgs<System.Windows.TextAlignment>(this.textAlignment, value);
					this.textAlignment = value;
					this.TextAlignmentChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.TextAlignment>(() => this.TextAlignment);
				}
			}
		}
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
		public HMILabel1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMILabel1";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMILabels=\"clr-namespace:HMI.RuntimeControls.Labels;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMILabels;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("SymbolicText", this.SymbolicText);
			base.AddXamlAttribute("TextAlignment", this.TextAlignment);
			base.AddXamlAttribute("BorderBrush", this.BorderBrush);
			base.AddXamlAttribute("BorderThickness", this.BorderThickness);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
			base.AddXamlAttribute("HorizontalContentAlignment", this.HorizontalContentAlignment);
			base.AddXamlAttribute("VerticalContentAlignment", this.VerticalContentAlignment);
		}
	}
}
