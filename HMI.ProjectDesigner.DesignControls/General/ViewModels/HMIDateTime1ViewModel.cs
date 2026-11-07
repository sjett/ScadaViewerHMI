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
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.General.ViewModels
{
	public class HMIDateTime1ViewModel : CommonControlViewModel
	{
		public enum DateTimeSource
		{
			Client,
			Server,
			ServerToClient
		}
		private string fontFamily = "Arial";
		private double fontSize = 10.0;
		private string fontStretch = "Normal";
		private string fontStyle = "Normal";
		private string fontWeight = "Normal";

		private string format;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehaviour;
		private string serverName;
		private DateTimeSource source;
		private bool useLocalColors;
		public event EventHandler<ValueChangedEventArgs<string>> FontFamilyChanged;
		public event EventHandler<ValueChangedEventArgs<double>> FontSizeChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStretchChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontStyleChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FontWeightChanged;
		public event EventHandler<ValueChangedEventArgs<string>> FormatChanged;
		public event EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviourChanged;
		public event EventHandler<ValueChangedEventArgs<string>> ServerNameChanged;
		public event EventHandler<ValueChangedEventArgs<DateTimeSource>> SourceChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;

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
        [Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
        public string Format
		{
			get
			{
				return format;
			}
			set
			{
				if (format != value)
				{
					RaisePropertyChanging(() => Format);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(format, value);
					format = value;
					FormatChanged.Raise(this, e);
					RaisePropertyChanged(() => Format);
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
					RaisePropertyChanging(() => SecurityAuthorizationName);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(securityAuthorizationName, value);
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
				return securityDenyAccessBehaviour;
			}
			set
			{
				if (securityDenyAccessBehaviour != value)
				{
					RaisePropertyChanging(() => SecurityDenyAccessBehaviour);
					ValueChangedEventArgs<SecurityDenyAccessBehavior> e = new ValueChangedEventArgs<SecurityDenyAccessBehavior>(securityDenyAccessBehaviour, value);
					securityDenyAccessBehaviour = value;
					SecurityDenyAccessBehaviourChanged.Raise(this, e);
					RaisePropertyChanged(() => SecurityDenyAccessBehaviour);
				}
			}
		}
		[Browsable(true), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public string ServerName
		{
			get
			{
				return serverName;
			}
			set
			{
				if (serverName != value)
				{
					RaisePropertyChanging(() => ServerName);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(serverName, value);
					serverName = value;
					ServerNameChanged.Raise(this, e);
					RaisePropertyChanged(() => ServerName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public DateTimeSource Source
		{
			get
			{
				return source;
			}
			set
			{
				if (source != value)
				{
					RaisePropertyChanging(() => Source);
					ValueChangedEventArgs<DateTimeSource> e = new ValueChangedEventArgs<DateTimeSource>(source, value);
					source = value;
					SourceChanged.Raise(this, e);
					RaisePropertyChanged(() => Source);
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
		public HMIDateTime1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIDateTime1";
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
			base.AddXamlAttribute("Format", this.Format);
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("ServerName", this.ServerName);
			base.AddXamlAttribute("Source", this.Source);
		}
	}
}
