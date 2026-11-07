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
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.Gauges.ViewModels
{
	public class HMIBargraphBaseViewModel : CommonControlViewModel
	{
		private string objectID;
		private SignalPrefix signalPrefix;
		private string signalName;
		private bool showScale;
		private double rangeMin;
		private double rangeMax = 100.0;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private int? visibilityMask;
		private string visibilitySignalName;

		public event System.EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event System.EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ShowScaleChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> RangeMinChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> RangeMaxChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> VisibilityMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> VisibilitySignalNameChanged;

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
					base.RaisePropertyChanging<string>(() => this.ObjectID);
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
					base.RaisePropertyChanging<SignalPrefix>(() => this.SignalPrefix);
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
					base.RaisePropertyChanging<string>(() => this.SignalName);
					this.signalName = value;
					this.SignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SignalName);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool ShowScale
		{
			get
			{
				return this.showScale;
			}
			set
			{
				if (this.showScale != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.showScale, value);
					base.RaisePropertyChanging<bool>(() => this.ShowScale);
					this.showScale = value;
					this.ShowScaleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ShowScale);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue("RangeMax")]
		public virtual double RangeMin
		{
			get
			{
				return this.rangeMin;
			}
			set
			{
				if (this.rangeMin != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.rangeMin, value);
					base.RaisePropertyChanging<double>(() => this.RangeMin);
					this.rangeMin = value;
					this.RangeMinChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.RangeMin);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue("RangeMin")]
		public virtual double RangeMax
		{
			get
			{
				return this.rangeMax;
			}
			set
			{
				if (this.rangeMax != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.rangeMax, value);
					base.RaisePropertyChanging<double>(() => this.RangeMax);
					this.rangeMax = value;
					this.RangeMaxChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.RangeMax);
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
					base.RaisePropertyChanging<string>(() => this.SecurityAuthorizationName);
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
					base.RaisePropertyChanging<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehaviour);
					this.securityDenyAccessBehavior = value;
					this.SecurityDenyAccessBehaviorChanged.Raise(this, e);
					base.RaisePropertyChanged<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehaviour);
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
					base.RaisePropertyChanging<int?>(() => this.VisibilityMask);
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
					base.RaisePropertyChanging<string>(() => this.VisibilitySignalName);
					this.visibilitySignalName = value;
					this.VisibilitySignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.VisibilitySignalName);
				}
			}
		}
		public HMIBargraphBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIGauges=\"clr-namespace:HMI.RuntimeControls.Gauges;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIGauges;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ObjectID", this.ObjectID);
			base.AddXamlAttribute("SignalPrefix", base.GetSignalPrefix());
			base.AddXamlAttribute("SignalName", this.SignalName);
			base.AddXamlAttribute("Scale", this.ShowScale);
			base.AddXamlAttribute("RangeMin", this.RangeMin);
			base.AddXamlAttribute("RangeMax", this.RangeMax);
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("VisibilitySignalName", this.VisibilitySignalName);
			base.AddXamlAttribute("VisibilityMask", this.VisibilityMask);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
