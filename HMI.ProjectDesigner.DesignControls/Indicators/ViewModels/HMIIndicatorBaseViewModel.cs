// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.Indicators.ViewModels
{
	public class HMIIndicatorBaseViewModel : CommonControlViewModel
	{
		private readonly ICollectionMonitor<ConditionSignal> conditionSignalsMonitor;
		private string objectID;
		private SignalPrefix signalPrefix;
		private string securityAuthorizationName;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;
		private int? visibilityMask;
		private string visibilitySignalName;
		private bool binaryMask;
		private string conditionRule1;
		private string conditionRule2;
		private string conditionRule3;
		private string conditionRule4;
		private string conditionRule5;
		private string conditionRule6;
		private string conditionRule7;
		private string conditionRule8;
		private readonly IObservableCollection<ConditionSignal> conditionSignals = new FullyObservableCollection<ConditionSignal>();
		private int? maskSignal1;
		private int? maskSignal2;
		private int? maskSignal3;
		private int? maskSignal4;
		private int? maskSignal5;
		private int? maskSignal6;
		private int? maskSignal7;
		private int? maskSignal8;
		private string stateSignalName1;
		private string stateSignalName2;
		private string stateSignalName3;
		private string stateSignalName4;
		private string stateSignalName5;
		private string stateSignalName6;
		private string stateSignalName7;
		private string stateSignalName8;
		public event System.EventHandler<ValueChangedEventArgs<string>> ObjectIDChanged;
		public event System.EventHandler<ValueChangedEventArgs<SignalPrefix>> SignalPrefixChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;
		public event System.EventHandler<ValueChangedEventArgs<int?>> VisibilityMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> VisibilitySignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> BinaryMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule1Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule2Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule3Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule4Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule5Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule6Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule7Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule8Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal1Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal2Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal3Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal4Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal5Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal6Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal7Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal8Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalName1Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalName2Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalName3Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalName4Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalName5Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalName6Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalName7Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateSignalName8Changed;
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
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public bool BinaryMask
		{
			get
			{
				return this.binaryMask;
			}
			set
			{
				if (this.binaryMask != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.binaryMask, value);
					base.RaisePropertyChanging<bool>(() => this.BinaryMask);
					this.binaryMask = value;
					this.BinaryMaskChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.BinaryMask);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule1
		{
			get
			{
				return this.conditionRule1;
			}
			set
			{
				if (this.conditionRule1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule1, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule1);
					this.conditionRule1 = value;
					this.ConditionRule1Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule1);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule2
		{
			get
			{
				return this.conditionRule2;
			}
			set
			{
				if (this.conditionRule2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule2, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule2);
					this.conditionRule2 = value;
					this.ConditionRule2Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule2);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule3
		{
			get
			{
				return this.conditionRule3;
			}
			set
			{
				if (this.conditionRule3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule3, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule3);
					this.conditionRule3 = value;
					this.ConditionRule3Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule3);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule4
		{
			get
			{
				return this.conditionRule4;
			}
			set
			{
				if (this.conditionRule4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule4, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule4);
					this.conditionRule4 = value;
					this.ConditionRule4Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule4);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule5
		{
			get
			{
				return this.conditionRule5;
			}
			set
			{
				if (this.conditionRule5 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule5, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule5);
					this.conditionRule5 = value;
					this.ConditionRule5Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule5);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule6
		{
			get
			{
				return this.conditionRule6;
			}
			set
			{
				if (this.conditionRule6 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule6, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule6);
					this.conditionRule6 = value;
					this.ConditionRule6Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule6);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule7
		{
			get
			{
				return this.conditionRule7;
			}
			set
			{
				if (this.conditionRule7 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule7, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule7);
					this.conditionRule7 = value;
					this.ConditionRule7Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule7);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public string ConditionRule8
		{
			get
			{
				return this.conditionRule8;
			}
			set
			{
				if (this.conditionRule8 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.conditionRule8, value);
					base.RaisePropertyChanging<string>(() => this.ConditionRule8);
					this.conditionRule8 = value;
					this.ConditionRule8Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConditionRule8);
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(ConditionSignal)), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public IObservableCollection<ConditionSignal> ConditionSignals
		{
			get
			{
				return this.conditionSignals;
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal1
		{
			get
			{
				return this.maskSignal1;
			}
			set
			{
				if (this.maskSignal1 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal1, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal1);
					this.maskSignal1 = value;
					this.MaskSignal1Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal1);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal2
		{
			get
			{
				return this.maskSignal2;
			}
			set
			{
				if (this.maskSignal2 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal2, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal2);
					this.maskSignal2 = value;
					this.MaskSignal2Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal2);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal3
		{
			get
			{
				return this.maskSignal3;
			}
			set
			{
				if (this.maskSignal3 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal3, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal3);
					this.maskSignal3 = value;
					this.MaskSignal3Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal3);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal4
		{
			get
			{
				return this.maskSignal4;
			}
			set
			{
				if (this.maskSignal4 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal4, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal4);
					this.maskSignal4 = value;
					this.MaskSignal4Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal4);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal5
		{
			get
			{
				return this.maskSignal5;
			}
			set
			{
				if (this.maskSignal5 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal5, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal5);
					this.maskSignal5 = value;
					this.MaskSignal5Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal5);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal6
		{
			get
			{
				return this.maskSignal6;
			}
			set
			{
				if (this.maskSignal6 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal6, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal6);
					this.maskSignal6 = value;
					this.MaskSignal6Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal6);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal7
		{
			get
			{
				return this.maskSignal7;
			}
			set
			{
				if (this.maskSignal7 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal7, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal7);
					this.maskSignal7 = value;
					this.MaskSignal7Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal7);
				}
			}
		}
		[PropertyCategory("States", typeof(StandardPropertyCategories))]
		public int? MaskSignal8
		{
			get
			{
				return this.maskSignal8;
			}
			set
			{
				if (this.maskSignal8 != value)
				{
					ValueChangedEventArgs<int?> e = new ValueChangedEventArgs<int?>(this.maskSignal8, value);
					base.RaisePropertyChanging<int?>(() => this.MaskSignal8);
					this.maskSignal8 = value;
					this.MaskSignal8Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal8);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName1
		{
			get
			{
				return this.stateSignalName1;
			}
			set
			{
				if (this.stateSignalName1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName1, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName1);
					this.stateSignalName1 = value;
					this.StateSignalName1Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName1);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName2
		{
			get
			{
				return this.stateSignalName2;
			}
			set
			{
				if (this.stateSignalName2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName2, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName2);
					this.stateSignalName2 = value;
					this.StateSignalName2Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName2);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName3
		{
			get
			{
				return this.stateSignalName3;
			}
			set
			{
				if (this.stateSignalName3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName3, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName3);
					this.stateSignalName3 = value;
					this.StateSignalName3Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName3);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName4
		{
			get
			{
				return this.stateSignalName4;
			}
			set
			{
				if (this.stateSignalName4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName4, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName4);
					this.stateSignalName4 = value;
					this.StateSignalName4Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName4);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName5
		{
			get
			{
				return this.stateSignalName5;
			}
			set
			{
				if (this.stateSignalName5 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName5, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName5);
					this.stateSignalName5 = value;
					this.StateSignalName5Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName5);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName6
		{
			get
			{
				return this.stateSignalName6;
			}
			set
			{
				if (this.stateSignalName6 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName6, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName6);
					this.stateSignalName6 = value;
					this.StateSignalName6Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName6);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName7
		{
			get
			{
				return this.stateSignalName7;
			}
			set
			{
				if (this.stateSignalName7 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName7, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName7);
					this.stateSignalName7 = value;
					this.StateSignalName7Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName7);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string StateSignalName8
		{
			get
			{
				return this.stateSignalName8;
			}
			set
			{
				if (this.stateSignalName8 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateSignalName8, value);
					base.RaisePropertyChanging<string>(() => this.StateSignalName8);
					this.stateSignalName8 = value;
					this.StateSignalName8Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateSignalName8);
				}
			}
		}
		public HMIIndicatorBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.conditionSignalsMonitor = container.Resolve<ICollectionMonitor<ConditionSignal>>();
			this.conditionSignalsMonitor.Start(this.ConditionSignals);
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIIndicators=\"clr-namespace:HMI.RuntimeControls.Indicators;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIIndicator;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ObjectID", this.ObjectID);
			base.AddXamlAttribute("SignalPrefix", base.GetSignalPrefix());
			base.AddXamlAttribute("SecurityAuthorizationName", this.SecurityAuthorizationName);
			base.AddXamlAttribute("SecurityDenyAccessBehaviour", this.SecurityDenyAccessBehaviour);
			base.AddXamlAttribute("VisibilityMask", this.VisibilityMask);
			base.AddXamlAttribute("VisibilitySignalName", this.VisibilitySignalName);
			base.AddXamlAttribute("BinaryMask", this.BinaryMask);
			base.AddXamlAttribute("ConditionRule1", this.ConditionRule1);
			base.AddXamlAttribute("ConditionRule2", this.ConditionRule2);
			base.AddXamlAttribute("ConditionRule3", this.ConditionRule3);
			base.AddXamlAttribute("ConditionRule4", this.ConditionRule4);
			base.AddXamlAttribute("ConditionRule5", this.ConditionRule5);
			base.AddXamlAttribute("ConditionRule6", this.ConditionRule6);
			base.AddXamlAttribute("ConditionRule7", this.ConditionRule7);
			base.AddXamlAttribute("ConditionRule8", this.ConditionRule8);
			base.AddXamlAttribute("MaskSignal1", this.MaskSignal1);
			base.AddXamlAttribute("MaskSignal2", this.MaskSignal2);
			base.AddXamlAttribute("MaskSignal3", this.MaskSignal3);
			base.AddXamlAttribute("MaskSignal4", this.MaskSignal4);
			base.AddXamlAttribute("MaskSignal5", this.MaskSignal5);
			base.AddXamlAttribute("MaskSignal6", this.MaskSignal6);
			base.AddXamlAttribute("MaskSignal7", this.MaskSignal7);
			base.AddXamlAttribute("MaskSignal8", this.MaskSignal8);
			base.AddXamlAttribute("StateSignalName1", this.StateSignalName1);
			base.AddXamlAttribute("StateSignalName2", this.StateSignalName2);
			base.AddXamlAttribute("StateSignalName3", this.StateSignalName3);
			base.AddXamlAttribute("StateSignalName4", this.StateSignalName4);
			base.AddXamlAttribute("StateSignalName5", this.StateSignalName5);
			base.AddXamlAttribute("StateSignalName6", this.StateSignalName6);
			base.AddXamlAttribute("StateSignalName7", this.StateSignalName7);
			base.AddXamlAttribute("StateSignalName8", this.StateSignalName8);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "ConditionSignals", this.ConditionSignals, true);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
			base.IsTemplateEnabled = true;
		}
	}
}
