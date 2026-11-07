// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.OperationDiary.ViewModels
{
	public class HMIOperationDiaryViewModel : CommonControlViewModel
	{
		private readonly ICollectionMonitor<AlarmTypeDisplay> alarmTypeDisplayMonitor;
		private readonly ICollectionMonitor<ColumnSettings> columnSettingsMonitor;
		private readonly IObservableCollection<AlarmTypeDisplay> alarmTypesInformation = new FullyObservableCollection<AlarmTypeDisplay>();
		private bool clientsideConfiguration;
		private string configurationNamespace;
		private bool groupByDate;
		private string initialConfiguration;
		private int maxEvents = 10;
		private int refreshInterval = 5;
		private readonly IObservableCollection<ColumnSettings> columns = new FullyObservableCollection<ColumnSettings>();

		public event System.EventHandler<ValueChangedEventArgs<bool>> ClientsideConfigurationChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConfigurationNamespaceChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> GroupByDateChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> InitialConfigurationChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> MaxEventsChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> RefreshIntervalChanged;

		[Browsable(true), CollectionEditor(typeof(AlarmTypeDisplay)), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public IObservableCollection<AlarmTypeDisplay> AlarmTypesInformation
		{
			get
			{
				return this.alarmTypesInformation;
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool ClientsideConfiguration
		{
			get
			{
				return this.clientsideConfiguration;
			}
			set
			{
				if (this.clientsideConfiguration != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.clientsideConfiguration, value);
					base.RaisePropertyChanging<bool>(() => this.ClientsideConfiguration);
					this.clientsideConfiguration = value;
					this.ClientsideConfigurationChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ClientsideConfiguration);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string ConfigurationNamespace
		{
			get
			{
				return this.configurationNamespace;
			}
			set
			{
				if (this.configurationNamespace != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.configurationNamespace, value);
					base.RaisePropertyChanging<string>(() => this.ConfigurationNamespace);
					this.configurationNamespace = value;
					this.ConfigurationNamespaceChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ConfigurationNamespace);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool GroupByDate
		{
			get
			{
				return this.groupByDate;
			}
			set
			{
				if (this.groupByDate != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.groupByDate, value);
					base.RaisePropertyChanging<bool>(() => this.GroupByDate);
					this.groupByDate = value;
					this.GroupByDateChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.GroupByDate);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string InitialConfiguration
		{
			get
			{
				return this.initialConfiguration;
			}
			set
			{
				if (this.initialConfiguration != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.initialConfiguration, value);
					base.RaisePropertyChanging<string>(() => this.InitialConfiguration);
					this.initialConfiguration = value;
					this.InitialConfigurationChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.InitialConfiguration);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(10000.0), MinimumValue(1.0)]
		public int MaxEvents
		{
			get
			{
				return this.maxEvents;
			}
			set
			{
				if (this.maxEvents != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.maxEvents, value);
					base.RaisePropertyChanging<int>(() => this.MaxEvents);
					this.maxEvents = value;
					this.MaxEventsChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.MaxEvents);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(1000.0), MinimumValue(1.0)]
		public int RefreshInterval
		{
			get
			{
				return this.refreshInterval;
			}
			set
			{
				if (this.refreshInterval != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.refreshInterval, value);
					base.RaisePropertyChanging<int>(() => this.RefreshInterval);
					this.refreshInterval = value;
					this.RefreshIntervalChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.RefreshInterval);
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(ColumnSettings)), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public IObservableCollection<ColumnSettings> Columns
		{
			get
			{
				return this.columns;
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(400.0, 400.0);
			}
		}
		public HMIOperationDiaryViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.alarmTypeDisplayMonitor = container.Resolve<ICollectionMonitor<AlarmTypeDisplay>>();
			this.alarmTypeDisplayMonitor.Start(this.AlarmTypesInformation);
			this.columnSettingsMonitor = container.Resolve<ICollectionMonitor<ColumnSettings>>();
			this.columnSettingsMonitor.Start(this.Columns);
		}
		public override string GetXamlControlName()
		{
			return "HMIOperationDiary";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIOperationDiary=\"clr-namespace:HMI.RuntimeControls.OperationDiary;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIOperationDiary;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ClientsideConfiguration", this.ClientsideConfiguration);
			base.AddXamlAttribute("ConfigurationNamespace", this.ConfigurationNamespace);
			base.AddXamlAttribute("InitialConfiguration", this.InitialConfiguration);
			base.AddXamlAttribute("MaxEvents", this.MaxEvents);
			base.AddXamlAttribute("RefreshInterval", this.RefreshInterval);
			base.AddXamlAttribute("GroupByDate", this.GroupByDate);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "Columns", this.Columns, true);
			base.AddXamlCollection(collectionXaml, "AlarmTypesInformation", this.AlarmTypesInformation, true);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
