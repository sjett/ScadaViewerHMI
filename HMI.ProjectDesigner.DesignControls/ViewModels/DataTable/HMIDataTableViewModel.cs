// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.ViewModels.DataTable
{
	public class HMIDataTableViewModel : CommonControlViewModel
	{
		private const int SampleTableRowCount = 20;
		private static readonly string[] SampleStringSignals = new string[]
		{
			"",
			"ON",
			"OFF"
		};
		private int pageSize = 100;
		private bool showPager;
		private string initialConfiguration;
		private string serverName = string.Empty;
		private string signals = string.Empty;
		private int rangeHours = 24;
		private int archiveBit;
		private bool clientSideConfiguration;
		private readonly System.Collections.ObjectModel.ObservableCollection<TableLine> tableLines = new System.Collections.ObjectModel.ObservableCollection<TableLine>();
		private readonly System.Collections.ObjectModel.ObservableCollection<DataTableButtonSettings> buttonSettings = new System.Collections.ObjectModel.ObservableCollection<DataTableButtonSettings>();
		public event System.EventHandler<ValueChangedEventArgs<int>> PageSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ShowPagerChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> InitialConfigurationChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ServerNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalsChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> RangeHoursChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> ArchiveBitChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ClientSideConfigurationChanged;
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(600.0, 400.0);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MinimumValue(1.0)]
		public int PageSize
		{
			get
			{
				return this.pageSize;
			}
			set
			{
				if (this.pageSize != value)
				{
					base.RaisePropertyChanging<int>(() => this.PageSize);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.pageSize, value);
					this.pageSize = value;
					this.PageSizeChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.PageSize);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowPager
		{
			get
			{
				return this.showPager;
			}
			set
			{
				if (this.showPager != value)
				{
					base.RaisePropertyChanging<bool>(() => this.ShowPager);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.showPager, value);
					this.showPager = value;
					this.ShowPagerChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ShowPager);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
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
					base.RaisePropertyChanging<string>(() => this.InitialConfiguration);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.initialConfiguration, value);
					this.initialConfiguration = value;
					this.InitialConfigurationChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.InitialConfiguration);
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string ServerName
		{
			get => serverName;
			set
			{
				if (serverName != value)
				{
					ValueChangedEventArgs<string> e = new(serverName, value);
					base.RaisePropertyChanging<string>(() => ServerName);
					serverName = value;
					ServerNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => ServerName);
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string Signals
		{
			get => signals;
			set
			{
				if (signals != value)
				{
					ValueChangedEventArgs<string> e = new(signals, value);
					base.RaisePropertyChanging<string>(() => Signals);
					signals = value;
					SignalsChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => Signals);
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue(1.0)]
		public int RangeHours
		{
			get => rangeHours;
			set
			{
				if (rangeHours != value)
				{
					ValueChangedEventArgs<int> e = new(rangeHours, value);
					base.RaisePropertyChanging<int>(() => RangeHours);
					rangeHours = value;
					RangeHoursChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => RangeHours);
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue(0.0), MaximumValue(255.0)]
		public int ArchiveBit
		{
			get => archiveBit;
			set
			{
				if (archiveBit != value)
				{
					ValueChangedEventArgs<int> e = new(archiveBit, value);
					base.RaisePropertyChanging<int>(() => ArchiveBit);
					archiveBit = value;
					ArchiveBitChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => ArchiveBit);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ClientSideConfiguration
		{
			get
			{
				return this.clientSideConfiguration;
			}
			set
			{
				if (this.clientSideConfiguration != value)
				{
					base.RaisePropertyChanging<bool>(() => this.ClientSideConfiguration);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.clientSideConfiguration, value);
					this.clientSideConfiguration = value;
					this.ClientSideConfigurationChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ClientSideConfiguration);
				}
			}
		}
		[Browsable(false)]
		public System.Collections.ObjectModel.ObservableCollection<TableLine> TableLines
		{
			get
			{
				return this.tableLines;
			}
		}
		[CollectionEditor(typeof(DataTableButtonSettings), IsCollectionReadOnly = true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public System.Collections.ObjectModel.ObservableCollection<DataTableButtonSettings> ButtonSettings
		{
			get
			{
				return this.buttonSettings;
			}
		}
		public HMIDataTableViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.FillTableLinesSampleData();
			this.FillButtonSettings();
		}
		private void FillButtonSettings()
		{
			this.ButtonSettings.Clear();
			this.ButtonSettings.Add(new DataTableButtonSettings(base.Services, this.Parent, DataTableButtons.TimeSelection));
			this.ButtonSettings.Add(new DataTableButtonSettings(base.Services, this.Parent, DataTableButtons.Settings));
			this.ButtonSettings.Add(new DataTableButtonSettings(base.Services, this.Parent, DataTableButtons.Save));
			this.ButtonSettings.Add(new DataTableButtonSettings(base.Services, this.Parent, DataTableButtons.Print));
			this.ButtonSettings.Add(new DataTableButtonSettings(base.Services, this.Parent, DataTableButtons.Load));
			this.ButtonSettings.Add(new DataTableButtonSettings(base.Services, this.Parent, DataTableButtons.Export));
		}
		private void FillTableLinesSampleData()
		{
			System.DateTime now = System.DateTime.Now;
			System.Random random = new System.Random();
			for (int i = 0; i < 20; i++)
			{
				TableLine item = new TableLine
				{
					Index = i + 1,
					Time = now.AddMilliseconds((double)(i * 135)),
					Signal1 = random.Next(-1000, 1000),
					Signal2 = (random.NextDouble() - 0.5) * 1000.0,
					Signal3 = HMIDataTableViewModel.SampleStringSignals[random.Next(0, HMIDataTableViewModel.SampleStringSignals.Length)]
				};
				this.TableLines.Add(item);
			}
		}
		public override string GetXamlControlName()
		{
			return "HMIDataTable";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIDataTable=\"clr-namespace:HMI.RuntimeControls.DataTable.Views;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIDataTable;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("PageSize", this.PageSize);
			base.AddXamlAttribute("ShowPager", this.ShowPager);
			base.AddXamlAttribute("InitialConfiguration", this.InitialConfiguration);
			base.AddXamlAttribute("ServerName", this.ServerName);
			base.AddXamlAttribute("ClientSideConfiguration", this.ClientSideConfiguration);
			base.AddXamlAttribute("Signals", this.Signals);
			base.AddXamlAttribute("RangeHours", this.RangeHours);
			base.AddXamlAttribute("ArchiveBit", this.ArchiveBit);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			this.AddButtonsConfiguration(collectionXaml);
		}
		private void AddButtonsConfiguration(System.Text.StringBuilder collectionXaml)
		{
			string namespacePrefix = base.GetNamespacePrefix();
			string xamlControlName = this.GetXamlControlName();
			base.AddCollectionOpenElement(collectionXaml, namespacePrefix, xamlControlName, "ButtonsConfiguration");
			base.AddXamlCollection(collectionXaml, "HMIDataTableButtons", this.ButtonSettings, false);
			base.AddCollectionCloseElement(collectionXaml, namespacePrefix, xamlControlName, "ButtonsConfiguration");
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
