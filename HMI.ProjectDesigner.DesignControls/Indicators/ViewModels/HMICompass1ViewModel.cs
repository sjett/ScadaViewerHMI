// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using System.Windows.Media;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.Indicators.ViewModels
{
	public class HMICompass1ViewModel : HMIIndicatorBaseViewModel
	{
		private System.Windows.Media.Brush background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 214, 214, 214));
		private System.Windows.Media.Brush borderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 183, 183, 183));
		private System.Windows.Media.Brush chromeFrontBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(34, 255, 255, 255));
		private double compassAngle;
		private string fontFamily = "Arial";
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Bold";
		private int majorLabelsFontSize = 36;
		private System.Windows.Media.Brush majorLabelsForeground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 63, 63, 63));
		private bool majorLabelsVisible = true;
		private System.Windows.Media.Brush majorScaleFillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 120, 120, 120));
		private int minorLabelsFontSize = 18;
		private System.Windows.Media.Brush minorLabelsForeground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 43, 43, 43));
		private bool minorLabelsVisible = true;
		private System.Windows.Media.Brush minorScaleFillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 108, 108, 108));
		private System.Windows.Media.Brush pointerFillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 219, 219, 219));
		private System.Windows.Media.Brush pointerHeadFillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 122, 186));
		private bool pointerIsInverted;
		private System.Windows.Media.Brush pointerStrokeBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 98, 98, 98));
		private string signalName;
		private bool useLocalColors;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> BackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> BorderBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ChromeFrontBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> CompassAngleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> MajorLabelsFontSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> MajorLabelsForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> MajorLabelsVisibleChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> MajorScaleFillBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> MinorLabelsFontSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> MinorLabelsForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> MinorLabelsVisibleChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> MinorScaleFillBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> PointerFillBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> PointerHeadFillBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> PointerIsInvertedChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> PointerStrokeBrushChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		public new System.Windows.Media.Brush Background
		{
			get
			{
				return this.background;
			}
			set
			{
				if (this.background != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.Background);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.background, value);
					this.background = value;
					this.BackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.Background);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
		[Browsable(false)]
		public double CompassAngle
		{
			get
			{
				return this.compassAngle;
			}
			set
			{
				if (this.compassAngle != value)
				{
					base.RaisePropertyChanging<double>(() => this.CompassAngle);
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.compassAngle, value);
					this.compassAngle = value;
					this.CompassAngleChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.CompassAngle);
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
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public int MajorLabelsFontSize
		{
			get
			{
				return this.majorLabelsFontSize;
			}
			set
			{
				if (this.majorLabelsFontSize != value)
				{
					base.RaisePropertyChanging<int>(() => this.MajorLabelsFontSize);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.majorLabelsFontSize, value);
					this.majorLabelsFontSize = value;
					this.MajorLabelsFontSizeChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.MajorLabelsFontSize);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush MajorLabelsForeground
		{
			get
			{
				return this.majorLabelsForeground;
			}
			set
			{
				if (this.majorLabelsForeground != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.MajorLabelsForeground);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.majorLabelsForeground, value);
					this.majorLabelsForeground = value;
					this.MajorLabelsForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.MajorLabelsForeground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool MajorLabelsVisible
		{
			get
			{
				return this.majorLabelsVisible;
			}
			set
			{
				if (this.majorLabelsVisible != value)
				{
					base.RaisePropertyChanging<bool>(() => this.MajorLabelsVisible);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.majorLabelsVisible, value);
					this.majorLabelsVisible = value;
					this.MajorLabelsVisibleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.MajorLabelsVisible);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush MajorScaleFillBrush
		{
			get
			{
				return this.majorScaleFillBrush;
			}
			set
			{
				if (this.majorScaleFillBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.MajorScaleFillBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.majorScaleFillBrush, value);
					this.majorScaleFillBrush = value;
					this.MajorScaleFillBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.MajorScaleFillBrush);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public int MinorLabelsFontSize
		{
			get
			{
				return this.minorLabelsFontSize;
			}
			set
			{
				if (this.minorLabelsFontSize != value)
				{
					base.RaisePropertyChanging<int>(() => this.MinorLabelsFontSize);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.minorLabelsFontSize, value);
					this.minorLabelsFontSize = value;
					this.MinorLabelsFontSizeChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.MinorLabelsFontSize);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush MinorLabelsForeground
		{
			get
			{
				return this.minorLabelsForeground;
			}
			set
			{
				if (this.minorLabelsForeground != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.MinorLabelsForeground);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.minorLabelsForeground, value);
					this.minorLabelsForeground = value;
					this.MinorLabelsForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.MinorLabelsForeground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool MinorLabelsVisible
		{
			get
			{
				return this.minorLabelsVisible;
			}
			set
			{
				if (this.minorLabelsVisible != value)
				{
					base.RaisePropertyChanging<bool>(() => this.MinorLabelsVisible);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.minorLabelsVisible, value);
					this.minorLabelsVisible = value;
					this.MinorLabelsVisibleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.MinorLabelsVisible);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush MinorScaleFillBrush
		{
			get
			{
				return this.minorScaleFillBrush;
			}
			set
			{
				if (this.minorScaleFillBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.MinorScaleFillBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.minorScaleFillBrush, value);
					this.minorScaleFillBrush = value;
					this.MinorScaleFillBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.MinorScaleFillBrush);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush PointerFillBrush
		{
			get
			{
				return this.pointerFillBrush;
			}
			set
			{
				if (this.pointerFillBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.PointerFillBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.pointerFillBrush, value);
					this.pointerFillBrush = value;
					this.PointerFillBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.PointerFillBrush);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush PointerHeadFillBrush
		{
			get
			{
				return this.pointerHeadFillBrush;
			}
			set
			{
				if (this.pointerHeadFillBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.PointerHeadFillBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.pointerHeadFillBrush, value);
					this.pointerHeadFillBrush = value;
					this.PointerHeadFillBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.PointerHeadFillBrush);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool PointerIsInverted
		{
			get
			{
				return this.pointerIsInverted;
			}
			set
			{
				if (this.pointerIsInverted != value)
				{
					base.RaisePropertyChanging<bool>(() => this.PointerIsInverted);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.pointerIsInverted, value);
					this.pointerIsInverted = value;
					this.PointerIsInvertedChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.PointerIsInverted);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush PointerStrokeBrush
		{
			get
			{
				return this.pointerStrokeBrush;
			}
			set
			{
				if (this.pointerStrokeBrush != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.PointerStrokeBrush);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.pointerStrokeBrush, value);
					this.pointerStrokeBrush = value;
					this.PointerStrokeBrushChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.PointerStrokeBrush);
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
					base.RaisePropertyChanging<string>(() => this.SignalName);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.signalName, value);
					this.signalName = value;
					this.SignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SignalName);
				}
			}
		}
		[EditorOrder(-253.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
					base.RaisePropertyChanging<bool>(() => this.UseLocalColors);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.useLocalColors, value);
					this.useLocalColors = value;
					this.UseLocalColorsChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.UseLocalColors);
				}
			}
		}
		public HMICompass1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMICompass1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Background", this.Background);
			base.AddXamlAttribute("BorderBrush", this.BorderBrush);
			base.AddXamlAttribute("ChromeFrontBrush", this.ChromeFrontBrush);
			base.AddXamlAttribute("CompassAngle", this.CompassAngle);
			base.AddXamlAttribute("MajorLabelsFontSize", this.MajorLabelsFontSize);
			base.AddXamlAttribute("MajorLabelsForeground", this.MajorLabelsForeground);
			base.AddXamlAttribute("MajorLabelsVisible", this.MajorLabelsVisible);
			base.AddXamlAttribute("MajorScaleFillBrush", this.MajorScaleFillBrush);
			base.AddXamlAttribute("MinorLabelsFontSize", this.MinorLabelsFontSize);
			base.AddXamlAttribute("MinorLabelsForeground", this.MinorLabelsForeground);
			base.AddXamlAttribute("MinorLabelsVisible", this.MinorLabelsVisible);
			base.AddXamlAttribute("MinorScaleFillBrush", this.MinorScaleFillBrush);
			base.AddXamlAttribute("PointerFillBrush", this.PointerFillBrush);
			base.AddXamlAttribute("PointerHeadFillBrush", this.PointerHeadFillBrush);
			base.AddXamlAttribute("PointerIsInverted", this.PointerIsInverted);
			base.AddXamlAttribute("PointerStrokeBrush", this.PointerStrokeBrush);
			base.AddXamlAttribute("SignalName", this.SignalName);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = true;
		}
	}
}
