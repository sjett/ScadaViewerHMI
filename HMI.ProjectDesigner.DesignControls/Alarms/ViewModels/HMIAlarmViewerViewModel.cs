// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using HMI.ProjectDesigner.DesignControls.Alarms;
using HMI.ProjectDesigner.DesignControls.Models;
using HMI.Common.Browsers.Models;
using HMI.Common.Browsers.Services;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.Reactive;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using Range = HMI.Infrastructure.Presentation.Range;
namespace HMI.ProjectDesigner.DesignControls.Alarms.ViewModels
{
	public class HMIAlarmViewerViewModel : CommonControlViewModel
	{
		private const int DeferredUpdateDelay = 500;
		private readonly IAlarmService alarmsService;
		private readonly ICollectionMonitor<AlarmColumnSettings> columnSettingsMonitor;
		private readonly FilterColumnType[] defaultColumnTypes = new FilterColumnType[]
		{
			FilterColumnType.None,
			FilterColumnType.Text,
			FilterColumnType.SignalName,
			FilterColumnType.OpcItem,
			FilterColumnType.Name,
			FilterColumnType.HttpLink,
			FilterColumnType.HelpCause,
			FilterColumnType.HelpEffect,
			FilterColumnType.HelpRepair,
			FilterColumnType.GeneralComment,
			FilterColumnType.OccurrenceComment,
			FilterColumnType.AcknowledgeComment,
			FilterColumnType.NavigationSource,
			FilterColumnType.NavigationTarget
		};
		private readonly ICollectionMonitor<PriorityColor> priorityColorsMonitor;
		private readonly ICollectionMonitor<RowColor> rowColorDefinitionsMonitor;
		private readonly IUiScheduler scheduler;
		private IProjectManagerViewModel projectManagerViewModel;
		private Random random;
		private DispatcherTimer updateAlarmsTimer;
		private Range offlineInterval;
		private string statusMessage;
		private int filteredActiveCount;
		private int activeCount;
		private int filteredActiveOrUnacknowledgedCount;
		private int activeOrUnacknowledgedCount;
		private int filteredUnacknowledgedCount;
		private int unacknowledgedCount;
		private bool isSoundEnabled = true;
		private double soundVolume = 0.5;
		private bool isRefreshingOfflineAlarms;
		private bool isFiltered;
		private readonly ObservableCollection<AlarmDetails> alarms = new ObservableCollection<AlarmDetails>();
		private readonly AlarmTreeItem hiddenRootAlarmGroup = new AlarmTreeItem(Guid.Empty, "*", "*", "*");
		private readonly AlarmTreeItem hiddenRootAlarmType = new AlarmTreeItem(Guid.Empty, "*", "*", "*");
		private bool showAlarmGroupsFilterTree = true;
		private bool showAlarmTypesFilterTree;
		private bool showStatusBar = true;
		private Brush acknowledgedAlarmBackground = (Brush)new SolidColorBrush(Color.FromArgb(255, 255, 255, 153)).GetAsFrozen();
		private Brush acknowledgedAlarmForeground = (Brush)new SolidColorBrush(Colors.Black).GetAsFrozen();
		private Brush acknowledgedAndGoneAlarmBackground = (Brush)new SolidColorBrush(Color.FromArgb(255, 154, 205, 50)).GetAsFrozen();
		private Brush acknowledgedAndGoneAlarmForeground = (Brush)new SolidColorBrush(Colors.Black).GetAsFrozen();
		private Brush activeAlarmBackground = (Brush)new SolidColorBrush(Color.FromArgb(255, 255, 99, 71)).GetAsFrozen();
		private Brush activeAlarmForeground = (Brush)new SolidColorBrush(Colors.Black).GetAsFrozen();
		private Brush inactiveAlarmBackground = (Brush)new SolidColorBrush(Color.FromArgb(255, 0, 204, 102)).GetAsFrozen();
		private Brush inactiveAlarmForeground = (Brush)new SolidColorBrush(Colors.Black).GetAsFrozen();
		private Brush badQualityAlarmBackground = (Brush)new SolidColorBrush(Color.FromArgb(255, 99, 182, 204)).GetAsFrozen();
		private Brush badQualityAlarmForeground = (Brush)new SolidColorBrush(Colors.Black).GetAsFrozen();
		private bool enablePopup = true;
		private string hornPath;
		private bool loop;
		private int refreshInterval = 5;
		private bool showDateTimeMilliseconds;
		private bool showHeader = true;
		private bool showScrollbars = true;
		private bool writeSecure;
		private bool enableNavigation = true;
		private bool showMultilineTextValues = true;
		private bool offline;
		private readonly IObservableCollection<RowColor> rowColorDefinitions = new FullyObservableCollection<RowColor>();
		private readonly IObservableCollection<PriorityColor> priorityColorDefinitions = new FullyObservableCollection<PriorityColor>();
		private readonly IObservableCollection<AlarmColumnSettings> columns = new FullyObservableCollection<AlarmColumnSettings>();
		private AlarmStatusFilter alarmVisibility = AlarmStatusFilter.All;
		private bool showAlarmVisibilityInRuntimeFilter = true;
		private int minimumPriority;
		private int maximumPriority = 100;
		private bool showPriorityInRuntimeFilter = true;
		private int maximumCount = 100;
		private bool showMaximumCountInRuntimeFilter = true;
		private string groupsFilter = "*";
		private bool showGroupsInRuntimeFilter = true;
		private string typesFilter = "*";
		private bool showTypesInRuntimeFilter;
		private string textFilter;
		private FilterColumnType textFilterColumn;
		private readonly ObservableCollection<FilterColumnType> filterColumnTypes = new ObservableCollection<FilterColumnType>();
		private bool showTextFilterInRuntimeFilter = true;
		private bool filterGroupsByUser;
		private ServerSortOrder serverSortOrder = ServerSortOrder.DateDescending;
		private bool showServerSortOrderInRuntimeFilter;
		private bool showTotalAlarmCount = true;
		private bool showActiveAlarmCount = true;
		private bool showUnacknowledgedAlarmCount = true;
		private AlarmCountDisplayMode alarmCountDisplayMode;
		private readonly ObservableCollection<SecurityPolicy> securityPolicies = new ObservableCollection<SecurityPolicy>();
		
		public event EventHandler<ValueChangedEventArgs<string>> StatusMessageChanged;
		public event EventHandler<ValueChangedEventArgs<int>> FilteredActiveCountChanged;
		public event EventHandler<ValueChangedEventArgs<int>> FilteredActiveOrUnacknowledgedCountChanged;
		public event EventHandler<ValueChangedEventArgs<int>> ActiveOrUnacknowledgedCountChanged;
		public event EventHandler<ValueChangedEventArgs<int>> FilteredUnacknowledgedCountChanged;
		public event EventHandler<ValueChangedEventArgs<int>> UnacknowledgedCountChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> IsSoundEnabledChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> IsFilteredChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowAlarmGroupsFilterTreeChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowAlarmTypesFilterTreeChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowStatusBarChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> AcknowledgedAlarmBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> AcknowledgedAlarmForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> AcknowledgedAndGoneAlarmBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> AcknowledgedAndGoneAlarmForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> ActiveAlarmBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> ActiveAlarmForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> InactiveAlarmBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> InactiveAlarmForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> BadQualityAlarmBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> BadQualityAlarmForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> EnablePopupChanged;
		public event EventHandler<ValueChangedEventArgs<string>> HornPathChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> LoopChanged;
		public event EventHandler<ValueChangedEventArgs<int>> RefreshIntervalChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowDateTimeMillisecondsChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowHeaderChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowScrollbarsChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> WriteSecureChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> EnableNavigationChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowMultilineTextValuesChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> OfflineChanged;
		public event EventHandler<ValueChangedEventArgs<AlarmStatusFilter>> AlarmVisibilityChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowAlarmVisibilityInRuntimeFilterChanged;
		public event EventHandler<ValueChangedEventArgs<int>> MinimumPriorityChanged;
		public event EventHandler<ValueChangedEventArgs<int>> MaximumPriorityChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowPriorityInRuntimeFilterChanged;
		public event EventHandler<ValueChangedEventArgs<int>> MaximumCountChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowMaximumCountInRuntimeFilterChanged;
		public event EventHandler<ValueChangedEventArgs<string>> GroupsFilterChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowGroupsInRuntimeFilterChanged;
		public event EventHandler<ValueChangedEventArgs<string>> TypesFilterChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowTypesInRuntimeFilterChanged;
		public event EventHandler<ValueChangedEventArgs<string>> TextFilterChanged;
		public event EventHandler<ValueChangedEventArgs<FilterColumnType>> TextFilterColumnChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowTextFilterInRuntimeFilterChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> FilterGroupsByUserChanged;
		public event EventHandler<ValueChangedEventArgs<ServerSortOrder>> ServerSortOrderChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowServerSortOrderInRuntimeFilterChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowTotalAlarmCountChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowActiveAlarmCountChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ShowUnacknowledgedAlarmCountChanged;
		public event EventHandler<ValueChangedEventArgs<AlarmCountDisplayMode>> AlarmCountDisplayModeChanged;
		public override Size DesignerDesiredSize
		{
			get
			{
				return new Size(400.0, 300.0);
			}
		}
		[DateRangeEditor, PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public Range OfflineInterval
		{
			get
			{
				return offlineInterval;
			}
			set
			{
				if (offlineInterval == value)
				{
					return;
				}
				RaisePropertyChanging(() => OfflineInterval);
				offlineInterval = value;
				RaisePropertyChanged(() => OfflineInterval);
			}
		}
		[Browsable(false)]
		public string StatusMessage
		{
			get
			{
				return statusMessage;
			}
			set
			{
				if (statusMessage != value)
				{
					RaisePropertyChanging(() => StatusMessage);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(statusMessage, value);
					statusMessage = value;
					StatusMessageChanged.Raise(this, e);
					RaisePropertyChanged(() => StatusMessage);
				}
			}
		}
		[Browsable(false)]
		public int FilteredActiveCount
		{
			get
			{
				return filteredActiveCount;
			}
			set
			{
				if (filteredActiveCount != value)
				{
					RaisePropertyChanging(() => FilteredActiveCount);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(filteredActiveCount, value);
					filteredActiveCount = value;
					FilteredActiveCountChanged.Raise(this, e);
					RaisePropertyChanged(() => FilteredActiveCount);
				}
			}
		}
		[Browsable(false)]
		public int ActiveCount
		{
			get
			{
				return activeCount;
			}
			set
			{
				if (activeCount != value)
				{
					RaisePropertyChanging(() => ActiveCount);
					activeCount = value;
					RaisePropertyChanged(() => ActiveCount);
				}
			}
		}
		[Browsable(false)]
		public int FilteredActiveOrUnacknowledgedCount
		{
			get
			{
				return filteredActiveOrUnacknowledgedCount;
			}
			set
			{
				if (filteredActiveOrUnacknowledgedCount != value)
				{
					RaisePropertyChanging(() => FilteredActiveOrUnacknowledgedCount);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(filteredActiveOrUnacknowledgedCount, value);
					filteredActiveOrUnacknowledgedCount = value;
					FilteredActiveOrUnacknowledgedCountChanged.Raise(this, e);
					RaisePropertyChanged(() => FilteredActiveOrUnacknowledgedCount);
				}
			}
		}
		[Browsable(false)]
		public int ActiveOrUnacknowledgedCount
		{
			get
			{
				return activeOrUnacknowledgedCount;
			}
			set
			{
				if (activeOrUnacknowledgedCount != value)
				{
					RaisePropertyChanging(() => ActiveOrUnacknowledgedCount);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(activeOrUnacknowledgedCount, value);
					activeOrUnacknowledgedCount = value;
					ActiveOrUnacknowledgedCountChanged.Raise(this, e);
					RaisePropertyChanged(() => ActiveOrUnacknowledgedCount);
				}
			}
		}
		[Browsable(false)]
		public int FilteredUnacknowledgedCount
		{
			get
			{
				return filteredUnacknowledgedCount;
			}
			set
			{
				if (filteredUnacknowledgedCount != value)
				{
					RaisePropertyChanging(() => FilteredUnacknowledgedCount);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(filteredUnacknowledgedCount, value);
					filteredUnacknowledgedCount = value;
					FilteredUnacknowledgedCountChanged.Raise(this, e);
					RaisePropertyChanged(() => FilteredUnacknowledgedCount);
				}
			}
		}
		[Browsable(false)]
		public int UnacknowledgedCount
		{
			get
			{
				return unacknowledgedCount;
			}
			set
			{
				if (unacknowledgedCount != value)
				{
					RaisePropertyChanging(() => UnacknowledgedCount);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(unacknowledgedCount, value);
					unacknowledgedCount = value;
					UnacknowledgedCountChanged.Raise(this, e);
					RaisePropertyChanged(() => UnacknowledgedCount);
				}
			}
		}
		[Browsable(false)]
		public bool IsSoundEnabled
		{
			get
			{
				return isSoundEnabled;
			}
			set
			{
				if (isSoundEnabled != value)
				{
					RaisePropertyChanging(() => IsSoundEnabled);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(isSoundEnabled, value);
					isSoundEnabled = value;
					IsSoundEnabledChanged.Raise(this, e);
					RaisePropertyChanged(() => IsSoundEnabled);
				}
			}
		}
		[Browsable(false)]
		public double SoundVolume
		{
			get
			{
				return soundVolume;
			}
			set
			{
				if (soundVolume != value)
				{
					RaisePropertyChanging(() => SoundVolume);
					soundVolume = value;
					RaisePropertyChanged(() => SoundVolume);
				}
			}
		}
		[Browsable(false)]
		public bool IsRefreshingOfflineAlarms
		{
			get
			{
				return isRefreshingOfflineAlarms;
			}
			set
			{
				if (isRefreshingOfflineAlarms != value)
				{
					RaisePropertyChanging(() => IsRefreshingOfflineAlarms);
					isRefreshingOfflineAlarms = value;
					RaisePropertyChanged(() => IsRefreshingOfflineAlarms);
				}
			}
		}
		[Browsable(false)]
		public bool IsFiltered
		{
			get
			{
				return isFiltered;
			}
			set
			{
				if (isFiltered != value)
				{
					RaisePropertyChanging(() => IsFiltered);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(isFiltered, value);
					isFiltered = value;
					IsFilteredChanged.Raise(this, e);
					RaisePropertyChanged(() => IsFiltered);
				}
			}
		}
		[Browsable(false)]
		public ObservableCollection<AlarmDetails> Alarms
		{
			get
			{
				return alarms;
			}
		}
		[Browsable(false)]
		public ObservableCollection<AlarmTreeItem> AlarmGroupItems
		{
			get
			{
				return hiddenRootAlarmGroup.Children;
			}
		}
		[Browsable(false)]
		public ObservableCollection<AlarmTreeItem> AlarmTypeItems
		{
			get
			{
				return hiddenRootAlarmType.Children;
			}
		}
		[Browsable(false)]
		public bool IsTextFilterEnabled
		{
			get
			{
				return TextFilterColumn != FilterColumnType.None;
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowAlarmGroupsFilterTree
		{
			get
			{
				return showAlarmGroupsFilterTree;
			}
			set
			{
				if (showAlarmGroupsFilterTree != value)
				{
					RaisePropertyChanging(() => ShowAlarmGroupsFilterTree);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showAlarmGroupsFilterTree, value);
					showAlarmGroupsFilterTree = value;
					ShowAlarmGroupsFilterTreeChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowAlarmGroupsFilterTree);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowAlarmTypesFilterTree
		{
			get
			{
				return showAlarmTypesFilterTree;
			}
			set
			{
				if (showAlarmTypesFilterTree != value)
				{
					RaisePropertyChanging(() => ShowAlarmTypesFilterTree);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showAlarmTypesFilterTree, value);
					showAlarmTypesFilterTree = value;
					ShowAlarmTypesFilterTreeChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowAlarmTypesFilterTree);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowStatusBar
		{
			get
			{
				return showStatusBar;
			}
			set
			{
				if (showStatusBar != value)
				{
					RaisePropertyChanging(() => ShowStatusBar);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showStatusBar, value);
					showStatusBar = value;
					ShowStatusBarChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowStatusBar);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush AcknowledgedAlarmBackground
		{
			get
			{
				return acknowledgedAlarmBackground;
			}
			set
			{
				if (acknowledgedAlarmBackground != value)
				{
					RaisePropertyChanging(() => AcknowledgedAlarmBackground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(acknowledgedAlarmBackground, value);
					acknowledgedAlarmBackground = value;
					AcknowledgedAlarmBackgroundChanged.Raise(this, e);
					RaisePropertyChanged(() => AcknowledgedAlarmBackground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush AcknowledgedAlarmForeground
		{
			get
			{
				return acknowledgedAlarmForeground;
			}
			set
			{
				if (acknowledgedAlarmForeground != value)
				{
					RaisePropertyChanging(() => AcknowledgedAlarmForeground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(acknowledgedAlarmForeground, value);
					acknowledgedAlarmForeground = value;
					AcknowledgedAlarmForegroundChanged.Raise(this, e);
					RaisePropertyChanged(() => AcknowledgedAlarmForeground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush AcknowledgedAndGoneAlarmBackground
		{
			get
			{
				return acknowledgedAndGoneAlarmBackground;
			}
			set
			{
				if (acknowledgedAndGoneAlarmBackground != value)
				{
					RaisePropertyChanging(() => AcknowledgedAndGoneAlarmBackground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(acknowledgedAndGoneAlarmBackground, value);
					acknowledgedAndGoneAlarmBackground = value;
					AcknowledgedAndGoneAlarmBackgroundChanged.Raise(this, e);
					RaisePropertyChanged(() => AcknowledgedAndGoneAlarmBackground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush AcknowledgedAndGoneAlarmForeground
		{
			get
			{
				return acknowledgedAndGoneAlarmForeground;
			}
			set
			{
				if (acknowledgedAndGoneAlarmForeground != value)
				{
					RaisePropertyChanging(() => AcknowledgedAndGoneAlarmForeground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(acknowledgedAndGoneAlarmForeground, value);
					acknowledgedAndGoneAlarmForeground = value;
					AcknowledgedAndGoneAlarmForegroundChanged.Raise(this, e);
					RaisePropertyChanged(() => AcknowledgedAndGoneAlarmForeground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush ActiveAlarmBackground
		{
			get
			{
				return activeAlarmBackground;
			}
			set
			{
				if (activeAlarmBackground != value)
				{
					RaisePropertyChanging(() => ActiveAlarmBackground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(activeAlarmBackground, value);
					activeAlarmBackground = value;
					ActiveAlarmBackgroundChanged.Raise(this, e);
					RaisePropertyChanged(() => ActiveAlarmBackground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush ActiveAlarmForeground
		{
			get
			{
				return activeAlarmForeground;
			}
			set
			{
				if (activeAlarmForeground != value)
				{
					RaisePropertyChanging(() => ActiveAlarmForeground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(activeAlarmForeground, value);
					activeAlarmForeground = value;
					ActiveAlarmForegroundChanged.Raise(this, e);
					RaisePropertyChanged(() => ActiveAlarmForeground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush InactiveAlarmBackground
		{
			get
			{
				return inactiveAlarmBackground;
			}
			set
			{
				if (inactiveAlarmBackground != value)
				{
					RaisePropertyChanging(() => InactiveAlarmBackground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(inactiveAlarmBackground, value);
					inactiveAlarmBackground = value;
					InactiveAlarmBackgroundChanged.Raise(this, e);
					RaisePropertyChanged(() => InactiveAlarmBackground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush InactiveAlarmForeground
		{
			get
			{
				return inactiveAlarmForeground;
			}
			set
			{
				if (inactiveAlarmForeground != value)
				{
					RaisePropertyChanging(() => InactiveAlarmForeground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(inactiveAlarmForeground, value);
					inactiveAlarmForeground = value;
					InactiveAlarmForegroundChanged.Raise(this, e);
					RaisePropertyChanged(() => InactiveAlarmForeground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush BadQualityAlarmBackground
		{
			get
			{
				return badQualityAlarmBackground;
			}
			set
			{
				if (badQualityAlarmBackground != value)
				{
					RaisePropertyChanging(() => BadQualityAlarmBackground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(badQualityAlarmBackground, value);
					badQualityAlarmBackground = value;
					BadQualityAlarmBackgroundChanged.Raise(this, e);
					RaisePropertyChanged(() => BadQualityAlarmBackground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush BadQualityAlarmForeground
		{
			get
			{
				return badQualityAlarmForeground;
			}
			set
			{
				if (badQualityAlarmForeground != value)
				{
					RaisePropertyChanging(() => BadQualityAlarmForeground);
					ValueChangedEventArgs<Brush> e = new ValueChangedEventArgs<Brush>(badQualityAlarmForeground, value);
					badQualityAlarmForeground = value;
					BadQualityAlarmForegroundChanged.Raise(this, e);
					RaisePropertyChanged(() => BadQualityAlarmForeground);
					DeferredUpdateAlarms();
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool EnablePopup
		{
			get
			{
				return enablePopup;
			}
			set
			{
				if (enablePopup != value)
				{
					RaisePropertyChanging(() => EnablePopup);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(enablePopup, value);
					enablePopup = value;
					EnablePopupChanged.Raise(this, e);
					RaisePropertyChanged(() => EnablePopup);
				}
			}
		}
		[MediaBrowser(MediaType.Audio), PropertyCategory("AlarmConfiguration", typeof(StandardPropertyCategories))]
		public string HornPath
		{
			get
			{
				return hornPath;
			}
			set
			{
				if (hornPath != value)
				{
					RaisePropertyChanging(() => HornPath);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(hornPath, value);
					hornPath = value;
					HornPathChanged.Raise(this, e);
					RaisePropertyChanged(() => HornPath);
				}
			}
		}
		[PropertyCategory("AlarmConfiguration", typeof(StandardPropertyCategories))]
		public bool Loop
		{
			get
			{
				return loop;
			}
			set
			{
				if (loop != value)
				{
					RaisePropertyChanging(() => Loop);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(loop, value);
					loop = value;
					LoopChanged.Raise(this, e);
					RaisePropertyChanged(() => Loop);
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories)), MaximumValue(36000.0), MinimumValue(0.0)]
		public int RefreshInterval
		{
			get
			{
				return refreshInterval;
			}
			set
			{
				if (refreshInterval != value)
				{
					RaisePropertyChanging(() => RefreshInterval);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(refreshInterval, value);
					refreshInterval = value;
					RefreshIntervalChanged.Raise(this, e);
					RaisePropertyChanged(() => RefreshInterval);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowDateTimeMilliseconds
		{
			get
			{
				return showDateTimeMilliseconds;
			}
			set
			{
				if (showDateTimeMilliseconds != value)
				{
					RaisePropertyChanging(() => ShowDateTimeMilliseconds);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showDateTimeMilliseconds, value);
					showDateTimeMilliseconds = value;
					ShowDateTimeMillisecondsChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowDateTimeMilliseconds);
					UpdateAlarms();
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowHeader
		{
			get
			{
				return showHeader;
			}
			set
			{
				if (showHeader != value)
				{
					RaisePropertyChanging(() => ShowHeader);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showHeader, value);
					showHeader = value;
					ShowHeaderChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowHeader);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowScrollbars
		{
			get
			{
				return showScrollbars;
			}
			set
			{
				if (showScrollbars != value)
				{
					RaisePropertyChanging(() => ShowScrollbars);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showScrollbars, value);
					showScrollbars = value;
					ShowScrollbarsChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowScrollbars);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public bool WriteSecure
		{
			get
			{
				return writeSecure;
			}
			set
			{
				if (writeSecure != value)
				{
					RaisePropertyChanging(() => WriteSecure);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(writeSecure, value);
					writeSecure = value;
					WriteSecureChanged.Raise(this, e);
					RaisePropertyChanged(() => WriteSecure);
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool EnableNavigation
		{
			get
			{
				return enableNavigation;
			}
			set
			{
				if (enableNavigation != value)
				{
					RaisePropertyChanging(() => EnableNavigation);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(enableNavigation, value);
					enableNavigation = value;
					EnableNavigationChanged.Raise(this, e);
					RaisePropertyChanged(() => EnableNavigation);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowMultilineTextValues
		{
			get
			{
				return showMultilineTextValues;
			}
			set
			{
				if (showMultilineTextValues != value)
				{
					RaisePropertyChanging(() => ShowMultilineTextValues);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showMultilineTextValues, value);
					showMultilineTextValues = value;
					ShowMultilineTextValuesChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowMultilineTextValues);
					UpdateAlarms();
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool Offline
		{
			get
			{
				return offline;
			}
			set
			{
				if (offline != value)
				{
					RaisePropertyChanging(() => Offline);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(offline, value);
					offline = value;
					OfflineChanged.Raise(this, e);
					RaisePropertyChanged(() => Offline);
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(RowColor)), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public IObservableCollection<RowColor> RowColorDefinitions
		{
			get
			{
				return rowColorDefinitions;
			}
		}
		[Browsable(true), CollectionEditor(typeof(PriorityColor)), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public IObservableCollection<PriorityColor> PriorityColorDefinitions
		{
			get
			{
				return priorityColorDefinitions;
			}
		}
		[Browsable(true), CollectionEditor(typeof(AlarmColumnSettings)), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public IObservableCollection<AlarmColumnSettings> Columns
		{
			get
			{
				return columns;
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public AlarmStatusFilter AlarmVisibility
		{
			get
			{
				return alarmVisibility;
			}
			set
			{
				if (alarmVisibility != value)
				{
					RaisePropertyChanging(() => AlarmVisibility);
					ValueChangedEventArgs<AlarmStatusFilter> e = new ValueChangedEventArgs<AlarmStatusFilter>(alarmVisibility, value);
					alarmVisibility = value;
					AlarmVisibilityChanged.Raise(this, e);
					RaisePropertyChanged(() => AlarmVisibility);
					UpdateAlarms();
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public bool ShowAlarmVisibilityInRuntimeFilter
		{
			get
			{
				return showAlarmVisibilityInRuntimeFilter;
			}
			set
			{
				if (showAlarmVisibilityInRuntimeFilter != value)
				{
					RaisePropertyChanging(() => ShowAlarmVisibilityInRuntimeFilter);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showAlarmVisibilityInRuntimeFilter, value);
					showAlarmVisibilityInRuntimeFilter = value;
					ShowAlarmVisibilityInRuntimeFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowAlarmVisibilityInRuntimeFilter);
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories)), MaximumValue("MaximumPriority"), MinimumValue(0.0)]
		public int MinimumPriority
		{
			get
			{
				return minimumPriority;
			}
			set
			{
				if (minimumPriority != value)
				{
					RaisePropertyChanging(() => MinimumPriority);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(minimumPriority, value);
					minimumPriority = value;
					MinimumPriorityChanged.Raise(this, e);
					RaisePropertyChanged(() => MinimumPriority);
					UpdateAlarms();
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories)), MaximumValue(1000000.0), MinimumValue("MinimumPriority")]
		public int MaximumPriority
		{
			get
			{
				return maximumPriority;
			}
			set
			{
				if (maximumPriority != value)
				{
					RaisePropertyChanging(() => MaximumPriority);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(maximumPriority, value);
					maximumPriority = value;
					MaximumPriorityChanged.Raise(this, e);
					RaisePropertyChanged(() => MaximumPriority);
					UpdateAlarms();
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public bool ShowPriorityInRuntimeFilter
		{
			get
			{
				return showPriorityInRuntimeFilter;
			}
			set
			{
				if (showPriorityInRuntimeFilter != value)
				{
					RaisePropertyChanging(() => ShowPriorityInRuntimeFilter);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showPriorityInRuntimeFilter, value);
					showPriorityInRuntimeFilter = value;
					ShowPriorityInRuntimeFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowPriorityInRuntimeFilter);
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories)), MaximumValue(9999.0), MinimumValue(1.0)]
		public int MaximumCount
		{
			get
			{
				return maximumCount;
			}
			set
			{
				if (maximumCount != value)
				{
					RaisePropertyChanging(() => MaximumCount);
					ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(maximumCount, value);
					maximumCount = value;
					MaximumCountChanged.Raise(this, e);
					RaisePropertyChanged(() => MaximumCount);
					UpdateAlarms();
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public bool ShowMaximumCountInRuntimeFilter
		{
			get
			{
				return showMaximumCountInRuntimeFilter;
			}
			set
			{
				if (showMaximumCountInRuntimeFilter != value)
				{
					RaisePropertyChanging(() => ShowMaximumCountInRuntimeFilter);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showMaximumCountInRuntimeFilter, value);
					showMaximumCountInRuntimeFilter = value;
					ShowMaximumCountInRuntimeFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowMaximumCountInRuntimeFilter);
				}
			}
		}
		[AlarmGroupTreeBrowser, PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public string GroupsFilter
		{
			get
			{
				return groupsFilter;
			}
			set
			{
				if (groupsFilter != value)
				{
					RaisePropertyChanging(() => GroupsFilter);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(groupsFilter, value);
					groupsFilter = value;
					GroupsFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => GroupsFilter);
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public bool ShowGroupsInRuntimeFilter
		{
			get
			{
				return showGroupsInRuntimeFilter;
			}
			set
			{
				if (showGroupsInRuntimeFilter != value)
				{
					RaisePropertyChanging(() => ShowGroupsInRuntimeFilter);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showGroupsInRuntimeFilter, value);
					showGroupsInRuntimeFilter = value;
					ShowGroupsInRuntimeFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowGroupsInRuntimeFilter);
				}
			}
		}
		[AlarmTypeTreeBrowser, PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public string TypesFilter
		{
			get
			{
				return typesFilter;
			}
			set
			{
				if (typesFilter != value)
				{
					RaisePropertyChanging(() => TypesFilter);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(typesFilter, value);
					typesFilter = value;
					TypesFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => TypesFilter);
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public bool ShowTypesInRuntimeFilter
		{
			get
			{
				return showTypesInRuntimeFilter;
			}
			set
			{
				if (showTypesInRuntimeFilter != value)
				{
					RaisePropertyChanging(() => ShowTypesInRuntimeFilter);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showTypesInRuntimeFilter, value);
					showTypesInRuntimeFilter = value;
					ShowTypesInRuntimeFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowTypesInRuntimeFilter);
				}
			}
		}
		[EnabledProperty("IsTextFilterEnabled"), PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public string TextFilter
		{
			get
			{
				return textFilter;
			}
			set
			{
				if (textFilter != value)
				{
					RaisePropertyChanging(() => TextFilter);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(textFilter, value);
					textFilter = value;
					TextFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => TextFilter);
					UpdateAlarms();
				}
			}
		}
		[OptionListEditor("FilterColumnTypes"), PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public FilterColumnType TextFilterColumn
		{
			get
			{
				return textFilterColumn;
			}
			set
			{
				if (textFilterColumn != value)
				{
					RaisePropertyChanging(() => TextFilterColumn);
					ValueChangedEventArgs<FilterColumnType> e = new ValueChangedEventArgs<FilterColumnType>(textFilterColumn, value);
					textFilterColumn = value;
					TextFilterColumnChanged.Raise(this, e);
					RaisePropertyChanged(() => TextFilterColumn);
					RaisePropertyChanged(() => IsTextFilterEnabled);
					UpdateAlarms();
				}
			}
		}
		public ObservableCollection<FilterColumnType> FilterColumnTypes
		{
			get
			{
				return filterColumnTypes;
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public bool ShowTextFilterInRuntimeFilter
		{
			get
			{
				return showTextFilterInRuntimeFilter;
			}
			set
			{
				if (showTextFilterInRuntimeFilter != value)
				{
					RaisePropertyChanging(() => ShowTextFilterInRuntimeFilter);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showTextFilterInRuntimeFilter, value);
					showTextFilterInRuntimeFilter = value;
					ShowTextFilterInRuntimeFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowTextFilterInRuntimeFilter);
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public bool FilterGroupsByUser
		{
			get
			{
				return filterGroupsByUser;
			}
			set
			{
				if (filterGroupsByUser != value)
				{
					RaisePropertyChanging(() => FilterGroupsByUser);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(filterGroupsByUser, value);
					filterGroupsByUser = value;
					FilterGroupsByUserChanged.Raise(this, e);
					RaisePropertyChanged(() => FilterGroupsByUser);
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public ServerSortOrder ServerSortOrder
		{
			get
			{
				return serverSortOrder;
			}
			set
			{
				if (serverSortOrder != value)
				{
					RaisePropertyChanging(() => ServerSortOrder);
					ValueChangedEventArgs<ServerSortOrder> e = new ValueChangedEventArgs<ServerSortOrder>(serverSortOrder, value);
					serverSortOrder = value;
					ServerSortOrderChanged.Raise(this, e);
					RaisePropertyChanged(() => ServerSortOrder);
					UpdateAlarms();
				}
			}
		}
		[PropertyCategory("FilterSettings", typeof(StandardPropertyCategories))]
		public bool ShowServerSortOrderInRuntimeFilter
		{
			get
			{
				return showServerSortOrderInRuntimeFilter;
			}
			set
			{
				if (showServerSortOrderInRuntimeFilter != value)
				{
					RaisePropertyChanging(() => ShowServerSortOrderInRuntimeFilter);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showServerSortOrderInRuntimeFilter, value);
					showServerSortOrderInRuntimeFilter = value;
					ShowServerSortOrderInRuntimeFilterChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowServerSortOrderInRuntimeFilter);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowTotalAlarmCount
		{
			get
			{
				return showTotalAlarmCount;
			}
			set
			{
				if (showTotalAlarmCount != value)
				{
					RaisePropertyChanging(() => ShowTotalAlarmCount);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showTotalAlarmCount, value);
					showTotalAlarmCount = value;
					ShowTotalAlarmCountChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowTotalAlarmCount);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowActiveAlarmCount
		{
			get
			{
				return showActiveAlarmCount;
			}
			set
			{
				if (showActiveAlarmCount != value)
				{
					RaisePropertyChanging(() => ShowActiveAlarmCount);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showActiveAlarmCount, value);
					showActiveAlarmCount = value;
					ShowActiveAlarmCountChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowActiveAlarmCount);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool ShowUnacknowledgedAlarmCount
		{
			get
			{
				return showUnacknowledgedAlarmCount;
			}
			set
			{
				if (showUnacknowledgedAlarmCount != value)
				{
					RaisePropertyChanging(() => ShowUnacknowledgedAlarmCount);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(showUnacknowledgedAlarmCount, value);
					showUnacknowledgedAlarmCount = value;
					ShowUnacknowledgedAlarmCountChanged.Raise(this, e);
					RaisePropertyChanged(() => ShowUnacknowledgedAlarmCount);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public AlarmCountDisplayMode AlarmCountDisplayMode
		{
			get
			{
				return alarmCountDisplayMode;
			}
			set
			{
				if (alarmCountDisplayMode != value)
				{
					RaisePropertyChanging(() => AlarmCountDisplayMode);
					ValueChangedEventArgs<AlarmCountDisplayMode> e = new ValueChangedEventArgs<AlarmCountDisplayMode>(alarmCountDisplayMode, value);
					alarmCountDisplayMode = value;
					AlarmCountDisplayModeChanged.Raise(this, e);
					RaisePropertyChanged(() => AlarmCountDisplayMode);
				}
			}
		}
		[CollectionEditor(typeof(SecurityPolicy), IsCollectionReadOnly = true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public ObservableCollection<SecurityPolicy> SecurityPolicies
		{
			get
			{
				return securityPolicies;
			}
		}
		public HMIAlarmViewerViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
			columnSettingsMonitor = container.Resolve<ICollectionMonitor<AlarmColumnSettings>>();
			columnSettingsMonitor.Start(Columns);
			rowColorDefinitionsMonitor = container.Resolve<ICollectionMonitor<RowColor>>();
			rowColorDefinitionsMonitor.Start(RowColorDefinitions);
			priorityColorsMonitor = container.Resolve<ICollectionMonitor<PriorityColor>>();
			priorityColorsMonitor.Start(PriorityColorDefinitions);
			alarmsService = container.Resolve<IAlarmService>();
			scheduler = container.Resolve<IUiScheduler>();
			InitializeUpdateAlarmsTimer();
			StartWatchingProjectServerSettings(container);
			AddDefaultColumns();
			UpdateAlarms();
			UpdateAlarmGroups();
			UpdateAlarmTypes();
			FillSecurityPolicies();
		}
		private void InitializeUpdateAlarmsTimer()
		{
			updateAlarmsTimer = new DispatcherTimer();
			updateAlarmsTimer.Interval = TimeSpan.FromMilliseconds(500.0);
			updateAlarmsTimer.Tick += new EventHandler(updateAlarmsTimer_Tick);
		}
		private void UnInitializeUpdateAlarmsTimer()
		{
			updateAlarmsTimer.Stop();
			updateAlarmsTimer.Tick -= new EventHandler(updateAlarmsTimer_Tick);
		}
		private void updateAlarmsTimer_Tick(object sender, EventArgs e)
		{
			UpdateAlarms();
			updateAlarmsTimer.Stop();
		}
		private void StartWatchingProjectServerSettings(IServiceProvider container)
		{
			projectManagerViewModel = container.Resolve<IProjectManagerViewModel>();
			projectManagerViewModel.ProjectChanged += new EventHandler<ValueChangedEventArgs<IProjectViewModel>>(projectManagerViewModel_ProjectChanged);
			SubscribeToProjectServerSettingsChanged(projectManagerViewModel.Project);
		}
		private void projectManagerViewModel_ProjectChanged(object sender, ValueChangedEventArgs<IProjectViewModel> e)
		{
			UnsubscribeToProjectServerSettingsChanged(e.OldValue);
			SubscribeToProjectServerSettingsChanged(e.NewValue);
		}
		private void SubscribeToProjectServerSettingsChanged(IProjectViewModel project)
		{
			if (project != null)
			{
				project.ServerSettingsChanged+=new EventHandler(Project_ServerSettingsChanged);
				InitializeFilterColumnTypes();
			}
		}
		private void UnsubscribeToProjectServerSettingsChanged(IProjectViewModel project)
		{
			if (project != null)
			{
				project.ServerSettingsChanged-=new EventHandler(Project_ServerSettingsChanged);
			}
		}
		private void Project_ServerSettingsChanged(object sender, EventArgs e)
		{
			InitializeFilterColumnTypes();
		}
		private void InitializeFilterColumnTypes()
		{
			if (projectManagerViewModel.Project != null)
			{
				alarmsService.GetExtendedAlarmProperties().ToList()
					.ObserveOn(scheduler)
					.Subscribe(new Action<IList<ExtendedAlarmProperty>>(OnReceivedExtendedAlarmProperties), 
						new Action<Exception>(OnErrorReceivingAlarmProperties));
				return;
			}
			UpdateFilterColumnTypes(Enumerable.Empty<ExtendedAlarmProperty>());
		}
		private void InitializeDefaultFilterColumnTypes()
		{
			FilterColumnTypes.AddRange(defaultColumnTypes);
		}
		private void OnErrorReceivingAlarmProperties(Exception obj)
		{
			UpdateFilterColumnTypes(Enumerable.Empty<ExtendedAlarmProperty>());
		}
		private void OnReceivedExtendedAlarmProperties(IEnumerable<ExtendedAlarmProperty> properties)
		{
			UpdateFilterColumnTypes(properties);
		}
		private void UpdateFilterColumnTypes(IEnumerable<ExtendedAlarmProperty> properties)
		{
			FilterColumnType item = TextFilterColumn;
			FilterColumnTypes.Clear();
			InitializeDefaultFilterColumnTypes();
			AddAvailableExtendedProperties(properties);
			if (FilterColumnTypes.Contains(item))
			{
				TextFilterColumn = item;
				return;
			}
			TextFilterColumn = FilterColumnType.None;
		}
		private void AddAvailableExtendedProperties(IEnumerable<ExtendedAlarmProperty> properties)
		{
			IEnumerable<FilterColumnType> items = 
				from p in properties
				where p.IsActive
				orderby p.Index
				select p into property
				select GetFilterColumnType(property);
			FilterColumnTypes.AddRange(items);
		}
		private FilterColumnType GetFilterColumnType(ExtendedAlarmProperty property)
		{
			return (FilterColumnType)Enum.Parse(typeof(FilterColumnType), "ExtendedProperty" + property.Index);
		}
		private void FillSecurityPolicies()
		{
			SecurityPolicies.Clear();
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.Help));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.Refresh));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.Filter));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.Acknowledge));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.Comments));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.ProcessingAndDisplay));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.SoundToggle));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.Navigate));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.OnlineMode));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.OfflineMode));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.CustomQueryMode));
			SecurityPolicies.Add(new SecurityPolicy(Services, Parent, AlarmViewerSecuredAction.HttpLink));
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIAlarms;
		//}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIAlarms=\"clr-namespace:HMI.RuntimeControls.Alarms;assembly=HMI.RuntimeControls\"",
                "HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\""
            };
		}
		public override string GetXamlControlName()
		{
			return "HMIAlarmViewer";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("ServerName", this.Project.ServerName);
			base.AddXamlAttribute("RefreshInterval", this.RefreshInterval);
			base.AddXamlAttribute("HornPath", ProjectFilesResolver.Resolve(this.HornPath, MediaType.Audio));
			base.AddXamlAttribute("Loop", this.Loop);
			base.AddXamlAttribute("WriteSecure", this.WriteSecure);
			base.AddXamlAttribute("EnableNavigation", this.EnableNavigation);
			base.AddXamlAttribute("EnablePopup", this.EnablePopup);
			base.AddXamlAttribute("ShowAlarmGroupsFilterTree", this.ShowAlarmGroupsFilterTree);
			base.AddXamlAttribute("ShowAlarmTypesFilterTree", this.ShowAlarmTypesFilterTree);
			base.AddXamlAttribute("ShowStatusBar", this.ShowStatusBar);
			base.AddXamlAttribute("ShowHeader", this.ShowHeader);
			base.AddXamlAttribute("ShowScrollBars", this.ShowScrollbars);
			base.AddXamlAttribute("ShowDateTimeMilliseconds", this.ShowDateTimeMilliseconds);
			base.AddXamlAttribute("ShowMultilineTextValues", this.ShowMultilineTextValues);
			base.AddXamlAttribute("ShowTotalAlarmCount", this.ShowTotalAlarmCount);
			base.AddXamlAttribute("ShowActiveAlarmCount", this.ShowActiveAlarmCount);
			base.AddXamlAttribute("ShowUnacknowledgedAlarmCount", this.ShowUnacknowledgedAlarmCount);
			base.AddXamlAttribute("AlarmCountDisplayMode", this.AlarmCountDisplayMode);
			base.AddXamlAttribute("Offline", this.Offline);
			base.AddXamlAttribute("ShowGroupsInRuntimeFilter", this.ShowGroupsInRuntimeFilter);
			base.AddXamlAttribute("GroupsFilter", this.GroupsFilter);
			base.AddXamlAttribute("FilterGroupsByUser", this.FilterGroupsByUser);
			base.AddXamlAttribute("ShowTypesInRuntimeFilter", this.ShowTypesInRuntimeFilter);
			base.AddXamlAttribute("TypesFilter", this.TypesFilter);
			base.AddXamlAttribute("ShowMaximumCountInRuntimeFilter", this.ShowMaximumCountInRuntimeFilter);
			base.AddXamlAttribute("MaximumCount", this.MaximumCount);
			base.AddXamlAttribute("ShowServerSortOrderInRuntimeFilter", this.ShowServerSortOrderInRuntimeFilter);
			base.AddXamlAttribute("ServerSortOrder", this.ServerSortOrder);
			base.AddXamlAttribute("ShowAlarmVisibilityInRuntimeFilter", this.ShowAlarmVisibilityInRuntimeFilter);
			base.AddXamlAttribute("AlarmVisibility", this.AlarmVisibility);
			base.AddXamlAttribute("ShowPriorityInRuntimeFilter", this.ShowPriorityInRuntimeFilter);
			base.AddXamlAttribute("MinimumPriority", this.MinimumPriority);
			base.AddXamlAttribute("MaximumPriority", this.MaximumPriority);
			base.AddXamlAttribute("ShowTextFilterInRuntimeFilter", this.ShowTextFilterInRuntimeFilter);
			base.AddXamlAttribute("TextFilterColumn", this.TextFilterColumn);
			base.AddXamlAttribute("TextFilter", this.TextFilter);
			base.AddXamlAttribute("ActiveAlarmBackground", this.ActiveAlarmBackground);
			base.AddXamlAttribute("ActiveAlarmForeground", this.ActiveAlarmForeground);
			base.AddXamlAttribute("InactiveAlarmBackground", this.InactiveAlarmBackground);
			base.AddXamlAttribute("InactiveAlarmForeground", this.InactiveAlarmForeground);
			base.AddXamlAttribute("AcknowledgedAlarmBackground", this.AcknowledgedAlarmBackground);
			base.AddXamlAttribute("AcknowledgedAlarmForeground", this.AcknowledgedAlarmForeground);
			base.AddXamlAttribute("AcknowledgedAndGoneAlarmBackground", this.AcknowledgedAndGoneAlarmBackground);
			base.AddXamlAttribute("AcknowledgedAndGoneAlarmForeground", this.AcknowledgedAndGoneAlarmForeground);
			base.AddXamlAttribute("BadQualityAlarmBackground", this.BadQualityAlarmBackground);
			base.AddXamlAttribute("BadQualityAlarmForeground", this.BadQualityAlarmForeground);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			base.AddXamlCollection(collectionXaml, "Columns", this.Columns, true);
			base.AddXamlCollection(collectionXaml, "RowColorDefinitions", this.RowColorDefinitions, true);
			base.AddXamlCollection(collectionXaml, "PriorityColorDefinitions", this.PriorityColorDefinitions, true);
			this.AddSecurityPolicies(collectionXaml);
			this.AddOfflineInterval(collectionXaml);
		}
		private void AddOfflineInterval(System.Text.StringBuilder collectionXaml)
		{
			string namespacePrefix = base.GetNamespacePrefix();
			string namespacePrefix2 = base.GetNamespacePrefix("HMIShared=\"clr-namespace:HMI.Runtime.Shared;assembly=HMI.Runtime.Shared\"");
			string xamlControlName = this.GetXamlControlName();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			base.AddCollectionOpenElement(collectionXaml, namespacePrefix, xamlControlName, "OfflineInterval");
			stringBuilder.Append("Range");
			base.AddXamlAttribute("DateRange", this.offlineInterval.dateRange, stringBuilder);
			base.AddXamlAttribute("TextualStartDate", this.offlineInterval.startDate, stringBuilder);
			base.AddXamlAttribute("TextualEndDate", this.offlineInterval.endDate, stringBuilder);
			base.AddXamlAttribute("Number", this.offlineInterval.number, stringBuilder);
			base.AddCollectionOpenElement(collectionXaml, namespacePrefix2, stringBuilder.ToString());
			base.AddCollectionCloseElement(collectionXaml, namespacePrefix2, "Range");
			base.AddCollectionCloseElement(collectionXaml, namespacePrefix, xamlControlName, "OfflineInterval");
		}
		private void AddSecurityPolicies(System.Text.StringBuilder collectionXaml)
		{
			string namespacePrefix = base.GetNamespacePrefix();
			string xamlControlName = this.GetXamlControlName();
			base.AddCollectionOpenElement(collectionXaml, namespacePrefix, xamlControlName, "SecurityPolicies");
			base.AddCollectionOpenElement(collectionXaml, namespacePrefix, "HMIAlarmViewerSecurityPolicies");
			foreach (SecurityPolicy securityPolicy in this.SecurityPolicies)
			{
				collectionXaml.AppendLine(securityPolicy.GetXaml());
			}
			base.AddCollectionCloseElement(collectionXaml, namespacePrefix, "HMIAlarmViewerSecurityPolicies");
			base.AddCollectionCloseElement(collectionXaml, namespacePrefix, xamlControlName, "SecurityPolicies");
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			IsBackgroundVisible = false;
			IsForegroundVisible = false;
			IsTemplateEnabled = false;
		}
		private void UpdateAlarms()
		{
			random = new Random(1);
			AlarmDetails[] array = new AlarmDetails[]
			{
				CreateAlarm(AlarmState.Active, 1, "Group1.SubGroup1", "Type1.SubType1", false),
				CreateAlarm(AlarmState.Active, 2, "Group1.SubGroup2", "Type1.SubType2", false),
				CreateAlarm(AlarmState.Active, 3, "Group2.SubGroup1", "Type2.SubType1", false),
				CreateAlarm(AlarmState.Active, 4, "Group2.SubGroup2", "Type2.SubType2", false),
				CreateAlarm(AlarmState.Active, 5, "Group1.SubGroup1", "Type1.SubType1", true),
				CreateAlarm(AlarmState.Active, 6, "Group1.SubGroup1", "Type1.SubType1", true),
				CreateAlarm(AlarmState.Acknowledged, 7, "Group1.SubGroup1", "Type1.SubType1", false),
				CreateAlarm(AlarmState.Acknowledged, 8, "Group1.SubGroup2", "Type1.SubType2", false),
				CreateAlarm(AlarmState.Acknowledged, 9, "Group2.SubGroup1", "Type2.SubType1", false),
				CreateAlarm(AlarmState.Acknowledged, 10, "Group2.SubGroup2", "Type2.SubType2", false),
				CreateAlarm(AlarmState.Inactive, 11, "Group1.SubGroup1", "Type1.SubType1", false),
				CreateAlarm(AlarmState.Inactive, 12, "Group1.SubGroup2", "Type1.SubType2", false),
				CreateAlarm(AlarmState.Inactive, 13, "Group2.SubGroup1", "Type2.SubType1", false),
				CreateAlarm(AlarmState.Inactive, 14, "Group2.SubGroup2", "Type2.SubType2", false),
				CreateAlarm(AlarmState.AcknowledgedAndGone, 15, "Group1.SubGroup1", "Type1.SubType1", false),
				CreateAlarm(AlarmState.AcknowledgedAndGone, 16, "Group1.SubGroup2", "Type1.SubType2", false),
				CreateAlarm(AlarmState.AcknowledgedAndGone, 17, "Group2.SubGroup1", "Type2.SubType1", false),
				CreateAlarm(AlarmState.AcknowledgedAndGone, 18, "Group2.SubGroup2", "Type2.SubType2", false)
			};
			Alarms.Clear();
			IEnumerable<AlarmDetails> filteredAlarms = GetFilteredAlarms(array);
			Alarms.AddRange(filteredAlarms);
			FilteredActiveCount = Alarms.Count(new Func<AlarmDetails, bool>(IsAlarmActive));
			FilteredActiveOrUnacknowledgedCount = Alarms.Count(new Func<AlarmDetails, bool>(IsAlarmActiveOrUnacknowledged));
			FilteredUnacknowledgedCount = Alarms.Count(new Func<AlarmDetails, bool>(IsAlarmUnacknowledged));
			ActiveCount = array.Count(new Func<AlarmDetails, bool>(IsAlarmActive));
			ActiveOrUnacknowledgedCount = array.Count(new Func<AlarmDetails, bool>(IsAlarmActiveOrUnacknowledged));
			UnacknowledgedCount = array.Count(new Func<AlarmDetails, bool>(IsAlarmUnacknowledged));
		}
		private void DeferredUpdateAlarms()
		{
			UnInitializeUpdateAlarmsTimer();
			InitializeUpdateAlarmsTimer();
			updateAlarmsTimer.Start();
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			UnInitializeUpdateAlarmsTimer();
		}
		private bool IsAlarmUnacknowledged(AlarmDetails alarm)
		{
			return alarm.AlarmType == AlarmState.Inactive || alarm.AlarmType == AlarmState.Active;
		}
		private bool IsAlarmActiveOrUnacknowledged(AlarmDetails alarm)
		{
			return alarm.AlarmType == AlarmState.Active || alarm.AlarmType == AlarmState.Inactive || alarm.AlarmType == AlarmState.Acknowledged;
		}
		private bool IsAlarmActive(AlarmDetails alarm)
		{
			return alarm.AlarmType == AlarmState.Active || alarm.AlarmType == AlarmState.Acknowledged;
		}
		private IEnumerable<AlarmDetails> GetFilteredAlarms(AlarmDetails[] newAlarms)
		{
			IEnumerable<AlarmDetails> enumerable = newAlarms.Where(new Func<AlarmDetails, bool>(IsAlarmVisible)).Take(MaximumCount);
			switch (ServerSortOrder)
			{
			case ServerSortOrder.DateDescending:
				enumerable = 
					from alarm in enumerable
					orderby alarm.DateOn descending, alarm.Priority descending
					select alarm;
				break;
			case ServerSortOrder.PriorityDescending:
				enumerable = 
					from alarm in enumerable
					orderby alarm.Priority descending, alarm.DateOn descending
					select alarm;
				break;
			}
			return enumerable;
		}
		private bool IsAlarmVisible(AlarmDetails alarm)
		{
			return IsAlarmMatchingStatusFilter(alarm) && IsAlarmMatchingPriorityFilter(alarm);
		}
		private bool IsAlarmMatchingPriorityFilter(AlarmDetails alarm)
		{
			return alarm.Priority >= MinimumPriority && alarm.Priority <= MaximumPriority;
		}
		private bool IsAlarmMatchingStatusFilter(AlarmDetails alarm)
		{
			AlarmStatusFilter alarmStatusFilter = AlarmVisibility;
			switch (alarmStatusFilter)
			{
			case AlarmStatusFilter.ActiveOrNotAcknowledged:
				return alarm.AlarmType != AlarmState.AcknowledgedAndGone;
			case (AlarmStatusFilter)1:
			case (AlarmStatusFilter)3:
				break;
			case AlarmStatusFilter.NotAcknowledged:
				return alarm.AlarmType != AlarmState.Acknowledged && alarm.AlarmType != AlarmState.AcknowledgedAndGone;
			case AlarmStatusFilter.Active:
				return alarm.AlarmType == AlarmState.Active;
			default:
				if (alarmStatusFilter == AlarmStatusFilter.Gone)
				{
					return alarm.AlarmType == AlarmState.Inactive || alarm.AlarmType == AlarmState.AcknowledgedAndGone;
				}
				break;
			}
			return true;
		}
		private AlarmDetails CreateAlarm(AlarmState alarmType, int index, string groupName, string typeName, bool hasBadQuality = false)
		{
			DateTime now = DateTime.Now;
			return new AlarmDetails(CultureInfo.CurrentCulture, ShowDateTimeMilliseconds, ShowMultilineTextValues)
			{
				AlarmLogID = Guid.NewGuid(),
				DateOn = now.AddSeconds((double)-(double)index),
				SystemTime = now.AddSeconds((double)-(double)index),
				DateOff = alarmType == AlarmState.Inactive || alarmType == AlarmState.AcknowledgedAndGone ? now.AddSeconds((double)(-(double)index + 2)) : DateTime.MinValue,
				DateAcknowledged = alarmType == AlarmState.Acknowledged || alarmType == AlarmState.AcknowledgedAndGone ? now.AddSeconds((double)(-(double)index + 1)) : DateTime.MinValue,
				AlarmID = Guid.NewGuid(),
				Name = "alarm name " + index.ToString("0#"),
				SignalName = "signal " + index.ToString("0#"),
				AliasName = "alias " + index.ToString("0#"),
				Text = "text " + index.ToString("0#"),
				Group = groupName,
				AlarmGroupSymbolicTextName = groupName,
				Type = typeName,
				AlarmTypeSymbolicTextName = typeName,
				Priority = random.Next(0, 100),
				AcknowledgeComment = alarmType == AlarmState.Acknowledged || alarmType == AlarmState.AcknowledgedAndGone ? "ack comment " + index.ToString("0#") : string.Empty,
				AcknowledgeUser = "user " + index.ToString("0#"),
				HttpLink = "http://www.google.com",
				HelpCause = "this is the cause of the alarm",
				HelpEffect = "this is the effect of the alarm",
				HelpRepair = "this is the repair of the alarm",
				GeneralComment = "This is a general comment",
				OccurrenceComment = "This is a comment for this specific alarm occurrence",
				ExtendedProperty1Text = "extended property 1",
				ExtendedProperty2Text = "extended property 2",
				ExtendedProperty3Text = "extended property 3",
				ExtendedProperty4Text = "extended property 4",
				ExtendedProperty5Text = "extended property 5",
				ExtendedProperty6Text = "extended property 6",
				ExtendedProperty7Text = "extended property 7",
				ExtendedProperty8Text = "extended property 8",
				ExtendedProperty9Text = "extended property 9",
				ExtendedProperty10Text = "extended property 10",
				ExtendedProperty11Text = "extended property 11",
				ExtendedProperty12Text = "extended property 12",
				ExtendedProperty13Text = "extended property 13",
				ExtendedProperty14Text = "extended property 14",
				ExtendedProperty15Text = "extended property 15",
				ExtendedProperty16Text = "extended property 16",
				ExtendedProperty17Text = "extended property 17",
				ExtendedProperty18Text = "extended property 18",
				ExtendedProperty19Text = "extended property 19",
				ExtendedProperty20Text = "extended property 20",
				ExtendedProperty21Text = "extended property 21",
				ExtendedProperty22Text = "extended property 22",
				ExtendedProperty23Text = "extended property 23",
				ExtendedProperty24Text = "extended property 24",
				ExtendedProperty25Text = "extended property 25",
				ExtendedProperty26Text = "extended property 26",
				ExtendedProperty27Text = "extended property 27",
				ExtendedProperty28Text = "extended property 28",
				ExtendedProperty29Text = "extended property 29",
				ExtendedProperty30Text = "extended property 30",
				ExtendedProperty31Text = "extended property 31",
				ExtendedProperty32Text = "extended property 32",
				HasBadQuality = hasBadQuality
			};
		}
		private void UpdateAlarmGroups()
		{
			AlarmGroupItems.Clear();
			AddAlarmGroup("Group1.SubGroup1", "Group1Text.SubGroup1Text");
			AddAlarmGroup("Group1.SubGroup2", "Group1Text.SubGroup2Text");
			AddAlarmGroup("Group2.SubGroup1", "Group2Text.SubGroup1Text");
			AddAlarmGroup("Group2.SubGroup2", "Group2Text.SubGroup2Text");
		}
		private void AddAlarmGroup(string groupName, string translatedGroupName)
		{
			hiddenRootAlarmGroup.AddItem(Guid.NewGuid(), groupName, translatedGroupName);
		}
		public void UpdateAlarmTypes()
		{
			AlarmTypeItems.Clear();
			AddAlarmType("Type1.SubType1", "Type1Text.SubType1Text");
			AddAlarmType("Type1.SubType2", "Type1Text.SubType2Text");
			AddAlarmType("Type2.SubType1", "Type2Text.SubType1Text");
			AddAlarmType("Type2.SubType2", "Type2Text.SubType2Text");
		}
		private void AddAlarmType(string typeName, string translatedTypeName)
		{
			AddAlarmTypeItemsFrom(Guid.NewGuid(), typeName, translatedTypeName);
		}
		private void AddAlarmTypeItemsFrom(Guid typeID, string typeName, string translatedTypeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				return;
			}
			string[] typeNameSegments = typeName.Split(new char[]
			{
				'.'
			});
			string[] translatedTypeNameSegments = translatedTypeName.Split(new char[]
			{
				'.'
			});
			if (typeNameSegments.Length != translatedTypeNameSegments.Length)
			{
				return;
			}
			AlarmTreeItem parent = hiddenRootAlarmType;
			for (int i = 0; i < typeNameSegments.Length; i++)
			{
				AlarmTreeItem alarmTreeItem = AddAlarmTypeItemIfMissing(parent, typeID, typeName, typeNameSegments[i], translatedTypeNameSegments[i]);
				parent = alarmTreeItem;
			}
		}
		private static AlarmTreeItem AddAlarmTypeItemIfMissing(AlarmTreeItem parent, Guid typeID, string typeName, string shortTypeName, string translatedShortTypeName)
		{
			AlarmTreeItem alarmTreeItem = parent.Children.FirstOrDefault((item) => item.ShortName == shortTypeName);
			if (alarmTreeItem == null)
			{
				alarmTreeItem = CreateAlarmTypeItemForParent(parent, typeID, typeName, shortTypeName, translatedShortTypeName);
			}
			return alarmTreeItem;
		}
		private static AlarmTreeItem CreateAlarmTypeItemForParent(AlarmTreeItem parent, Guid typeID, string typeName, string shortTypeName, string translatedShortTypeName)
		{
			AlarmTreeItem alarmTreeItem = new AlarmTreeItem(typeID, shortTypeName, translatedShortTypeName, typeName);
			alarmTreeItem.Parent = parent;
			parent.Children.Add(alarmTreeItem);
			return alarmTreeItem;
		}
		private void AddDefaultColumns()
		{
			if (Columns.Count == 0)
			{
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.Active, Services, Parent));
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.Gone, Services, Parent));
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.Acknowledged, Services, Parent));
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.Text, Services, Parent));
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.Group, Services, Parent));
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.Type, Services, Parent));
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.SignalName, Services, Parent));
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.OpcItem, Services, Parent));
				Columns.Add(new AlarmColumnSettings(AlarmColumnTypes.Priority, Services, Parent));
			}
		}
	}
}
