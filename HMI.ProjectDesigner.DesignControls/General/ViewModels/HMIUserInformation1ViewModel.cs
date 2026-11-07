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
	public class HMIUserInformation1ViewModel : CommonControlViewModel
	{
		private UserInformationField userInformation;
		private Brush borderBrush;
		private Thickness borderThickness;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Center;
		private VerticalAlignment verticalContentAlignment = VerticalAlignment.Center;
		private bool useLocalColors;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;

		public event EventHandler<ValueChangedEventArgs<UserInformationField>> UserInformationChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> BorderBrushChanged;
		public event EventHandler<ValueChangedEventArgs<Thickness>> BorderThicknessChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event EventHandler<ValueChangedEventArgs<HorizontalAlignment>> TextHorizontalAlignmentChanged;
		public event EventHandler<ValueChangedEventArgs<VerticalAlignment>> TextVerticalAlignmentChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		public event EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;

		public UserInformationField UserInformation
		{
			get
			{
				return userInformation;
			}
			set
			{
				if (userInformation != value)
				{
                    RaisePropertyChanging(() => UserInformation);
					ValueChangedEventArgs<UserInformationField> e = new ValueChangedEventArgs<UserInformationField>(userInformation, value);
                    userInformation = value;
                    UserInformationChanged.Raise(this, e);
                    RaisePropertyChanged(() => UserInformation);
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
		[Browsable(true), ProjectAuthorizationsBrowser, PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public string SecurityAuthorizationName
		{
			get
			{
				return securityAuthorizationName;
			}
			set
			{
				if (securityAuthorizationName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(securityAuthorizationName, value);
                    RaisePropertyChanging(() => SecurityAuthorizationName);
                    securityAuthorizationName = value;
                    SecurityAuthorizationNameChanged.Raise(this, e);
                    RaisePropertyChanged(() => SecurityAuthorizationName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public SecurityDenyAccessBehavior SecurityDenyAccessBehaviour
		{
			get
			{
				return securityDenyAccessBehavior;
			}
			set
			{
				if (securityDenyAccessBehavior != value)
				{
					ValueChangedEventArgs<SecurityDenyAccessBehavior> e = new ValueChangedEventArgs<SecurityDenyAccessBehavior>(securityDenyAccessBehavior, value);
                    RaisePropertyChanging(() => SecurityDenyAccessBehaviour);
                    securityDenyAccessBehavior = value;
                    SecurityDenyAccessBehaviorChanged.Raise(this, e);
                    RaisePropertyChanged(() => SecurityDenyAccessBehaviour);
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
		public HMIUserInformation1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIUserInformation1";
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
			base.AddXamlAttribute("UserInformation", this.UserInformation);
			base.AddXamlAttribute("Background", this.UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("Foreground", this.UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
			base.AddXamlAttribute("BorderBrush", this.BorderBrush);
			base.AddXamlAttribute("BorderThickness", this.BorderThickness);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
			base.AddXamlAttribute("HorizontalContentAlignment", this.HorizontalContentAlignment);
			base.AddXamlAttribute("VerticalContentAlignment", this.VerticalContentAlignment);
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
            IsBackgroundVisible = true;
            IsForegroundVisible = true;
            IsTemplateEnabled = true;
		}
	}
}
