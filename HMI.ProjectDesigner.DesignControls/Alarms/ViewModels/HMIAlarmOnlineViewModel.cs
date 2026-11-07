// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Models;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using HMI.ProjectDesigner.DesignControls.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
namespace HMI.ProjectDesigner.DesignControls.Alarms.ViewModels
{
	public class HMIAlarmOnlineViewModel : CommonControlViewModel
	{
		private const string ColumnAcknowledged = "确认时间";
		private const string ColumnActive = "激活时间";
		private const string ColumnGone = "结束时间";
		private const string ColumnGroup = "报警组";
		private const string ColumnOpcItem = "OPC点名称";
		private const string ColumnPriority = "报警级别";
		private const string ColumnSignalName = "点位名称";
		private const string ColumnSystemTime = "系统时间";
		private const string ColumnText = "报警描述";
		private const string ColumnType = "报警类型";
		private const string ColumnPriorityColor = "报警级别颜色";
		private const string ColumnAcknowledgedWithComment = "确认信息";
		private ICollectionMonitor<AlarmGroupFilter> alarmGroupFiltersMonitor;
		private ICollectionMonitor<AlarmTypeFilter> alarmTypeFiltersMonitor;
		private ICollectionMonitor<PriorityColor> priorityColorDefinitionsMonitor;
		private ICollectionMonitor<RowColor> rowColorDefinitionsMonitor;
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
			10,
			11,
			12
		});
		private System.DateTime date = System.DateTime.Now;
		private System.Collections.ObjectModel.ObservableCollection<Alarm> alarms = new System.Collections.ObjectModel.ObservableCollection<Alarm>();
		private System.Windows.Media.Brush activeAlarmBackground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Tomato.GetAsFrozen();
		private System.Windows.Media.Brush activeAlarmForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private System.Windows.Media.Brush inactiveAlarmBackground = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 204, 102)).GetAsFrozen();
		private System.Windows.Media.Brush inactiveAlarmForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAlarmBackground = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 153)).GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAlarmForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAndGoneAlarmBackground = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(154, 205, 50)).GetAsFrozen();
		private System.Windows.Media.Brush acknowledgedAndGoneAlarmForeground = (System.Windows.Media.Brush)System.Windows.Media.Brushes.Black.GetAsFrozen();
		private readonly IObservableCollection<RowColor> rowColorDefinitions = new FullyObservableCollection<RowColor>();
		private readonly IObservableCollection<PriorityColor> priorityColorDefinitions = new FullyObservableCollection<PriorityColor>();
		private int refreshInterval = 5;
		private ServerSortOrder serverSortOrder = ServerSortOrder.DateDescending;
		private int maxAlarmRows = 100;
		private bool showDateTimeMilliseconds;
		private bool showHeader = true;
		private bool showScrollBars = true;
		private bool enablePopup = true;
		private AlarmVisibility filter = AlarmVisibility.All;
		private readonly IObservableCollection<AlarmGroupFilter> groupFilters = new FullyObservableCollection<AlarmGroupFilter>();
		private readonly IObservableCollection<AlarmTypeFilter> typeFilters = new FullyObservableCollection<AlarmTypeFilter>();
		private bool filterAlarmGroupsByUser;
		private int minimumPriority = -1;
		private int maximumPriority = -1;
		private string hornPath;
		private bool loop;
		private bool writeSecure;
		private bool useColumnWidthsProperties;
		private bool autoSizeColumns = true;
		private double priorityColorWidth;
		private double activeWidth = 140.0;
		private double systemTimeWidth;
		private double goneWidth = 140.0;
		private double acknowledgedWidth = 140.0;
		private double acknowledgedWithCommentWidth;
		private double textWidth = 140.0;
		private double groupWidth = 140.0;
		private double typeWidth = 140.0;
		private double signalNameWidth = 200.0;
		private double opcItemWidth = 140.0;
		private double priorityWidth = 65.0;
		private bool useColumnOrderProperties;
		private int priorityColorOrder = 1;
		private int activeOrder = 2;
		private int systemTimeOrder = 3;
		private int goneOrder = 4;
		private int acknowledgedOrder = 5;
		private int acknowledgedWithCommentOrder = 6;
		private int textOrder = 7;
		private int groupOrder = 8;
		private int typeOrder = 9;
		private int signalNameOrder = 10;
		private int opcItemOrder = 11;
		private int priorityOrder = 12;

		public event System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>>> ColumnWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>>> ColumnOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<object>> ColorChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<Alarm>>> AlarmsChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ActiveAlarmBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> ActiveAlarmForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> InactiveAlarmBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> InactiveAlarmForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAlarmBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAlarmForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAndGoneAlarmBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> AcknowledgedAndGoneAlarmForegroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> RefreshIntervalChanged;
		public event System.EventHandler<ValueChangedEventArgs<ServerSortOrder>> ServerSortOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> MaxAlarmRowsChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ShowDateTimeMillisecondsChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ShowHeaderChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ShowScrollBarsChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> EnablePopupChanged;
		public event System.EventHandler<ValueChangedEventArgs<AlarmVisibility>> FilterChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> FilterAlarmGroupsByUserChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> MinimumPriorityChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> MaximumPriorityChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> HornPathChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> LoopChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> WriteSecureChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseColumnWidthsPropertiesChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> AutoSizeColumnsChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> PriorityColorWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> ActiveWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> SystemTimeWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> GoneWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> AcknowledgedWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> AcknowledgedWithCommentWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> TextWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> GroupWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> TypeWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> SignalNameWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> OpcItemWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> PriorityWidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> UseColumnOrderPropertiesChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> PriorityColorOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> ActiveOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> SystemTimeOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> GoneOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> AcknowledgedOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> AcknowledgedWithCommentOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> TextOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> GroupOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> TypeOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> SignalNameOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> OpcItemOrderChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> PriorityOrderChanged;
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(430.0, 250.0);
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
		public System.Collections.ObjectModel.ObservableCollection<Alarm> Alarms
		{
			get
			{
				return this.alarms;
			}
			set
			{
				if (this.alarms != value)
				{
					ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<Alarm>> e = new ValueChangedEventArgs<System.Collections.ObjectModel.ObservableCollection<Alarm>>(this.alarms, value);
					this.alarms = value;
					this.AlarmsChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Collections.ObjectModel.ObservableCollection<Alarm>>(() => this.Alarms);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush ActiveAlarmBackground
		{
			get
			{
				return this.activeAlarmBackground;
			}
			set
			{
				if (this.activeAlarmBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.activeAlarmBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.ActiveAlarmBackground);
					this.activeAlarmBackground = value;
					this.ActiveAlarmBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.ActiveAlarmBackground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush ActiveAlarmForeground
		{
			get
			{
				return this.activeAlarmForeground;
			}
			set
			{
				if (this.activeAlarmForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.activeAlarmForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.ActiveAlarmForeground);
					this.activeAlarmForeground = value;
					this.ActiveAlarmForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.ActiveAlarmForeground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush InactiveAlarmBackground
		{
			get
			{
				return this.inactiveAlarmBackground;
			}
			set
			{
				if (this.inactiveAlarmBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.inactiveAlarmBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.InactiveAlarmBackground);
					this.inactiveAlarmBackground = value;
					this.InactiveAlarmBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.InactiveAlarmBackground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush InactiveAlarmForeground
		{
			get
			{
				return this.inactiveAlarmForeground;
			}
			set
			{
				if (this.inactiveAlarmForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.inactiveAlarmForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.InactiveAlarmForeground);
					this.inactiveAlarmForeground = value;
					this.InactiveAlarmForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.InactiveAlarmForeground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush AcknowledgedAlarmBackground
		{
			get
			{
				return this.acknowledgedAlarmBackground;
			}
			set
			{
				if (this.acknowledgedAlarmBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAlarmBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmBackground);
					this.acknowledgedAlarmBackground = value;
					this.AcknowledgedAlarmBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmBackground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush AcknowledgedAlarmForeground
		{
			get
			{
				return this.acknowledgedAlarmForeground;
			}
			set
			{
				if (this.acknowledgedAlarmForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAlarmForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmForeground);
					this.acknowledgedAlarmForeground = value;
					this.AcknowledgedAlarmForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAlarmForeground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush AcknowledgedAndGoneAlarmBackground
		{
			get
			{
				return this.acknowledgedAndGoneAlarmBackground;
			}
			set
			{
				if (this.acknowledgedAndGoneAlarmBackground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAndGoneAlarmBackground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmBackground);
					this.acknowledgedAndGoneAlarmBackground = value;
					this.AcknowledgedAndGoneAlarmBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmBackground);
				}
			}
		}
		[Browsable(true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush AcknowledgedAndGoneAlarmForeground
		{
			get
			{
				return this.acknowledgedAndGoneAlarmForeground;
			}
			set
			{
				if (this.acknowledgedAndGoneAlarmForeground != value)
				{
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.acknowledgedAndGoneAlarmForeground, value);
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmForeground);
					this.acknowledgedAndGoneAlarmForeground = value;
					this.AcknowledgedAndGoneAlarmForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.AcknowledgedAndGoneAlarmForeground);
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(RowColor)), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public IObservableCollection<RowColor> RowColorDefinitions
		{
			get
			{
				return this.rowColorDefinitions;
			}
		}
		[Browsable(true), CollectionEditor(typeof(PriorityColor)), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public IObservableCollection<PriorityColor> PriorityColorDefinitions
		{
			get
			{
				return this.priorityColorDefinitions;
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(36000.0), MinimumValue(1.0)]
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
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public ServerSortOrder ServerSortOrder
		{
			get
			{
				return this.serverSortOrder;
			}
			set
			{
				if (this.serverSortOrder != value)
				{
					ValueChangedEventArgs<ServerSortOrder> e = new ValueChangedEventArgs<ServerSortOrder>(this.serverSortOrder, value);
					base.RaisePropertyChanging<ServerSortOrder>(() => this.ServerSortOrder);
					this.serverSortOrder = value;
					this.ServerSortOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<ServerSortOrder>(() => this.ServerSortOrder);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(5000.0), MinimumValue(1.0)]
		public int MaxAlarmRows
		{
			get
			{
				return this.maxAlarmRows;
			}
			set
			{
				if (this.maxAlarmRows != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.maxAlarmRows, value);
					base.RaisePropertyChanging<int>(() => this.MaxAlarmRows);
					this.maxAlarmRows = value;
					this.MaxAlarmRowsChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.MaxAlarmRows);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool ShowDateTimeMilliseconds
		{
			get
			{
				return this.showDateTimeMilliseconds;
			}
			set
			{
				if (this.showDateTimeMilliseconds != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.showDateTimeMilliseconds, value);
					base.RaisePropertyChanging<bool>(() => this.ShowDateTimeMilliseconds);
					this.showDateTimeMilliseconds = value;
					this.ShowDateTimeMillisecondsChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ShowDateTimeMilliseconds);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool ShowHeader
		{
			get
			{
				return this.showHeader;
			}
			set
			{
				if (this.showHeader != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.showHeader, value);
					base.RaisePropertyChanging<bool>(() => this.ShowHeader);
					this.showHeader = value;
					this.ShowHeaderChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ShowHeader);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool ShowScrollBars
		{
			get
			{
				return this.showScrollBars;
			}
			set
			{
				if (this.showScrollBars != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.showScrollBars, value);
					base.RaisePropertyChanging<bool>(() => this.ShowScrollBars);
					this.showScrollBars = value;
					this.ShowScrollBarsChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ShowScrollBars);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool EnablePopup
		{
			get
			{
				return this.enablePopup;
			}
			set
			{
				if (this.enablePopup != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.enablePopup, value);
					base.RaisePropertyChanging<bool>(() => this.EnablePopup);
					this.enablePopup = value;
					this.EnablePopupChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.EnablePopup);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public AlarmVisibility Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				if (this.filter != value)
				{
					ValueChangedEventArgs<AlarmVisibility> e = new ValueChangedEventArgs<AlarmVisibility>(this.filter, value);
					base.RaisePropertyChanging<AlarmVisibility>(() => this.Filter);
					this.filter = value;
					this.FilterChanged.Raise(this, e);
					base.RaisePropertyChanged<AlarmVisibility>(() => this.Filter);
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(AlarmGroupFilter)), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public IObservableCollection<AlarmGroupFilter> GroupFilters
		{
			get
			{
				return this.groupFilters;
			}
		}
		[Browsable(true), CollectionEditor(typeof(AlarmTypeFilter)), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public IObservableCollection<AlarmTypeFilter> TypeFilters
		{
			get
			{
				return this.typeFilters;
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool FilterAlarmGroupsByUser
		{
			get
			{
				return this.filterAlarmGroupsByUser;
			}
			set
			{
				if (this.filterAlarmGroupsByUser != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.filterAlarmGroupsByUser, value);
					base.RaisePropertyChanging<bool>(() => this.FilterAlarmGroupsByUser);
					this.filterAlarmGroupsByUser = value;
					this.FilterAlarmGroupsByUserChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.FilterAlarmGroupsByUser);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(100.0), MinimumValue(-1.0)]
		public int MinimumPriority
		{
			get
			{
				return this.minimumPriority;
			}
			set
			{
				if (this.minimumPriority != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.minimumPriority, value);
					base.RaisePropertyChanging<int>(() => this.MinimumPriority);
					this.minimumPriority = value;
					this.MinimumPriorityChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.MinimumPriority);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(100.0), MinimumValue(-1.0)]
		public int MaximumPriority
		{
			get
			{
				return this.maximumPriority;
			}
			set
			{
				if (this.maximumPriority != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.maximumPriority, value);
					base.RaisePropertyChanging<int>(() => this.MaximumPriority);
					this.maximumPriority = value;
					this.MaximumPriorityChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.MaximumPriority);
				}
			}
		}
		[Browsable(true), MediaBrowser(MediaType.Audio), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string HornPath
		{
			get
			{
				return this.hornPath;
			}
			set
			{
				if (this.hornPath != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.hornPath, value);
					base.RaisePropertyChanging<string>(() => this.HornPath);
					this.hornPath = value;
					this.HornPathChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.HornPath);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool Loop
		{
			get
			{
				return this.loop;
			}
			set
			{
				if (this.loop != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.loop, value);
					base.RaisePropertyChanging<bool>(() => this.Loop);
					this.loop = value;
					this.LoopChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.Loop);
				}
			}
		}
		[Browsable(true), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
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
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories))]
		public bool UseColumnWidthsProperties
		{
			get
			{
				return this.useColumnWidthsProperties;
			}
			set
			{
				if (this.useColumnWidthsProperties != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.useColumnWidthsProperties, value);
					base.RaisePropertyChanging<bool>(() => this.UseColumnWidthsProperties);
					this.useColumnWidthsProperties = value;
					this.UseColumnWidthsPropertiesChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.UseColumnWidthsProperties);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories))]
		public bool AutoSizeColumns
		{
			get
			{
				return this.autoSizeColumns;
			}
			set
			{
				if (this.autoSizeColumns != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.autoSizeColumns, value);
					base.RaisePropertyChanging<bool>(() => this.AutoSizeColumns);
					this.autoSizeColumns = value;
					this.AutoSizeColumnsChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.AutoSizeColumns);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double PriorityColorWidth
		{
			get
			{
				return this.priorityColorWidth;
			}
			set
			{
				if (this.priorityColorWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.priorityColorWidth, value);
					base.RaisePropertyChanging<double>(() => this.PriorityColorWidth);
					this.priorityColorWidth = value;
					this.PriorityColorWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.PriorityColorWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double ActiveWidth
		{
			get
			{
				return this.activeWidth;
			}
			set
			{
				if (this.activeWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.activeWidth, value);
					base.RaisePropertyChanging<double>(() => this.ActiveWidth);
					this.activeWidth = value;
					this.ActiveWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.ActiveWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double SystemTimeWidth
		{
			get
			{
				return this.systemTimeWidth;
			}
			set
			{
				if (this.systemTimeWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.systemTimeWidth, value);
					base.RaisePropertyChanging<double>(() => this.SystemTimeWidth);
					this.systemTimeWidth = value;
					this.SystemTimeWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.SystemTimeWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double GoneWidth
		{
			get
			{
				return this.goneWidth;
			}
			set
			{
				if (this.goneWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.goneWidth, value);
					base.RaisePropertyChanging<double>(() => this.GoneWidth);
					this.goneWidth = value;
					this.GoneWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.GoneWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double AcknowledgedWidth
		{
			get
			{
				return this.acknowledgedWidth;
			}
			set
			{
				if (this.acknowledgedWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.acknowledgedWidth, value);
					base.RaisePropertyChanging<double>(() => this.AcknowledgedWidth);
					this.acknowledgedWidth = value;
					this.AcknowledgedWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.AcknowledgedWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double AcknowledgedWithCommentWidth
		{
			get
			{
				return this.acknowledgedWithCommentWidth;
			}
			set
			{
				if (this.acknowledgedWithCommentWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.acknowledgedWithCommentWidth, value);
					base.RaisePropertyChanging<double>(() => this.AcknowledgedWithCommentWidth);
					this.acknowledgedWithCommentWidth = value;
					this.AcknowledgedWithCommentWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.AcknowledgedWithCommentWidth);
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
					base.RaisePropertyChanging<double>(() => this.GroupWidth);
					this.groupWidth = value;
					this.GroupWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.GroupWidth);
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
		public double OpcItemWidth
		{
			get
			{
				return this.opcItemWidth;
			}
			set
			{
				if (this.opcItemWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.opcItemWidth, value);
					base.RaisePropertyChanging<double>(() => this.OpcItemWidth);
					this.opcItemWidth = value;
					this.OpcItemWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.OpcItemWidth);
				}
			}
		}
		[Browsable(true), PropertyCategory("ColumnWidths", typeof(StandardPropertyCategories)), MaximumValue(500.0), MinimumValue(0.0)]
		public double PriorityWidth
		{
			get
			{
				return this.priorityWidth;
			}
			set
			{
				if (this.priorityWidth != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.priorityWidth, value);
					base.RaisePropertyChanging<double>(() => this.PriorityWidth);
					this.priorityWidth = value;
					this.PriorityWidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.PriorityWidth);
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
		public int PriorityColorOrder
		{
			get
			{
				return this.priorityColorOrder;
			}
			set
			{
				if (this.priorityColorOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.priorityColorOrder, value);
					base.RaisePropertyChanging<int>(() => this.PriorityColorOrder);
					this.priorityColorOrder = value;
					this.PriorityColorOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.PriorityColorOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int ActiveOrder
		{
			get
			{
				return this.activeOrder;
			}
			set
			{
				if (this.activeOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.activeOrder, value);
					base.RaisePropertyChanging<int>(() => this.ActiveOrder);
					this.activeOrder = value;
					this.ActiveOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.ActiveOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int SystemTimeOrder
		{
			get
			{
				return this.systemTimeOrder;
			}
			set
			{
				if (this.systemTimeOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.systemTimeOrder, value);
					base.RaisePropertyChanging<int>(() => this.SystemTimeOrder);
					this.systemTimeOrder = value;
					this.SystemTimeOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.SystemTimeOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int GoneOrder
		{
			get
			{
				return this.goneOrder;
			}
			set
			{
				if (this.goneOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.goneOrder, value);
					base.RaisePropertyChanging<int>(() => this.GoneOrder);
					this.goneOrder = value;
					this.GoneOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.GoneOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int AcknowledgedOrder
		{
			get
			{
				return this.acknowledgedOrder;
			}
			set
			{
				if (this.acknowledgedOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.acknowledgedOrder, value);
					base.RaisePropertyChanging<int>(() => this.AcknowledgedOrder);
					this.acknowledgedOrder = value;
					this.AcknowledgedOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.AcknowledgedOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int AcknowledgedWithCommentOrder
		{
			get
			{
				return this.acknowledgedWithCommentOrder;
			}
			set
			{
				if (this.acknowledgedWithCommentOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.acknowledgedWithCommentOrder, value);
					base.RaisePropertyChanging<int>(() => this.AcknowledgedWithCommentOrder);
					this.acknowledgedWithCommentOrder = value;
					this.AcknowledgedWithCommentOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.AcknowledgedWithCommentOrder);
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
		public int OpcItemOrder
		{
			get
			{
				return this.opcItemOrder;
			}
			set
			{
				if (this.opcItemOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.opcItemOrder, value);
					base.RaisePropertyChanging<int>(() => this.OpcItemOrder);
					this.opcItemOrder = value;
					this.OpcItemOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.OpcItemOrder);
				}
			}
		}
		[Browsable(true), OptionListEditor("OrderList"), PropertyCategory("ColumnOrders", typeof(StandardPropertyCategories))]
		public int PriorityOrder
		{
			get
			{
				return this.priorityOrder;
			}
			set
			{
				if (this.priorityOrder != value)
				{
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this.priorityOrder, value);
					base.RaisePropertyChanging<int>(() => this.PriorityOrder);
					this.priorityOrder = value;
					this.PriorityOrderChanged.Raise(this, e);
					base.RaisePropertyChanged<int>(() => this.PriorityOrder);
				}
			}
		}
		public HMIAlarmOnlineViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.InitializeRowColorDefinitionsMonitor(container);
			this.InitializePriorityColorDefinitionsMonitor(container);
			this.InitializeAlarmGroupFiltersMonitor(container);
			this.InitializeAlarmTypeFiltersMonitor(container);
			this.RefreshAlarms();
			this.SubscribeToEvents();
			this.InitializePropertyStates();
		}
		private void InitializeAlarmTypeFiltersMonitor(IServiceProvider container)
		{
			this.alarmTypeFiltersMonitor = container.Resolve<ICollectionMonitor<AlarmTypeFilter>>();
			this.alarmTypeFiltersMonitor.Start(this.TypeFilters);
		}
		private void InitializeAlarmGroupFiltersMonitor(IServiceProvider container)
		{
			this.alarmGroupFiltersMonitor = container.Resolve<ICollectionMonitor<AlarmGroupFilter>>();
			this.alarmGroupFiltersMonitor.Start(this.GroupFilters);
		}
		private void InitializePriorityColorDefinitionsMonitor(IServiceProvider container)
		{
			this.priorityColorDefinitionsMonitor = container.Resolve<ICollectionMonitor<PriorityColor>>();
			this.priorityColorDefinitionsMonitor.Start(this.PriorityColorDefinitions);
		}
		private void InitializeRowColorDefinitionsMonitor(IServiceProvider container)
		{
			this.rowColorDefinitionsMonitor = container.Resolve<ICollectionMonitor<RowColor>>();
			this.rowColorDefinitionsMonitor.Start(this.RowColorDefinitions);
		}
		~HMIAlarmOnlineViewModel()
		{
			this.UnSubscribeToEvents();
		}
		public override string GetXamlControlName()
		{
			return "HMIAlarmOnline";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIAlarms=\"clr-namespace:HMI.RuntimeControls.Alarms;assembly=HMI.RuntimeControls\"",
                "HMIAlarmsCommon=\"clr-namespace:HMI.RuntimeControls.Alarms.Common;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIAlarms;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ServerName", this.Project.ServerName);

			base.AddXamlAttribute("ActiveAlarmBackground", this.ActiveAlarmBackground);
			base.AddXamlAttribute("ActiveAlarmForeground", this.ActiveAlarmForeground);
			base.AddXamlAttribute("InactiveAlarmBackground", this.InactiveAlarmBackground);
			base.AddXamlAttribute("InactiveAlarmForeground", this.InactiveAlarmForeground);
			base.AddXamlAttribute("AcknowledgedAlarmBackground", this.AcknowledgedAlarmBackground);
			base.AddXamlAttribute("AcknowledgedAlarmForeground", this.AcknowledgedAlarmForeground);
			base.AddXamlAttribute("AcknowledgedAndGoneAlarmBackground", this.AcknowledgedAndGoneAlarmBackground);
			base.AddXamlAttribute("AcknowledgedAndGoneAlarmForeground", this.AcknowledgedAndGoneAlarmForeground);
			base.AddXamlAttribute("RefreshInterval", this.RefreshInterval);
			base.AddXamlAttribute("ServerSortOrder", this.ServerSortOrder);
			base.AddXamlAttribute("MaxAlarmRows", this.MaxAlarmRows);
			base.AddXamlAttribute("ShowDateTimeMilliseconds", this.ShowDateTimeMilliseconds);
			base.AddXamlAttribute("ShowHeader", this.ShowHeader);
			base.AddXamlAttribute("ShowScrollBars", this.ShowScrollBars);
			base.AddXamlAttribute("EnablePopup", this.EnablePopup);
			base.AddXamlAttribute("Filter", this.Filter);
			base.AddXamlAttribute("FilterAlarmGroupsByUser", this.FilterAlarmGroupsByUser);
			base.AddXamlAttribute("MinimumPriority", this.MinimumPriority);
			base.AddXamlAttribute("MaximumPriority", this.MaximumPriority);
			base.AddXamlAttribute("HornPath", ProjectFilesResolver.Resolve(this.HornPath, MediaType.Audio));
			base.AddXamlAttribute("Loop", this.Loop);
			base.AddXamlAttribute("WriteSecure", this.WriteSecure);
			base.AddXamlAttribute("UseColumnWidthsProperties", this.UseColumnWidthsProperties);
			base.AddXamlAttribute("AutoSizeColumns", this.AutoSizeColumns);
			base.AddXamlAttribute("ActiveWidth", this.ActiveWidth);
			base.AddXamlAttribute("SystemTimeWidth", this.SystemTimeWidth);
			base.AddXamlAttribute("GoneWidth", this.GoneWidth);
			base.AddXamlAttribute("AcknowledgedWidth", this.AcknowledgedWidth);
			base.AddXamlAttribute("TextWidth", this.TextWidth);
			base.AddXamlAttribute("GroupWidth", this.GroupWidth);
			base.AddXamlAttribute("TypeWidth", this.TypeWidth);
			base.AddXamlAttribute("SignalNameWidth", this.SignalNameWidth);
			base.AddXamlAttribute("OpcItemWidth", this.OpcItemWidth);
			base.AddXamlAttribute("PriorityWidth", this.PriorityWidth);
			base.AddXamlAttribute("AcknowledgedWithCommentOrder", this.AcknowledgedWithCommentOrder);
			base.AddXamlAttribute("PriorityColorOrder", this.PriorityColorOrder);
			base.AddXamlAttribute("UseColumnOrderProperties", this.UseColumnOrderProperties);
			base.AddXamlAttribute("ActiveOrder", this.ActiveOrder);
			base.AddXamlAttribute("SystemTimeOrder", this.SystemTimeOrder);
			base.AddXamlAttribute("GoneOrder", this.GoneOrder);
			base.AddXamlAttribute("AcknowledgedOrder", this.AcknowledgedOrder);
			base.AddXamlAttribute("TextOrder", this.TextOrder);
			base.AddXamlAttribute("GroupOrder", this.GroupOrder);
			base.AddXamlAttribute("TypeOrder", this.TypeOrder);
			base.AddXamlAttribute("SignalNameOrder", this.SignalNameOrder);
			base.AddXamlAttribute("OpcItemOrder", this.OpcItemOrder);
			base.AddXamlAttribute("PriorityOrder", this.PriorityOrder);
			base.AddXamlAttribute("AcknowledgedWithCommentWidth", this.AcknowledgedWithCommentWidth);
			base.AddXamlAttribute("PriorityColorWidth", this.PriorityColorWidth);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "RowColorDefinitions", this.RowColorDefinitions, true);
			base.AddXamlCollection(collectionXaml, "PriorityColorDefinitions", this.PriorityColorDefinitions, true);
			base.AddXamlCollection(collectionXaml, "GroupFilters", this.GroupFilters, true);
			base.AddXamlCollection(collectionXaml, "TypeFilters", this.TypeFilters, true);
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
			this.UseColumnWidthsPropertiesChanged.Raise(this, null);
		}
		private Func<Alarm, object> GetSortFunc()
		{
			if (this.ServerSortOrder == ServerSortOrder.DateDescending)
			{
				return (Alarm a) => a.DateOn;
			}
			return (Alarm a) => a.Priority;
		}
		private Func<Alarm, bool> GetStateFilterFunc()
		{
			AlarmVisibility alarmVisibility = this.Filter;
			switch (alarmVisibility)
			{
			case AlarmVisibility.NotAcknowledged:
				return (Alarm a) => a.AlarmType != AlarmState.Acknowledged && a.AlarmType != AlarmState.AcknowledgedAndGone;
			case (AlarmVisibility)3:
				break;
			case AlarmVisibility.Active:
				return (Alarm a) => a.AlarmType == AlarmState.Active || a.AlarmType == AlarmState.Acknowledged;
			default:
				if (alarmVisibility == AlarmVisibility.All)
				{
					return (Alarm a) => true;
				}
				break;
			}
			return (Alarm a) => a.AlarmType != AlarmState.AcknowledgedAndGone;
		}
		private Func<Alarm, bool> GetPriorityFilterFunc()
		{
			if (this.MinimumPriority == -1 && this.MaximumPriority == -1)
			{
				return (Alarm a) => true;
			}
			return (Alarm a) => a.Priority >= this.MinimumPriority && a.Priority <= this.MaximumPriority;
		}
		private Alarm CreateAlarm(AlarmState alarmType, int index)
		{
			return new Alarm(System.Globalization.CultureInfo.CurrentCulture, this.ShowDateTimeMilliseconds)
			{
				AlarmLogID = "alarm log id " + index.ToString("0#"),
				DateOn = this.date.AddSeconds((double)(-(double)index)),
				SystemTime = this.date.AddSeconds((double)(-(double)index)),
				DateOff = (alarmType == AlarmState.Inactive || alarmType == AlarmState.AcknowledgedAndGone) ? this.date.AddSeconds((double)(-(double)index + 2)) : System.DateTime.MinValue,
				DateAcknowledged = (alarmType == AlarmState.Acknowledged || alarmType == AlarmState.AcknowledgedAndGone) ? this.date.AddSeconds((double)(-(double)index + 1)) : System.DateTime.MinValue,
				AlarmID = "alarm id " + index.ToString("0#"),
				Name = "alarm name " + index.ToString("0#"),
				SignalName = "signal " + index.ToString("0#"),
				AliasName = "alias " + index.ToString("0#"),
				Text = "text " + index.ToString("0#"),
				Group = "group " + index.ToString("0#"),
				AlarmGroupSymbolicTextName = "group " + index.ToString("0#"),
				Type = "type " + index.ToString("0#"),
				AlarmTypeSymbolicTextName = "type " + index.ToString("0#"),
				ServerID = "server id " + index.ToString("0#"),
				Priority = 0,
				AcknowledgeComment = (alarmType == AlarmState.Acknowledged || alarmType == AlarmState.AcknowledgedAndGone) ? ("ack comment " + index.ToString("0#")) : string.Empty,
				AcknowledgeUser = "user " + index.ToString("0#"),
				HttpLink = "link " + index.ToString("0#")
			};
		}
		private void RefreshAlarms()
		{
			this.Alarms.Clear();
			System.Collections.Generic.List<Alarm> source = new System.Collections.Generic.List<Alarm>
			{
				this.CreateAlarm(AlarmState.Active, 1),
				this.CreateAlarm(AlarmState.Acknowledged, 2),
				this.CreateAlarm(AlarmState.Inactive, 3),
				this.CreateAlarm(AlarmState.Active, 4),
				this.CreateAlarm(AlarmState.AcknowledgedAndGone, 4)
			};
			Func<Alarm, object> sortFunc = this.GetSortFunc();
			Func<Alarm, bool> stateFilterFunc = this.GetStateFilterFunc();
			Func<Alarm, bool> priorityFilterFunc = this.GetPriorityFilterFunc();
			System.Collections.Generic.IEnumerable<Alarm> items = source.OrderByDescending(sortFunc).Where(stateFilterFunc).Where(priorityFilterFunc).Take(this.MaxAlarmRows);
			this.Alarms.AddRange(items);
		}
		private System.Collections.Generic.Dictionary<string, int> GetColumnOrders()
		{
			return new System.Collections.Generic.Dictionary<string, int>
			{

				{
					ColumnPriorityColor,
					this.UseColumnOrderProperties ? this.PriorityColorOrder : 1
				},

				{
					ColumnActive,
					this.UseColumnOrderProperties ? this.ActiveOrder : 2
				},

				{
					ColumnSystemTime,
					this.UseColumnOrderProperties ? this.SystemTimeOrder : 3
				},

				{
					ColumnGone,
					this.UseColumnOrderProperties ? this.GoneOrder : 4
				},

				{
					ColumnAcknowledged,
					this.UseColumnOrderProperties ? this.AcknowledgedOrder : 5
				},

				{
					ColumnAcknowledgedWithComment,
					this.UseColumnOrderProperties ? this.AcknowledgedWithCommentOrder : 6
				},

				{
					ColumnText,
					this.UseColumnOrderProperties ? this.TextOrder : 7
				},

				{
					ColumnGroup,
					this.UseColumnOrderProperties ? this.GroupOrder : 8
				},

				{
					ColumnType,
					this.UseColumnOrderProperties ? this.TypeOrder : 9
				},

				{
					ColumnSignalName,
					this.UseColumnOrderProperties ? this.SignalNameOrder : 10
				},

				{
					ColumnOpcItem,
					this.UseColumnOrderProperties ? this.OpcItemOrder : 11
				},

				{
					ColumnPriority,
					this.UseColumnOrderProperties ? this.PriorityOrder : 12
				}
			};
		}
		private System.Collections.Generic.Dictionary<string, DataGridLength> GetColumnWidths()
		{
			return new System.Collections.Generic.Dictionary<string, DataGridLength>
			{

				{
					ColumnPriorityColor,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.PriorityColorWidth) : 0.0
				},

				{
					ColumnActive,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.ActiveWidth) : 140.0
				},

				{
					ColumnSystemTime,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.SystemTimeWidth) : 0.0
				},

				{
					ColumnGone,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.GoneWidth) : 140.0
				},

				{
					ColumnAcknowledged,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.AcknowledgedWidth) : 140.0
				},

				{
					ColumnAcknowledgedWithComment,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.AcknowledgedWithCommentWidth) : 0.0
				},

				{
					ColumnText,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.TextWidth) : 140.0
				},

				{
					ColumnGroup,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.GroupWidth) : 140.0
				},

				{
					ColumnType,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.TypeWidth) : 140.0
				},

				{
					ColumnSignalName,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.SignalNameWidth) : 140.0
				},

				{
					ColumnOpcItem,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.OpcItemWidth) : 200.0
				},

				{
					ColumnPriority,
					this.UseColumnWidthsProperties ? (this.AutoSizeColumns ? DataGridLength.Auto : this.PriorityWidth) : 65.0
				}
			};
		}
		private void SubscribeToEvents()
		{
			this.UseColumnOrderPropertiesChanged += new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmOnlineViewModel_UseColumnOrderPropertiesChanged);
			this.ActiveOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.SystemTimeOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.GoneOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.AcknowledgedOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.TextOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.GroupOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.TypeOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.SignalNameOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.OpcItemOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.PriorityOrderChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.UseColumnWidthsPropertiesChanged += new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmOnlineViewModel_UseColumnWidthPropertiesChanged);
			this.AutoSizeColumnsChanged += new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmOnlineViewModel_UseColumnWidthPropertiesChanged);
			this.ActiveWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.SystemTimeWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.GoneWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.AcknowledgedWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.TextWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.GroupWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.TypeWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.SignalNameWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.OpcItemWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.PriorityWidthChanged += new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.ActiveAlarmBackgroundChanged += new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.ActiveAlarmForegroundChanged += new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.InactiveAlarmBackgroundChanged += new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.InactiveAlarmForegroundChanged += new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.AcknowledgedAlarmBackgroundChanged += new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.AcknowledgedAlarmForegroundChanged += new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
		}
		private void UnSubscribeToEvents()
		{
			this.UseColumnOrderPropertiesChanged -= new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmOnlineViewModel_UseColumnOrderPropertiesChanged);
			this.ActiveOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.SystemTimeOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.GoneOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.AcknowledgedOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.TextOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.GroupOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.TypeOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.SignalNameOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.OpcItemOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.PriorityOrderChanged -= new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIAlarmOnlineViewModel_ColumnOrderChanged);
			this.UseColumnWidthsPropertiesChanged -= new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmOnlineViewModel_UseColumnWidthPropertiesChanged);
			this.AutoSizeColumnsChanged -= new System.EventHandler<ValueChangedEventArgs<bool>>(this.HMIAlarmOnlineViewModel_UseColumnWidthPropertiesChanged);
			this.ActiveWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.SystemTimeWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.GoneWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.AcknowledgedWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.TextWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.GroupWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.TypeWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.SignalNameWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.OpcItemWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.PriorityWidthChanged -= new System.EventHandler<ValueChangedEventArgs<double>>(this.HMIAlarmOnlineViewModel_ColumnWidthChanged);
			this.ActiveAlarmBackgroundChanged -= new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.ActiveAlarmForegroundChanged -= new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.InactiveAlarmBackgroundChanged -= new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.InactiveAlarmForegroundChanged -= new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.AcknowledgedAlarmBackgroundChanged -= new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
			this.AcknowledgedAlarmForegroundChanged -= new System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>>(this.HMIAlarmOnlineViewModel_BrushChanged);
		}
		private void HMIAlarmOnlineViewModel_ColumnOrderChanged(object sender, ValueChangedEventArgs<int> e)
		{
			this.HMIAlarmOnlineViewModel_UseColumnOrderPropertiesChanged(sender, null);
		}
		private void HMIAlarmOnlineViewModel_UseColumnOrderPropertiesChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>> e2 = new ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>>(null, this.GetColumnOrders());
			this.ColumnOrderChanged.Raise(sender, e2);
		}
		private void HMIAlarmOnlineViewModel_ColumnWidthChanged(object sender, ValueChangedEventArgs<double> e)
		{
			this.HMIAlarmOnlineViewModel_UseColumnWidthPropertiesChanged(sender, null);
		}
		private void HMIAlarmOnlineViewModel_UseColumnWidthPropertiesChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>> e2 = new ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>>(null, this.GetColumnWidths());
			this.ColumnWidthChanged.Raise(sender, e2);
		}
		private void HMIAlarmOnlineViewModel_BrushChanged(object sender, ValueChangedEventArgs<System.Windows.Media.Brush> e)
		{
			this.ColorChanged.Raise(sender, null);
		}
	}
}
