// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.Buttons.ViewModels
{
	public class HMIToggleButtonBaseViewModel : CommonControlViewModel
	{
		private string objectID;
		private SignalPrefix signalPrefix;
		private string signalName;
		private bool useSynchronizedToggling;
		private double toggleValue1;
		private double toggleValue2 = 1.0;
		private bool writeSecure;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private int? enableMask;
		private string enableSignalName;
		private int? visibilityMask;
		private string visibilitySignalName;
		private bool isTipModeEnabled;
		public event System.EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event System.EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseSynchronizedTogglingChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> Value1Changed;
		public event System.EventHandler<ValueChangedEventArgs<double>> Value2Changed;
		public event System.EventHandler<ValueChangedEventArgs<bool>> WriteSecureChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> EnableMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> EnableSignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> VisibilityMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> VisibilitySignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsTipModeEnabledChanged;
		[Browsable(true), DisplayName("ObjectName"), EditorOrder(-248.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public string ObjectID
		{
			get
			{
				return this.objectID;
			}
			set
			{
				if (this.objectID != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.objectID, value);
					this.objectID = value;
					this.ObjectIDChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ObjectID);
				}
			}
		}
		[Browsable(true), EditorOrder(-249.0), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public SignalPrefix SignalPrefix
		{
			get
			{
				return this.signalPrefix;
			}
			set
			{
				if (this.signalPrefix != value)
				{
					ValueChangedEventArgs<SignalPrefix> e = new ValueChangedEventArgs<SignalPrefix>(this.signalPrefix, value);
					this.signalPrefix = value;
					this.SignalPrefixChanged.Raise(this, e);
					base.RaisePropertyChanged<SignalPrefix>(() => this.SignalPrefix);
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
					this.signalName = value;
					this.SignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SignalName);
				}
			}
		}
		[PropertyCategory("General", typeof(StandardPropertyCategories))]
		public bool UseSynchronizedToggling
		{
			get
			{
				return this.useSynchronizedToggling;
			}
			set
			{
				if (this.useSynchronizedToggling != value)
				{
					base.RaisePropertyChanging<bool>(() => this.UseSynchronizedToggling);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.useSynchronizedToggling, value);
					this.useSynchronizedToggling = value;
					this.UseSynchronizedTogglingChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.UseSynchronizedToggling);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public double Value1
		{
			get
			{
				return this.toggleValue1;
			}
			set
			{
				if (this.toggleValue1 != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.toggleValue1, value);
					this.toggleValue1 = value;
					this.Value1Changed.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Value1);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public double Value2
		{
			get
			{
				return this.toggleValue2;
			}
			set
			{
				if (this.toggleValue2 != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.toggleValue2, value);
					this.toggleValue2 = value;
					this.Value2Changed.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Value2);
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
					this.writeSecure = value;
					this.WriteSecureChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.WriteSecure);
				}
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
					this.visibilitySignalName = value;
					this.VisibilitySignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.VisibilitySignalName);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public bool IsTipModeEnabled
		{
			get
			{
				return this.isTipModeEnabled;
			}
			set
			{
				if (this.isTipModeEnabled != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isTipModeEnabled, value);
					this.isTipModeEnabled = value;
					this.IsTipModeEnabledChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsTipModeEnabled);
				}
			}
		}
		public HMIToggleButtonBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}

		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIButtons=\"clr-namespace:HMI.RuntimeControls.Buttons;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIButtons;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("ObjectID", this.ObjectID);
			base.AddXamlAttribute("SignalPrefix", base.GetSignalPrefix());
			base.AddXamlAttribute("SignalName", this.SignalName);
			base.AddXamlAttribute("UseSynchronizedToggling", this.UseSynchronizedToggling);
			base.AddXamlAttribute("Value1", this.Value1);
			base.AddXamlAttribute("Value2", this.Value2);
			base.AddXamlAttribute("WriteSecure", this.WriteSecure);
			base.AddXamlAttribute("EnableMask", this.EnableMask);
			base.AddXamlAttribute("EnableSignalName", this.EnableSignalName);
			base.AddXamlAttribute("VisibilityMask", this.VisibilityMask);
			base.AddXamlAttribute("VisibilitySignalName", this.VisibilitySignalName);
			base.AddXamlAttribute("IsTipModeEnabled", this.IsTipModeEnabled);
		}

		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
