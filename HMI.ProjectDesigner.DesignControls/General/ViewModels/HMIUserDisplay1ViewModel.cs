// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.General.ViewModels
{
	public class HMIUserDisplay1ViewModel : CommonControlViewModel
	{
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior = SecurityDenyAccessBehavior.Hide;
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";
		private System.Windows.HorizontalAlignment horizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
		private System.Windows.VerticalAlignment verticalContentAlignment = System.Windows.VerticalAlignment.Center;
		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
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
					this.securityDenyAccessBehavior = value;
					this.SecurityDenyAccessBehaviorChanged.Raise(this, e);
					base.RaisePropertyChanged<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehaviour);
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
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
		public HMIUserDisplay1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIUserDisplay1";
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
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("FontFamily", this.FontFamily);
			base.AddXamlAttribute("FontSize", this.FontSize);
			base.AddXamlAttribute("FontStretch", this.FontStretch);
			base.AddXamlAttribute("FontStyle", this.FontStyle);
			base.AddXamlAttribute("FontWeight", this.FontWeight);
			base.AddXamlAttribute("HorizontalContentAlignment", this.HorizontalContentAlignment);
			base.AddXamlAttribute("VerticalContentAlignment", this.VerticalContentAlignment);
			base.AddXamlAttribute("Background", base.Background);
			base.AddXamlAttribute("Foreground", base.Foreground);
		}
	}
}
