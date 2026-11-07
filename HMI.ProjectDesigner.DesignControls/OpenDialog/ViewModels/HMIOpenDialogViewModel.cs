// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.OpenDialog.ViewModels
{
	public class HMIOpenDialogViewModel : CommonControlViewModel
	{
		private Brush borderBrush = Brushes.Black;
		private Thickness borderThickness = new Thickness(1.0);
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private string symbolicText;
		private bool useLocalColors;
		public event EventHandler<ValueChangedEventArgs<Brush>> BorderBrushChanged;
		public event EventHandler<ValueChangedEventArgs<Thickness>> BorderThicknessChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event EventHandler<ValueChangedEventArgs<string>> SymbolicTextChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush BorderBrush
		{
			get
			{
				return borderBrush;
			}
			set
			{
				if (borderBrush != value)
				{
					base.RaisePropertyChanging<Brush>(() => BorderBrush);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(borderBrush, value);
					borderBrush = value;
					BorderBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<Brush>(() => BorderBrush);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Thickness BorderThickness
		{
			get
			{
				return borderThickness;
			}
			set
			{
				if (borderThickness != value)
				{
					base.RaisePropertyChanging<Thickness>(() => BorderThickness);
					ValueChangedEventArgs<Thickness> e = new ValueChangedEventArgs<Thickness>(borderThickness, value);
					borderThickness = value;
					BorderThicknessChanged.Raise(this, e);
					base.RaisePropertyChanged<Thickness>(() => BorderThickness);
				}
			}
		}
		[Browsable(true), FontFamilyEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontFamily
		{
			get
			{
				return fontFamily;
			}
			set
			{
				if (fontFamily != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(fontFamily, value);
					base.RaisePropertyChanging<string>(() => FontFamily);
					fontFamily = value;
					FontFamilyChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => FontFamily);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MinimumValue(4.0)]
		public double FontSize
		{
			get
			{
				return fontSize;
			}
			set
			{
				if (fontSize != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(fontSize, value);
					base.RaisePropertyChanging<double>(() => FontSize);
					fontSize = value;
					FontSizeChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => FontSize);
				}
			}
		}
		[Browsable(true), FontStretchEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontStretch
		{
			get
			{
				return fontStretch;
			}
			set
			{
				if (fontStretch != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(fontStretch, value);
					base.RaisePropertyChanging<string>(() => FontStretch);
					fontStretch = value;
					FontStretchChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => FontStretch);
				}
			}
		}
		[Browsable(true), FontStyleEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontStyle
		{
			get
			{
				return fontStyle;
			}
			set
			{
				if (fontStyle != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(fontStyle, value);
					base.RaisePropertyChanging<string>(() => FontStyle);
					fontStyle = value;
					FontStyleChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => FontStyle);
				}
			}
		}
		[Browsable(true), FontWeightEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string FontWeight
		{
			get
			{
				return fontWeight;
			}
			set
			{
				if (fontWeight != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(fontWeight, value);
					base.RaisePropertyChanging<string>(() => FontWeight);
					fontWeight = value;
					FontWeightChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => FontWeight);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories)), SymbolicTextBrowser]
		public string SymbolicText
		{
			get
			{
				return symbolicText;
			}
			set
			{
				if (symbolicText != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicText, value);
					base.RaisePropertyChanging<string>(() => SymbolicText);
					symbolicText = value;
					SymbolicTextChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => SymbolicText);
				}
			}
		}
		[Browsable(true), EditorOrder(-253.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool UseLocalColors
		{
			get
			{
				return useLocalColors;
			}
			set
			{
				if (useLocalColors != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(useLocalColors, value);
					base.RaisePropertyChanging<bool>(() => UseLocalColors);
					useLocalColors = value;
					UseLocalColorsChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => UseLocalColors);
				}
			}
		}
		public override Size DesignerDesiredSize
		{
			get
			{
				return new Size(100.0, 25.0);
			}
		}
		public HMIOpenDialogViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIOpenDialog";
		}
		public override IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIOpenDialog=\"clr-namespace:HMI.RuntimeControls.OpenDialog;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIOpenDialog;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Background", UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("Foreground", UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
			base.AddXamlAttribute("BorderBrush", BorderBrush);
			base.AddXamlAttribute("BorderThickness", BorderThickness);
			base.AddXamlAttribute("FontFamily", FontFamily);
			base.AddXamlAttribute("FontSize", FontSize);
			base.AddXamlAttribute("FontStretch", FontStretch);
			base.AddXamlAttribute("FontStyle", FontStyle);
			base.AddXamlAttribute("FontWeight", FontWeight);
			base.AddXamlAttribute("SymbolicText", SymbolicText);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = true;
			base.IsForegroundVisible = true;
			base.IsTemplateEnabled = true;
			base.IsShowTemplateEnabled = false;
		}
	}
}
