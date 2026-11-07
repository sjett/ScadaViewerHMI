// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;

namespace HMI.ProjectDesigner.DesignControls.General.ViewModels
{
	public class HMILogFileViewerViewModel : CommonControlViewModel
	{
		private Brush borderBrush = Brushes.Black;
		private Thickness borderThickness = new Thickness(1.0);
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private DialogModality popupModality;
		private string symbolicText = "HMILogFileViewer";
		private bool useLocalColors;

		public event EventHandler<ValueChangedEventArgs<Brush>> BorderBrushChanged;
		public event EventHandler<ValueChangedEventArgs<Thickness>> BorderThicknessChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event EventHandler<ValueChangedEventArgs<DialogModality>> PopupModalityChanged;
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
					RaisePropertyChanging(() => BorderBrush);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(borderBrush, value);
					borderBrush = value;
					BorderBrushChanged.Raise(this, e);
					RaisePropertyChanged(() => BorderBrush);
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
					RaisePropertyChanging(() => BorderThickness);
					ValueChangedEventArgs<Thickness> e = new ValueChangedEventArgs<Thickness>(borderThickness, value);
					borderThickness = value;
					BorderThicknessChanged.Raise(this, e);
					RaisePropertyChanged(() => BorderThickness);
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
					RaisePropertyChanging(() => FontFamily);
					fontFamily = value;
					FontFamilyChanged.Raise(this, e);
					RaisePropertyChanged(() => FontFamily);
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
					RaisePropertyChanging(() => FontSize);
					fontSize = value;
					FontSizeChanged.Raise(this, e);
					RaisePropertyChanged(() => FontSize);
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
					RaisePropertyChanging(() => FontStretch);
					fontStretch = value;
					FontStretchChanged.Raise(this, e);
					RaisePropertyChanged(() => FontStretch);
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
					RaisePropertyChanging(() => FontStyle);
					fontStyle = value;
					FontStyleChanged.Raise(this, e);
					RaisePropertyChanged(() => FontStyle);
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
					RaisePropertyChanging(() => FontWeight);
					fontWeight = value;
					FontWeightChanged.Raise(this, e);
					RaisePropertyChanged(() => FontWeight);
				}
			}
		}
		[Browsable(true), PropertyCategory("Templating", typeof(StandardPropertyCategories))]
		public DialogModality PopupModality
		{
			get
			{
				return popupModality;
			}
			set
			{
				if (popupModality != value)
				{
					RaisePropertyChanging(() => PopupModality);
					ValueChangedEventArgs<DialogModality> e = new ValueChangedEventArgs<DialogModality>(popupModality, value);
					popupModality = value;
					PopupModalityChanged.Raise(this, e);
					RaisePropertyChanged(() => PopupModality);
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
					RaisePropertyChanging(() => SymbolicText);
					symbolicText = value;
					SymbolicTextChanged.Raise(this, e);
					RaisePropertyChanged(() => SymbolicText);
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
					RaisePropertyChanging(() => UseLocalColors);
					useLocalColors = value;
					UseLocalColorsChanged.Raise(this, e);
					RaisePropertyChanged(() => UseLocalColors);
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
		public HMILogFileViewerViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMILogFileViewer";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIGeneral=\"clr-namespace:HMI.RuntimeControls.General;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIGeneral;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Background", this.UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("Foreground", this.UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
			base.AddXamlAttribute("BorderBrush", this.BorderBrush);
			base.AddXamlAttribute("BorderThickness", this.BorderThickness);
			base.AddXamlAttribute("PopupModality", this.PopupModality);
			base.AddXamlAttribute("Content", this.SymbolicText);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			IsBackgroundVisible = true;
			IsForegroundVisible = true;
		}
	}
}
