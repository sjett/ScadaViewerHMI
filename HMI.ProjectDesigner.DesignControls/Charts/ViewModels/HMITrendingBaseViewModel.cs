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
namespace HMI.ProjectDesigner.DesignControls.Charts.ViewModels
{
	public class HMITrendingBaseViewModel : CommonControlViewModel
	{
		private bool clientSideConfiguration;
		private string configurationNamespace;
		private string initialConfiguration;
		private string serverName = string.Empty;
		private string signals = string.Empty;
		private int rangeHours = 24;
		private int archiveBit;
		private HMITrendingBase.TrendingTimeZone timeZone;
		private DialogModality popupModality;

		public event System.EventHandler<ValueChangedEventArgs<bool>> ClientSideConfigurationChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ConfigurationNamespaceChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> InitialConfigurationChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ServerNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> SignalsChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> RangeHoursChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> ArchiveBitChanged;
		public event System.EventHandler<ValueChangedEventArgs<HMITrendingBase.TrendingTimeZone>> TimeZoneChanged;
		public event System.EventHandler<ValueChangedEventArgs<DialogModality>> PopupModalityChanged;

		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
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
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.clientSideConfiguration, value);
					base.RaisePropertyChanging<bool>(() => this.ClientSideConfiguration);
					this.clientSideConfiguration = value;
					this.ClientSideConfigurationChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ClientSideConfiguration);
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
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
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
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
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
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue(1.0)]
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
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MinimumValue(0.0), MaximumValue(255.0)]
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
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public HMITrendingBase.TrendingTimeZone TimeZone
		{
			get
			{
				return this.timeZone;
			}
			set
			{
				if (this.timeZone != value)
				{
					ValueChangedEventArgs<HMITrendingBase.TrendingTimeZone> e = new ValueChangedEventArgs<HMITrendingBase.TrendingTimeZone>(this.timeZone, value);
					base.RaisePropertyChanging<HMITrendingBase.TrendingTimeZone>(() => this.TimeZone);
					this.timeZone = value;
					this.TimeZoneChanged.Raise(this, e);
					base.RaisePropertyChanged<HMITrendingBase.TrendingTimeZone>(() => this.TimeZone);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public DialogModality PopupModality
		{
			get
			{
				return this.popupModality;
			}
			set
			{
				if (this.popupModality != value)
				{
					ValueChangedEventArgs<DialogModality> e = new ValueChangedEventArgs<DialogModality>(this.popupModality, value);
					base.RaisePropertyChanging<DialogModality>(() => this.PopupModality);
					this.popupModality = value;
					this.PopupModalityChanged.Raise(this, e);
					base.RaisePropertyChanged<DialogModality>(() => this.PopupModality);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(430.0, 250.0);
			}
		}
		public HMITrendingBaseViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMICharts=\"clr-namespace:HMI.RuntimeControls.Charts;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMICharts;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ClientsideConfiguration", this.ClientSideConfiguration);
			base.AddXamlAttribute("ConfigurationNamespace", this.ConfigurationNamespace);
			base.AddXamlAttribute("InitialConfiguration", this.InitialConfiguration);
			base.AddXamlAttribute("ServerName", this.ServerName);
			base.AddXamlAttribute("Signals", this.Signals);
			base.AddXamlAttribute("RangeHours", this.RangeHours);
			base.AddXamlAttribute("ArchiveBit", this.ArchiveBit);
			base.AddXamlAttribute("PopupModality", this.PopupModality);
			base.AddXamlAttribute("TimeZone", this.TimeZone);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
