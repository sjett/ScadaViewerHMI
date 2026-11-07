// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using HMI.ProjectDesigner.DesignControls.Alarms.AlarmStateManager;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
namespace HMI.ProjectDesigner.DesignControls.Alarms.ViewModels
{
	public class HMIAlarmStateManagerViewModel : CommonControlViewModel
	{
		private const string ColumnConditionName = "报警条件";
		private const string ColumnGroup = "报警组";
		private const string ColumnOPCItemName = "OPC点名称";
		private const string ColumnReactivationDate = "重激活时间";
		private const string ColumnSignalDescription = "点位描述";
		private const string ColumnSignalName = "点位名称";
		private const string ColumnStatus = "报警状态";
		private const string ColumnTag = "设备地址";
		private const string ColumnText = "报警描述";
		private const string ColumnType = "报警类型";
		private readonly System.Collections.ObjectModel.ObservableCollection<int> orderList = new System.Collections.ObjectModel.ObservableCollection<int>(new int[]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10
		});
		private System.Collections.ObjectModel.ObservableCollection<HMIAlarm> alarms = new System.Collections.ObjectModel.ObservableCollection<HMIAlarm>();
		private System.Collections.ObjectModel.ObservableCollection<HMIServer> servers = new System.Collections.ObjectModel.ObservableCollection<HMIServer>();
		private bool useColumnWidthProperties;
		private double conditionNameWidth = 150.0;
		private double groupWidth = 150.0;
		private double oPCItemNameWidth = 150.0;
		private double reactivationDateWidth = 150.0;
		private double signalDescriptionWidth = 150.0;
		private double signalNameWidth = 150.0;
		private double statusWidth = 150.0;
		private double tagWidth = 150.0;
		private double textWidth = 150.0;
		private double typeWidth = 150.0;
		private bool useColumnOrderProperties;
		private int conditionNameOrder = 5;
		private int groupOrder = 8;
		private int oPCItemNameOrder = 2;
		private int reactivationDateOrder = 10;
		private int signalDescriptionOrder = 3;
		private int signalNameOrder = 1;
		private int statusOrder = 9;
		private int tagOrder = 4;
		private int textOrder = 6;
		private int typeOrder = 7;
		private bool useColumnVisibilityProperties;

		private SilverlightVisibility conditionNameVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility groupVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility oPCItemNameVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility reactivationDateVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility signalDescriptionVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility signalNameVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility statusVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility tagVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility textVisibility = SilverlightVisibility.Visible;
		private SilverlightVisibility typeVisibility = SilverlightVisibility.Visible;

		public event System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, System.Windows.Visibility>>> ColumnVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>>> ColumnWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>>> ColumnOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<HMIAlarm>>> AlarmsChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<HMIServer>>> ServersChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseColumnWidthPropertiesChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> ConditionNameWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> GroupWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> OPCItemNameWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> ReactivationDateWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> SignalDescriptionWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> SignalNameWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> StatusWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> TagWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> TextWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> TypeWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseColumnOrderPropertiesChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> ConditionNameOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> GroupOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> OPCItemNameOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> ReactivationDateOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> SignalDescriptionOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> SignalNameOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> StatusOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> TagOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> TextOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> TypeOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseColumnVisibilityPropertiesChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> ConditionNameVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> GroupVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> OPCItemNameVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> ReactivationDateVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> SignalDescriptionVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> SignalNameVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> StatusVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> TagVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> TextVisibilityChanged;
		public event System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>> TypeVisibilityChanged;
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(600.0, 400.0);
			}
		}
		[Browsable(false)]
		public System.Collections.ObjectModel.ObservableCollection<int> OrderList
		{
			get
			{
				return this.orderList;
			}
		}
		[Browsable(false)]
		public System.Collections.ObjectModel.ObservableCollection<HMIAlarm> Alarms
		{
			get
			{
				return this.alarms;
			}
			set
			{
				if (this.alarms != value)
				{
					ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<HMIAlarm>> e = new ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<HMIAlarm>>(this.alarms, value);
					this.alarms = value;
					this.AlarmsChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Collections.ObjectModel.ObservableCollection<HMIAlarm>>(() => this.Alarms);
				}
			}
		}
		[Browsable(false)]
		public System.Collections.ObjectModel.ObservableCollection<HMIServer> Servers
		{
			get
			{
				return this.servers;
			}
			set
			{
				if (this.servers != value)
				{
					ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<HMIServer>> e = new ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<HMIServer>>(this.servers, value);
					this.servers = value;
					this.ServersChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Collections.ObjectModel.ObservableCollection<HMIServer>>(() => this.Servers);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories))]
		public bool UseColumnWidthProperties
		{
			get
			{
				return this.useColumnWidthProperties;
			}
			set
			{
				if (this.useColumnWidthProperties != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.useColumnWidthProperties, value);
					this.useColumnWidthProperties = value;
					base.RaisePropertyChanging<bool>(() => this.UseColumnWidthProperties);
					this.UseColumnWidthPropertiesChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.UseColumnWidthProperties);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double ConditionNameWidth
		{
			get
			{
				return this.conditionNameWidth;
			}
			set
			{
				if (this.conditionNameWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.conditionNameWidth, value);
					base.RaisePropertyChanging<double>(() => this.ConditionNameWidth);
					this.conditionNameWidth = value;
					this.ConditionNameWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.ConditionNameWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double GroupWidth
		{
			get
			{
				return this.groupWidth;
			}
			set
			{
				if (this.groupWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.groupWidth, value);
					this.groupWidth = value;
					base.RaisePropertyChanging<double>(() => this.GroupWidth);
					this.GroupWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.GroupWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double OPCItemNameWidth
		{
			get
			{
				return this.oPCItemNameWidth;
			}
			set
			{
				if (this.oPCItemNameWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.oPCItemNameWidth, value);
					this.oPCItemNameWidth = value;
					base.RaisePropertyChanging<double>(() => this.OPCItemNameWidth);
					this.OPCItemNameWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.OPCItemNameWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double ReactivationDateWidth
		{
			get
			{
				return this.reactivationDateWidth;
			}
			set
			{
				if (this.reactivationDateWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.reactivationDateWidth, value);
					base.RaisePropertyChanging<double>(() => this.ReactivationDateWidth);
					this.reactivationDateWidth = value;
					this.ReactivationDateWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.ReactivationDateWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double SignalDescriptionWidth
		{
			get
			{
				return this.signalDescriptionWidth;
			}
			set
			{
				if (this.signalDescriptionWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.signalDescriptionWidth, value);
					base.RaisePropertyChanging<double>(() => this.SignalDescriptionWidth);
					this.signalDescriptionWidth = value;
					this.SignalDescriptionWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.SignalDescriptionWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double SignalNameWidth
		{
			get
			{
				return this.signalNameWidth;
			}
			set
			{
				if (this.signalNameWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.signalNameWidth, value);
					base.RaisePropertyChanging<double>(() => this.SignalNameWidth);
					this.signalNameWidth = value;
					this.SignalNameWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.SignalNameWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double StatusWidth
		{
			get
			{
				return this.statusWidth;
			}
			set
			{
				if (this.statusWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.statusWidth, value);
					base.RaisePropertyChanging<double>(() => this.StatusWidth);
					this.statusWidth = value;
					this.StatusWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.StatusWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double TagWidth
		{
			get
			{
				return this.tagWidth;
			}
			set
			{
				if (this.tagWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.tagWidth, value);
					base.RaisePropertyChanging<double>(() => this.TagWidth);
					this.tagWidth = value;
					this.TagWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.TagWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double TextWidth
		{
			get
			{
				return this.textWidth;
			}
			set
			{
				if (this.textWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.textWidth, value);
					base.RaisePropertyChanging<double>(() => this.TextWidth);
					this.textWidth = value;
					this.TextWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.TextWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double TypeWidth
		{
			get
			{
				return this.typeWidth;
			}
			set
			{
				if (this.typeWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.typeWidth, value);
					base.RaisePropertyChanging<double>(() => this.TypeWidth);
					this.typeWidth = value;
					this.TypeWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.TypeWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public bool UseColumnOrderProperties
		{
			get
			{
				return this.useColumnOrderProperties;
			}
			set
			{
				if (this.useColumnOrderProperties != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.useColumnOrderProperties, value);
					base.RaisePropertyChanging<bool>(() => this.UseColumnOrderProperties);
					this.useColumnOrderProperties = value;
					this.UseColumnOrderPropertiesChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.UseColumnOrderProperties);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int ConditionNameOrder
		{
			get
			{
				return this.conditionNameOrder;
			}
			set
			{
				if (this.conditionNameOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.conditionNameOrder, value);
					base.RaisePropertyChanging<int>(() => this.ConditionNameOrder);
					this.conditionNameOrder = value;
					this.ConditionNameOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.ConditionNameOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int GroupOrder
		{
			get
			{
				return this.groupOrder;
			}
			set
			{
				if (this.groupOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.groupOrder, value);
					base.RaisePropertyChanging<int>(() => this.GroupOrder);
					this.groupOrder = value;
					this.GroupOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.GroupOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int OPCItemNameOrder
		{
			get
			{
				return this.oPCItemNameOrder;
			}
			set
			{
				if (this.oPCItemNameOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.oPCItemNameOrder, value);
					base.RaisePropertyChanging<int>(() => this.OPCItemNameOrder);
					this.oPCItemNameOrder = value;
					this.OPCItemNameOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.OPCItemNameOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int ReactivationDateOrder
		{
			get
			{
				return this.reactivationDateOrder;
			}
			set
			{
				if (this.reactivationDateOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.reactivationDateOrder, value);
					base.RaisePropertyChanging<int>(() => this.ReactivationDateOrder);
					this.reactivationDateOrder = value;
					this.ReactivationDateOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.ReactivationDateOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int SignalDescriptionOrder
		{
			get
			{
				return this.signalDescriptionOrder;
			}
			set
			{
				if (this.signalDescriptionOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.signalDescriptionOrder, value);
					base.RaisePropertyChanging<int>(() => this.SignalDescriptionOrder);
					this.signalDescriptionOrder = value;
					this.SignalDescriptionOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.SignalDescriptionOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int SignalNameOrder
		{
			get
			{
				return this.signalNameOrder;
			}
			set
			{
				if (this.signalNameOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.signalNameOrder, value);
					base.RaisePropertyChanging<int>(() => this.SignalNameOrder);
					this.signalNameOrder = value;
					this.SignalNameOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.SignalNameOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int StatusOrder
		{
			get
			{
				return this.statusOrder;
			}
			set
			{
				if (this.statusOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.statusOrder, value);
					base.RaisePropertyChanging<int>(() => this.StatusOrder);
					this.statusOrder = value;
					this.StatusOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.StatusOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int TagOrder
		{
			get
			{
				return this.tagOrder;
			}
			set
			{
				if (this.tagOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.tagOrder, value);
					base.RaisePropertyChanging<int>(() => this.TagOrder);
					this.tagOrder = value;
					this.TagOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.TagOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int TextOrder
		{
			get
			{
				return this.textOrder;
			}
			set
			{
				if (this.textOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.textOrder, value);
					base.RaisePropertyChanging<int>(() => this.TextOrder);
					this.textOrder = value;
					this.TextOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.TextOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int TypeOrder
		{
			get
			{
				return this.typeOrder;
			}
			set
			{
				if (this.typeOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.typeOrder, value);
					base.RaisePropertyChanging<int>(() => this.TypeOrder);
					this.typeOrder = value;
					this.TypeOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.TypeOrder);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public bool UseColumnVisibilityProperties
		{
			get
			{
				return this.useColumnVisibilityProperties;
			}
			set
			{
				if (this.useColumnVisibilityProperties != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.useColumnVisibilityProperties, value);
					base.RaisePropertyChanging<bool>(() => this.UseColumnVisibilityProperties);
					this.useColumnVisibilityProperties = value;
					this.UseColumnVisibilityPropertiesChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.UseColumnVisibilityProperties);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility ConditionNameVisibility
		{
			get
			{
				return this.conditionNameVisibility;
			}
			set
			{
				if (this.conditionNameVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.conditionNameVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.ConditionNameVisibility);
					this.conditionNameVisibility = value;
					this.ConditionNameVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.ConditionNameVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility GroupVisibility
		{
			get
			{
				return this.groupVisibility;
			}
			set
			{
				if (this.groupVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.groupVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.GroupVisibility);
					this.groupVisibility = value;
					this.GroupVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.GroupVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility OPCItemNameVisibility
		{
			get
			{
				return this.oPCItemNameVisibility;
			}
			set
			{
				if (this.oPCItemNameVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.oPCItemNameVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.OPCItemNameVisibility);
					this.oPCItemNameVisibility = value;
					this.OPCItemNameVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.OPCItemNameVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility ReactivationDateVisibility
		{
			get
			{
				return this.reactivationDateVisibility;
			}
			set
			{
				if (this.reactivationDateVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.reactivationDateVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.ReactivationDateVisibility);
					this.reactivationDateVisibility = value;
					this.ReactivationDateVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.ReactivationDateVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility SignalDescriptionVisibility
		{
			get
			{
				return this.signalDescriptionVisibility;
			}
			set
			{
				if (this.signalDescriptionVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.signalDescriptionVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.SignalDescriptionVisibility);
					this.signalDescriptionVisibility = value;
					this.SignalDescriptionVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.SignalDescriptionVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility SignalNameVisibility
		{
			get
			{
				return this.signalNameVisibility;
			}
			set
			{
				if (this.signalNameVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.signalNameVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.SignalNameVisibility);
					this.signalNameVisibility = value;
					this.SignalNameVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.SignalNameVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility StatusVisibility
		{
			get
			{
				return this.statusVisibility;
			}
			set
			{
				if (this.statusVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.statusVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.StatusVisibility);
					this.statusVisibility = value;
					this.StatusVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.StatusVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility TagVisibility
		{
			get
			{
				return this.tagVisibility;
			}
			set
			{
				if (this.tagVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.tagVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.TagVisibility);
					this.tagVisibility = value;
					this.TagVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.TagVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility TextVisibility
		{
			get
			{
				return this.textVisibility;
			}
			set
			{
				if (this.textVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.textVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.TextVisibility);
					this.textVisibility = value;
					this.TextVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.TextVisibility);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnVisibilities", typeof(StandardPropertyCategories))]
		public SilverlightVisibility TypeVisibility
		{
			get
			{
				return this.typeVisibility;
			}
			set
			{
				if (this.typeVisibility != value)
				{
					ValueChangedEventArgs<SilverlightVisibility> e = new ValueChangedEventArgs<SilverlightVisibility>(this.typeVisibility, value);
					base.RaisePropertyChanging<SilverlightVisibility>(() => this.TypeVisibility);
					this.typeVisibility = value;
					this.TypeVisibilityChanged.Raise(this, e);
					base.RaisePropertyChanged<SilverlightVisibility>(() => this.TypeVisibility);
				}
			}
		}
		public HMIAlarmStateManagerViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.InitializeAlarms();
			this.SubsribeToEvents();
		}
		~HMIAlarmStateManagerViewModel()
		{
			this.UnSubsribeToEvents();
		}
		public override string GetXamlControlName()
		{
			return "HMIAlarmStateManager";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIAlarms=\"clr-namespace:HMI.RuntimeControls.Alarms;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIAlarms;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("UseColumnOrderProperties", this.UseColumnOrderProperties);
			base.AddXamlAttribute("ConditionNameOrder", this.ConditionNameOrder);
			base.AddXamlAttribute("GroupOrder", this.GroupOrder);
			base.AddXamlAttribute("OPCItemNameOrder", this.OPCItemNameOrder);
			base.AddXamlAttribute("ReactivationDateOrder", this.ReactivationDateOrder);
			base.AddXamlAttribute("SignalDescriptionOrder", this.SignalDescriptionOrder);
			base.AddXamlAttribute("SignalNameOrder", this.SignalNameOrder);
			base.AddXamlAttribute("StatusOrder", this.StatusOrder);
			base.AddXamlAttribute("TagOrder", this.TagOrder);
			base.AddXamlAttribute("TextOrder", this.TextOrder);
			base.AddXamlAttribute("TypeOrder", this.TypeOrder);
			base.AddXamlAttribute("UseColumnWidthProperties", this.UseColumnWidthProperties);
			base.AddXamlAttribute("ConditionNameWidth", this.ConditionNameWidth);
			base.AddXamlAttribute("GroupWidth", this.GroupWidth);
			base.AddXamlAttribute("OPCItemNameWidth", this.OPCItemNameWidth);
			base.AddXamlAttribute("ReactivationDateWidth", this.ReactivationDateWidth);
			base.AddXamlAttribute("SignalDescriptionWidth", this.SignalDescriptionWidth);
			base.AddXamlAttribute("SignalNameWidth", this.SignalNameWidth);
			base.AddXamlAttribute("StatusWidth", this.StatusWidth);
			base.AddXamlAttribute("TagWidth", this.TagWidth);
			base.AddXamlAttribute("TextWidth", this.TextWidth);
			base.AddXamlAttribute("TypeWidth", this.TypeWidth);
			base.AddXamlAttribute("UseColumnVisibilityProperties", this.UseColumnVisibilityProperties);
			base.AddXamlAttribute("ConditionNameVisibility", this.ConditionNameVisibility);
			base.AddXamlAttribute("GroupVisibility", this.GroupVisibility);
			base.AddXamlAttribute("OPCItemNameVisibility", this.OPCItemNameVisibility);
			base.AddXamlAttribute("ReactivationDateVisibility", this.ReactivationDateVisibility);
			base.AddXamlAttribute("SignalDescriptionVisibility", this.SignalDescriptionVisibility);
			base.AddXamlAttribute("SignalNameVisibility", this.SignalNameVisibility);
			base.AddXamlAttribute("StatusVisibility", this.StatusVisibility);
			base.AddXamlAttribute("TagVisibility", this.TagVisibility);
			base.AddXamlAttribute("TextVisibility", this.TextVisibility);
			base.AddXamlAttribute("TypeVisibility", this.TypeVisibility);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
		public void InitializePropertyStates()
		{
			this.UseColumnOrderPropertiesChanged.Raise(this, null);
			this.UseColumnWidthPropertiesChanged.Raise(this, null);
			this.UseColumnVisibilityPropertiesChanged.Raise(this, null);
		}
		private void InitializeAlarms()
		{
			this.Alarms.Clear();
			this.Servers.Clear();
			HMIServer wFServer = new HMIServer(System.Guid.NewGuid(), ".")
			{
				IsExpanded = true
			};
			HMIAlarmGroup wFAlarmGroup = new HMIAlarmGroup(wFServer, System.Guid.NewGuid(), "报警组 1");
			HMIAlarm alarm = HMIAlarmStateManagerViewModel.GetAlarm(wFAlarmGroup, "1");
			alarm.Status = 1;
			wFAlarmGroup.Alarms.Add(alarm);
			this.Alarms.Add(alarm);
			wFServer.AlarmGroups.Add(wFAlarmGroup);
			wFAlarmGroup = new HMIAlarmGroup(wFServer, System.Guid.NewGuid(), "报警组 2");
			alarm = HMIAlarmStateManagerViewModel.GetAlarm(wFAlarmGroup, "2");
			alarm.ReactivationDate = System.DateTime.Now;
			alarm.ReactivationDateText = System.DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture);
			wFAlarmGroup.Alarms.Add(alarm);
			this.Alarms.Add(alarm);
			wFServer.AlarmGroups.Add(wFAlarmGroup);
			wFAlarmGroup = new HMIAlarmGroup(wFServer, System.Guid.NewGuid(), "报警组 3");
			alarm = HMIAlarmStateManagerViewModel.GetAlarm(wFAlarmGroup, "3");
			wFAlarmGroup.Alarms.Add(alarm);
			this.Alarms.Add(alarm);
			wFServer.AlarmGroups.Add(wFAlarmGroup);
			this.Servers.Add(wFServer);
		}
		private static HMIAlarm GetAlarm(HMIAlarmGroup wfAlarmGroup, string number)
		{
			return new HMIAlarm(wfAlarmGroup, System.Guid.NewGuid())
			{
				ConditionName = "Condition " + number,
				OPCItemName = "OPC " + number,
				ReactivationDate = System.DateTime.MinValue,
				ReactivationDateText = System.DateTime.MinValue.ToString(System.Globalization.CultureInfo.CurrentCulture),
				SignalDescription = "Description " + number,
				SignalName = "Signal " + number,
				Status = 0,
				StatusText = "Inactive" + number,
				Tag = "Tag " + number,
				Text = "Text " + number,
				Type = "Type " + number
			};
		}
		private System.Collections.Generic.Dictionary<string, int> GetColumnOrders()
		{
			return new System.Collections.Generic.Dictionary<string, int>
			{

				{
					ColumnConditionName,
					this.UseColumnOrderProperties ? this.ConditionNameOrder : 5
				},

				{
					ColumnGroup,
					this.UseColumnOrderProperties ? this.GroupOrder : 8
				},

				{
					ColumnOPCItemName,
					this.UseColumnOrderProperties ? this.OPCItemNameOrder : 2
				},

				{
					ColumnReactivationDate,
					this.UseColumnOrderProperties ? this.ReactivationDateOrder : 10
				},

				{
					ColumnSignalDescription,
					this.UseColumnOrderProperties ? this.SignalDescriptionOrder : 3
				},

				{
					ColumnSignalName,
					this.UseColumnOrderProperties ? this.SignalNameOrder : 1
				},

				{
					ColumnStatus,
					this.UseColumnOrderProperties ? this.StatusOrder : 9
				},

				{
					ColumnTag,
					this.UseColumnOrderProperties ? this.TagOrder : 4
				},

				{
					ColumnText,
					this.UseColumnOrderProperties ? this.TextOrder : 6
				},

				{
					ColumnType,
					this.UseColumnOrderProperties ? this.TypeOrder : 7
				}
			};
		}
		private System.Collections.Generic.Dictionary<string, DataGridLength> GetColumnWidths()
		{
			return new System.Collections.Generic.Dictionary<string, DataGridLength>
			{

				{
					ColumnConditionName,
					this.UseColumnWidthProperties ? this.ConditionNameWidth : DataGridLength.Auto
				},

				{
					ColumnGroup,
					this.UseColumnWidthProperties ? this.GroupWidth : DataGridLength.Auto
				},

				{
					ColumnOPCItemName,
					this.UseColumnWidthProperties ? this.OPCItemNameWidth : DataGridLength.Auto
				},

				{
					ColumnReactivationDate,
					this.UseColumnWidthProperties ? this.ReactivationDateWidth : DataGridLength.Auto
				},

				{
					ColumnSignalDescription,
					this.UseColumnWidthProperties ? this.SignalDescriptionWidth : DataGridLength.Auto
				},

				{
					ColumnSignalName,
					this.UseColumnWidthProperties ? this.SignalNameWidth : DataGridLength.Auto
				},

				{
					ColumnStatus,
					this.UseColumnWidthProperties ? this.StatusWidth : DataGridLength.Auto
				},

				{
					ColumnTag,
					this.UseColumnWidthProperties ? this.TagWidth : DataGridLength.Auto
				},

				{
					ColumnText,
					this.UseColumnWidthProperties ? this.TextWidth : DataGridLength.Auto
				},

				{
					ColumnType,
					this.UseColumnWidthProperties ? this.TypeWidth : DataGridLength.Auto
				}
			};
		}
		private System.Collections.Generic.Dictionary<string, System.Windows.Visibility> GetColumnVisibilitys()
		{
			return new System.Collections.Generic.Dictionary<string, System.Windows.Visibility>
			{

				{
					ColumnConditionName,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.ConditionNameVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnGroup,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.GroupVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnOPCItemName,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.OPCItemNameVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnReactivationDate,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.ReactivationDateVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnSignalDescription,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.SignalDescriptionVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnSignalName,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.SignalNameVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnStatus,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.StatusVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnTag,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.TagVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnText,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.TextVisibility) : System.Windows.Visibility.Visible
				},

				{
					ColumnType,
					this.UseColumnVisibilityProperties ? HMIAlarmStateManagerViewModel.ConvertToNormalVisibility(this.TypeVisibility) : System.Windows.Visibility.Visible
				}
			};
		}
		private static System.Windows.Visibility ConvertToNormalVisibility(SilverlightVisibility silverlightVisibility)
		{
			if (silverlightVisibility == SilverlightVisibility.Visible)
			{
				return System.Windows.Visibility.Visible;
			}
			return System.Windows.Visibility.Collapsed;
		}
		private void SubsribeToEvents()
		{
			this.UseColumnOrderPropertiesChanged += new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmStateManagerViewModel_UseColumnOrderPropertiesChanged);
			this.ConditionNameOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.GroupOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.OPCItemNameOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.ReactivationDateOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.SignalDescriptionOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.SignalNameOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.StatusOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.TagOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.TextOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.TypeOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.UseColumnWidthPropertiesChanged += new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmStateManagerViewModel_UseColumnWidthPropertiesChanged);
			this.ConditionNameWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.GroupWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.OPCItemNameWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.ReactivationDateWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.SignalDescriptionWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.SignalNameWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.StatusWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.TagWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.TextWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.TypeWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.UseColumnVisibilityPropertiesChanged += new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmStateManagerViewModel_UseColumnVisibilityPropertiesChanged);
			this.ConditionNameVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.GroupVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.OPCItemNameVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.ReactivationDateVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.SignalDescriptionVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.SignalNameVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.StatusVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.TagVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.TextVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.TypeVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
		}
		private void UnSubsribeToEvents()
		{
			this.UseColumnOrderPropertiesChanged -= new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmStateManagerViewModel_UseColumnOrderPropertiesChanged);
			this.ConditionNameOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.GroupOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.OPCItemNameOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.ReactivationDateOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.SignalDescriptionOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.SignalNameOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.StatusOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.TagOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.TextOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.TypeOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmStateManagerViewModel_OrderChanged);
			this.UseColumnWidthPropertiesChanged -= new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmStateManagerViewModel_UseColumnWidthPropertiesChanged);
			this.ConditionNameWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.GroupWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.OPCItemNameWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.ReactivationDateWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.SignalDescriptionWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.SignalNameWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.StatusWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.TagWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.TextWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.TypeWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmStateManagerViewModel_WidthChanged);
			this.UseColumnVisibilityPropertiesChanged -= new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmStateManagerViewModel_UseColumnVisibilityPropertiesChanged);
			this.ConditionNameVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.GroupVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.OPCItemNameVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.ReactivationDateVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.SignalDescriptionVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.SignalNameVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.StatusVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.TagVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.TextVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
			this.TypeVisibilityChanged -= new System.EventHandler<ValueChangedEventArgs<SilverlightVisibility>>(this.HMIAlarmStateManagerViewModel_VisibilityChanged);
		}
		private void HMIAlarmStateManagerViewModel_VisibilityChanged(object sender, ValueChangedEventArgs<SilverlightVisibility> e)
		{
			this.HMIAlarmStateManagerViewModel_UseColumnVisibilityPropertiesChanged(sender, null);
		}
		private void HMIAlarmStateManagerViewModel_UseColumnVisibilityPropertiesChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, System.Windows.Visibility>> e2 = new ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, System.Windows.Visibility>>(null, this.GetColumnVisibilitys());
			this.ColumnVisibilityChanged.Raise(sender, e2);
		}
		private void HMIAlarmStateManagerViewModel_WidthChanged(object sender, ValueChangedEventArgs<double> e)
		{
			this.HMIAlarmStateManagerViewModel_UseColumnWidthPropertiesChanged(sender, null);
		}
		private void HMIAlarmStateManagerViewModel_UseColumnWidthPropertiesChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>> e2 = new ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>>(null, this.GetColumnWidths());
			this.ColumnWidthChanged.Raise(sender, e2);
		}
		private void HMIAlarmStateManagerViewModel_OrderChanged(object sender, ValueChangedEventArgs<int> e)
		{
			this.HMIAlarmStateManagerViewModel_UseColumnOrderPropertiesChanged(sender, null);
		}
		private void HMIAlarmStateManagerViewModel_UseColumnOrderPropertiesChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>> e2 = new ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>>(null, this.GetColumnOrders());
			this.ColumnOrderChanged.Raise(sender, e2);
		}
	}
}
