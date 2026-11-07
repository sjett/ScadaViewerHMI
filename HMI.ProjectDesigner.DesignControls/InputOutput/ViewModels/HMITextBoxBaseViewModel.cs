// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using System.Windows.Media;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMITextBoxBaseViewModel : HMIIOControlBaseViewModel
	{
		private bool writeSecure;
		private string enableSignalName;
		private int? enableMask;
		private bool useLocalColors;
		private System.Windows.Media.Brush editModeHintBackground = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 249, 217, 130)).GetAsFrozen();
		public event System.EventHandler<ValueChangedEventArgs<bool>> WriteSecureChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> EnableSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> EnableMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> EditModeHintBackgroundChanged;
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
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush EditModeHintBackground
		{
			get
			{
				return this.editModeHintBackground;
			}
			set
			{
				if (this.editModeHintBackground != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.EditModeHintBackground);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.editModeHintBackground, value);
					this.editModeHintBackground = value;
					this.EditModeHintBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.EditModeHintBackground);
				}
			}
		}
		public HMITextBoxBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("WriteSecure", this.WriteSecure);
			base.AddXamlAttribute("EnableSignalName", this.EnableSignalName);
			base.AddXamlAttribute("EnableMask", this.EnableMask);
			base.AddXamlAttribute("Background", this.UseLocalColors ? base.Background : this.Project.StateDefaultBackground);
			base.AddXamlAttribute("Foreground", this.UseLocalColors ? base.Foreground : this.Project.StateDefaultForeground);
			base.AddXamlAttribute("EditModeHintBackground", this.EditModeHintBackground);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = true;
			base.IsForegroundVisible = true;
		}
	}
}
