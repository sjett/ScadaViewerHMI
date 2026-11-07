// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMITextDisplay1ViewModel : HMIIOControlBaseViewModel
	{
		private readonly ICollectionMonitor<ConditionSignal> conditionSignalsMonitor;
		private bool binaryMask;
		private string conditionRule1;
		private string conditionRule2;
		private string conditionRule3;
		private string conditionRule4;
		private readonly IObservableCollection<ConditionSignal> conditionSignals = new FullyObservableCollection<ConditionSignal>();
		private int? maskSignal1;
		private int? maskSignal2;
		private int? maskSignal3;
		private int? maskSignal4;
		private string limitSignalName1;
		private string limitSignalName2;
		private string limitSignalName3;
		private string limitSignalName4;
		public event System.EventHandler<ValueChangedEventArgs<bool>> BinaryMaskChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule1Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule2Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule3Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConditionRule4Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal1Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal2Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MaskSignal3Changed;
		public event System.EventHandler<ValueChangedEventArgs<int?>> MarkSignal4Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> LimitSignalName1Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> LimitSignalName2Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> LimitSignalName3Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> LimitSignalName4Changed;
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
		[Browsable(true), CollectionEditor(typeof(ConditionSignal)), PropertyCategory("States", typeof(StandardPropertyCategories))]
		public IObservableCollection<ConditionSignal> ConditionSignals
		{
			get
			{
				return this.conditionSignals;
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
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
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
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
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
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
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories))]
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
					this.MarkSignal4Changed.Raise(this, e);
					base.RaisePropertyChanged<int?>(() => this.MaskSignal4);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string LimitSignalName1
		{
			get
			{
				return this.limitSignalName1;
			}
			set
			{
				if (this.limitSignalName1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.limitSignalName1, value);
					base.RaisePropertyChanging<string>(() => this.LimitSignalName1);
					this.limitSignalName1 = value;
					this.LimitSignalName1Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.LimitSignalName1);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string LimitSignalName2
		{
			get
			{
				return this.limitSignalName2;
			}
			set
			{
				if (this.limitSignalName2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.limitSignalName2, value);
					base.RaisePropertyChanging<string>(() => this.LimitSignalName2);
					this.limitSignalName2 = value;
					this.LimitSignalName2Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.LimitSignalName2);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string LimitSignalName3
		{
			get
			{
				return this.limitSignalName3;
			}
			set
			{
				if (this.limitSignalName3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.limitSignalName3, value);
					base.RaisePropertyChanging<string>(() => this.LimitSignalName3);
					this.limitSignalName3 = value;
					this.LimitSignalName3Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.LimitSignalName3);
				}
			}
		}
		[Browsable(true), PropertyCategory("States", typeof(StandardPropertyCategories)), SignalBrowser]
		public string LimitSignalName4
		{
			get
			{
				return this.limitSignalName4;
			}
			set
			{
				if (this.limitSignalName4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.limitSignalName4, value);
					base.RaisePropertyChanging<string>(() => this.LimitSignalName4);
					this.limitSignalName4 = value;
					this.LimitSignalName4Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.LimitSignalName4);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(100.0, 25.0);
			}
		}
		public HMITextDisplay1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.conditionSignalsMonitor = container.Resolve<ICollectionMonitor<ConditionSignal>>();
			this.conditionSignalsMonitor.Start(this.ConditionSignals);
		}
		public override string GetXamlControlName()
		{
			return "HMITextDisplay1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("BinaryMask", this.BinaryMask);
			base.AddXamlAttribute("ConditionRule1", this.ConditionRule1);
			base.AddXamlAttribute("ConditionRule2", this.ConditionRule2);
			base.AddXamlAttribute("ConditionRule3", this.ConditionRule3);
			base.AddXamlAttribute("ConditionRule4", this.ConditionRule4);
			base.AddXamlAttribute("MaskSignal1", this.MaskSignal1);
			base.AddXamlAttribute("MaskSignal2", this.MaskSignal2);
			base.AddXamlAttribute("MaskSignal3", this.MaskSignal3);
			base.AddXamlAttribute("MaskSignal4", this.MaskSignal4);
			base.AddXamlAttribute("LimitSignalName1", this.LimitSignalName1);
			base.AddXamlAttribute("LimitSignalName2", this.LimitSignalName2);
			base.AddXamlAttribute("LimitSignalName3", this.LimitSignalName3);
			base.AddXamlAttribute("LimitSignalName4", this.LimitSignalName4);
			base.AddXamlAttribute("Foreground", base.Foreground);
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
			base.IsTemplateEnabled = true;
		}
	}
}
