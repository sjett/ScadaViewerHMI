// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Labels.ViewModels
{
	public class HMISignalInformationLabel1ViewModel : CommonControlViewModel
	{
		private string objectID;
		private SignalPrefix signalPrefix;
		private string signalName;
		private SignalInformationField signalInformation = SignalInformationField.Name;
		private bool useLocalColors;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Center;
		private VerticalAlignment verticalContentAlignment = VerticalAlignment.Center;
		private Brush borderBrush;
		private Thickness borderThickness;

		public event EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event EventHandler<ValueChangedEventArgs<SignalInformationField>> SignalInformationChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event EventHandler<ValueChangedEventArgs<HorizontalAlignment>> TextHorizontalAlignmentChanged;
		public event EventHandler<ValueChangedEventArgs<VerticalAlignment>> TextVerticalAlignmentChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> BorderBrushChanged;
		public event EventHandler<ValueChangedEventArgs<Thickness>> BorderThicknessChanged;

		[Browsable(true), DisplayName("ObjectName"), EditorOrder(-248.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public string ObjectID
		{
			get
			{
				return objectID;
			}
			set
			{
				if (objectID != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(objectID, value);
					RaisePropertyChanging(() => ObjectID);
					objectID = value;
					ObjectIDChanged.Raise(this, e);
					RaisePropertyChanged(() => ObjectID);
				}
			}
		}
		[Browsable(true), EditorOrder(-249.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public SignalPrefix SignalPrefix
		{
			get
			{
				return signalPrefix;
			}
			set
			{
				if (signalPrefix != value)
				{
					ValueChangedEventArgs<SignalPrefix> e = new ValueChangedEventArgs<SignalPrefix>(signalPrefix, value);
					RaisePropertyChanging(() => SignalPrefix);
					signalPrefix = value;
					SignalPrefixChanged.Raise(this, e);
					RaisePropertyChanged(() => SignalPrefix);
				}
			}
		}
		[EditorOrder(-250.0), PropertyCategory("Signals", typeof(StandardPropertyCategories)), SignalBrowser]
		public string SignalName
		{
			get
			{
				return signalName;
			}
			set
			{
				if (signalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(signalName, value);
					RaisePropertyChanging(() => SignalName);
					signalName = value;
					SignalNameChanged.Raise(this, e);
					RaisePropertyChanged(() => SignalName);
				}
			}
		}
		[Browsable(false)]
		public string ResolvedSignalName
		{
			get
			{
				return string.Format("{0}:{1}", GetFullSignalName(SignalName), SignalInformation);
			}
		}
		[PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public SignalInformationField SignalInformation
		{
			get
			{
				return signalInformation;
			}
			set
			{
				if (signalInformation != value)
				{
					ValueChangedEventArgs<SignalInformationField> e = new ValueChangedEventArgs<SignalInformationField>(signalInformation, value);
					RaisePropertyChanging(() => SignalInformation);
					signalInformation = value;
					SignalInformationChanged.Raise(this, e);
					RaisePropertyChanged(() => SignalInformation);
					RaisePropertyChanged(() => ResolvedSignalName);
				}
			}
		}
		[EditorOrder(-253.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
		[Browsable(true), EditorOrder(-235.0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public HorizontalAlignment HorizontalContentAlignment
		{
			get
			{
				return horizontalContentAlignment;
			}
			set
			{
				if (horizontalContentAlignment != value)
				{
					ValueChangedEventArgs<HorizontalAlignment> e = new ValueChangedEventArgs<HorizontalAlignment>(horizontalContentAlignment, value);
					RaisePropertyChanging(() => HorizontalContentAlignment);
					horizontalContentAlignment = value;
					TextHorizontalAlignmentChanged.Raise(this, e);
					RaisePropertyChanged(() => HorizontalContentAlignment);
				}
			}
		}
		[Browsable(true), EditorOrder(-234.0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
		public VerticalAlignment VerticalContentAlignment
		{
			get
			{
				return verticalContentAlignment;
			}
			set
			{
				if (verticalContentAlignment != value)
				{
					ValueChangedEventArgs<VerticalAlignment> e = new ValueChangedEventArgs<VerticalAlignment>(verticalContentAlignment, value);
					RaisePropertyChanging(() => VerticalContentAlignment);
					verticalContentAlignment = value;
					TextVerticalAlignmentChanged.Raise(this, e);
					RaisePropertyChanged(() => VerticalContentAlignment);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
		public override Size DesignerDesiredSize
		{
			get
			{
				return new Size(100.0, 25.0);
			}
		}
		public HMISignalInformationLabel1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
			RegisterResolvedSignalNameProperty(() => SignalName, () => ResolvedSignalName);
		}
		public override string GetXamlControlName()
		{
			return "HMISignalInformationLabel1";
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
			base.AddXamlAttribute("SignalName", this.SignalName);
			base.AddXamlAttribute("SignalInformation", this.SignalInformation);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
			base.AddXamlAttribute("HorizontalContentAlignment", this.HorizontalContentAlignment);
			base.AddXamlAttribute("VerticalContentAlignment", this.VerticalContentAlignment);
			base.AddXamlAttribute("BorderBrush", this.BorderBrush);
			base.AddXamlAttribute("BorderThickness", this.BorderThickness);
			base.AddXamlAttribute("Foreground", this.UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
			base.AddXamlAttribute("Background", this.UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			IsBackgroundVisible = true;
			IsForegroundVisible = true;
		}
	}
}
