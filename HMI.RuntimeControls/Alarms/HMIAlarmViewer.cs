// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using HMI.Runtime.Communication;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace HMI.RuntimeControls.Alarms;

public class HMIAlarmViewer : Control
{
    private readonly ObservableCollection<AlarmHistoryEvent> alarms = [];
    private IReadOnlyList<AlarmHistoryEvent> queriedAlarms = [];
    private readonly DispatcherTimer refreshTimer;
    private RadGridView? alarmGrid;
    private DatePicker? startDatePicker;
    private DatePicker? endDatePicker;
    private StackPanel? textFilterPanel;
    private TextBox? textFilterBox;
    private StackPanel? alarmVisibilityPanel;
    private ComboBox? alarmVisibilitySelector;
    private StackPanel? groupFilterPanel;
    private ListBox? groupFilterList;
    private StackPanel? typeFilterPanel;
    private ListBox? typeFilterList;
    private StackPanel? priorityFilterPanel;
    private TextBox? minimumPriorityBox;
    private TextBox? maximumPriorityBox;
    private StackPanel? maximumCountPanel;
    private TextBox? maximumCountBox;
    private StackPanel? sortOrderPanel;
    private ComboBox? sortOrderSelector;
    private Button? refreshButton;
    private TextBlock? statusTextBlock;
    private CancellationTokenSource? lifetimeCancellationSource;
    private IHMIConnector? connector;
    private bool isRefreshing;
    private bool isSynchronizingGroupFilter;
    private bool isSynchronizingTypeFilter;

    public static readonly DependencyProperty RefreshIntervalProperty =
        DependencyProperty.Register(nameof(RefreshInterval), typeof(int), typeof(HMIAlarmViewer), new PropertyMetadata(30, OnRefreshIntervalChanged));
    public static readonly DependencyProperty StatusTextProperty =
        DependencyProperty.Register(nameof(StatusText), typeof(string), typeof(HMIAlarmViewer), new PropertyMetadata("选择时间范围并刷新历史报警"));

    static HMIAlarmViewer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmViewer), new FrameworkPropertyMetadata(typeof(HMIAlarmViewer)));
    }

    public HMIAlarmViewer()
    {
        refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(RefreshInterval) };
        refreshTimer.Tick += RefreshTimer_Tick;
        Loaded += HMIAlarmViewer_Loaded;
        Unloaded += HMIAlarmViewer_Unloaded;
    }

    public int RefreshInterval
    {
        get => (int)GetValue(RefreshIntervalProperty);
        set => SetValue(RefreshIntervalProperty, value);
    }

    public string StatusText
    {
        get => (string)GetValue(StatusTextProperty);
        private set => SetValue(StatusTextProperty, value);
    }

    public string ServerName { get; set; } = string.Empty;
    public string HornPath { get; set; } = string.Empty;
    public bool Loop { get; set; }
    public bool WriteSecure { get; set; }
    public bool EnableNavigation { get; set; }
    public bool EnablePopup { get; set; }
    public bool ShowAlarmGroupsFilterTree { get; set; } = true;
    public bool ShowAlarmTypesFilterTree { get; set; }
    public bool ShowStatusBar { get; set; } = true;
    public bool ShowHeader { get; set; } = true;
    public bool ShowScrollBars { get; set; } = true;
    public bool ShowDateTimeMilliseconds { get; set; }
    public bool ShowMultilineTextValues { get; set; }
    public bool ShowTotalAlarmCount { get; set; } = true;
    public bool ShowActiveAlarmCount { get; set; } = true;
    public bool ShowUnacknowledgedAlarmCount { get; set; } = true;
    public AlarmCountDisplayMode AlarmCountDisplayMode { get; set; }
    public bool Offline { get; set; }
    public bool ShowGroupsInRuntimeFilter { get; set; } = true;
    public string GroupsFilter { get; set; } = "*";
    public bool FilterGroupsByUser { get; set; }
    public bool ShowTypesInRuntimeFilter { get; set; }
    public string TypesFilter { get; set; } = "*";
    public bool ShowMaximumCountInRuntimeFilter { get; set; } = true;
    public int MaximumCount { get; set; } = 1000;
    public bool ShowServerSortOrderInRuntimeFilter { get; set; }
    public ServerSortOrder ServerSortOrder { get; set; }
    public bool ShowAlarmVisibilityInRuntimeFilter { get; set; } = true;
    public AlarmStatusFilter AlarmVisibility { get; set; } = AlarmStatusFilter.All;
    public bool ShowPriorityInRuntimeFilter { get; set; } = true;
    public int MinimumPriority { get; set; } = -1;
    public int MaximumPriority { get; set; } = -1;
    public bool ShowTextFilterInRuntimeFilter { get; set; } = true;
    public FilterColumnType TextFilterColumn { get; set; }
    public string TextFilter { get; set; } = string.Empty;
    public Brush ActiveAlarmBackground { get; set; } = Brushes.Tomato;
    public Brush ActiveAlarmForeground { get; set; } = Brushes.Black;
    public Brush InactiveAlarmBackground { get; set; } = Brushes.LightGreen;
    public Brush InactiveAlarmForeground { get; set; } = Brushes.Black;
    public Brush AcknowledgedAlarmBackground { get; set; } = Brushes.LightYellow;
    public Brush AcknowledgedAlarmForeground { get; set; } = Brushes.Black;
    public Brush AcknowledgedAndGoneAlarmBackground { get; set; } = Brushes.YellowGreen;
    public Brush AcknowledgedAndGoneAlarmForeground { get; set; } = Brushes.Black;
    public Brush BadQualityAlarmBackground { get; set; } = Brushes.LightGray;
    public Brush BadQualityAlarmForeground { get; set; } = Brushes.Black;
    public ObservableCollection<ColumnSettings> Columns { get; } = [];
    public ObservableCollection<RowColor> RowColorDefinitions { get; } = [];
    public ObservableCollection<PriorityColor> PriorityColorDefinitions { get; } = [];
    public ObservableCollection<HMI.Runtime.Shared.Range> OfflineInterval { get; } = [];
    public HMIAlarmViewerSecurityPolicies SecurityPolicies { get; set; } = new();
    public ObservableCollection<AlarmHistoryEvent> Alarms => alarms;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (alarmGrid is not null)
        {
            alarmGrid.ItemsSource = null;
        }
        if (refreshButton is not null)
        {
            refreshButton.Click -= RefreshButton_Click;
        }
        if (textFilterBox is not null)
        {
            textFilterBox.TextChanged -= TextFilterBox_TextChanged;
        }
        if (alarmVisibilitySelector is not null)
        {
            alarmVisibilitySelector.SelectionChanged -= AlarmVisibilitySelector_SelectionChanged;
        }
        if (groupFilterList is not null)
        {
            groupFilterList.SelectionChanged -= GroupFilterList_SelectionChanged;
        }
        if (typeFilterList is not null)
        {
            typeFilterList.SelectionChanged -= TypeFilterList_SelectionChanged;
        }
        if (sortOrderSelector is not null)
        {
            sortOrderSelector.SelectionChanged -= SortOrderSelector_SelectionChanged;
        }
        if (minimumPriorityBox is not null)
        {
            minimumPriorityBox.LostFocus -= PriorityFilterBox_LostFocus;
        }
        if (maximumPriorityBox is not null)
        {
            maximumPriorityBox.LostFocus -= PriorityFilterBox_LostFocus;
        }
        if (maximumCountBox is not null)
        {
            maximumCountBox.LostFocus -= MaximumCountBox_LostFocus;
        }

        alarmGrid = GetTemplateChild("PART_AlarmGrid") as RadGridView;
        startDatePicker = GetTemplateChild("PART_StartDatePicker") as DatePicker;
        endDatePicker = GetTemplateChild("PART_EndDatePicker") as DatePicker;
        textFilterPanel = GetTemplateChild("PART_TextFilterPanel") as StackPanel;
        textFilterBox = GetTemplateChild("PART_TextFilterBox") as TextBox;
        alarmVisibilityPanel = GetTemplateChild("PART_AlarmVisibilityPanel") as StackPanel;
        alarmVisibilitySelector = GetTemplateChild("PART_AlarmVisibilitySelector") as ComboBox;
        groupFilterPanel = GetTemplateChild("PART_GroupFilterPanel") as StackPanel;
        groupFilterList = GetTemplateChild("PART_GroupFilterList") as ListBox;
        typeFilterPanel = GetTemplateChild("PART_TypeFilterPanel") as StackPanel;
        typeFilterList = GetTemplateChild("PART_TypeFilterList") as ListBox;
        priorityFilterPanel = GetTemplateChild("PART_PriorityFilterPanel") as StackPanel;
        minimumPriorityBox = GetTemplateChild("PART_MinimumPriorityBox") as TextBox;
        maximumPriorityBox = GetTemplateChild("PART_MaximumPriorityBox") as TextBox;
        maximumCountPanel = GetTemplateChild("PART_MaximumCountPanel") as StackPanel;
        maximumCountBox = GetTemplateChild("PART_MaximumCountBox") as TextBox;
        sortOrderPanel = GetTemplateChild("PART_SortOrderPanel") as StackPanel;
        sortOrderSelector = GetTemplateChild("PART_SortOrderSelector") as ComboBox;
        refreshButton = GetTemplateChild("PART_RefreshButton") as Button;
        statusTextBlock = GetTemplateChild("PART_StatusText") as TextBlock;
        if (alarmGrid is not null)
        {
            alarmGrid.ItemsSource = alarms;
            alarmGrid.ShowColumnHeaders = ShowHeader;
            ConfigureColumns();
        }
        if (refreshButton is not null)
        {
            refreshButton.Click += RefreshButton_Click;
        }
        if (textFilterBox is not null)
        {
            textFilterBox.Text = TextFilter;
            textFilterBox.TextChanged += TextFilterBox_TextChanged;
        }
        if (textFilterPanel is not null)
        {
            textFilterPanel.Visibility = ShowTextFilterInRuntimeFilter ? Visibility.Visible : Visibility.Collapsed;
        }
        if (alarmVisibilityPanel is not null)
        {
            alarmVisibilityPanel.Visibility = ShowAlarmVisibilityInRuntimeFilter ? Visibility.Visible : Visibility.Collapsed;
        }
        if (alarmVisibilitySelector is not null)
        {
            alarmVisibilitySelector.ItemsSource = new[]
            {
                new FilterOption<AlarmStatusFilter>("全部事件", AlarmStatusFilter.All),
                new FilterOption<AlarmStatusFilter>("仅未确认", AlarmStatusFilter.NotAcknowledged)
            };
            alarmVisibilitySelector.SelectedValuePath = nameof(FilterOption<AlarmStatusFilter>.Value);
            alarmVisibilitySelector.DisplayMemberPath = nameof(FilterOption<AlarmStatusFilter>.Label);
            alarmVisibilitySelector.SelectedValue = AlarmVisibility;
            alarmVisibilitySelector.SelectionChanged += AlarmVisibilitySelector_SelectionChanged;
        }
        if (groupFilterPanel is not null)
        {
            groupFilterPanel.Visibility = ShowGroupsInRuntimeFilter || ShowAlarmGroupsFilterTree
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        if (groupFilterList is not null)
        {
            groupFilterList.SelectionChanged += GroupFilterList_SelectionChanged;
        }
        if (typeFilterPanel is not null)
        {
            typeFilterPanel.Visibility = ShowTypesInRuntimeFilter || ShowAlarmTypesFilterTree
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        if (typeFilterList is not null)
        {
            typeFilterList.SelectionChanged += TypeFilterList_SelectionChanged;
        }
        if (priorityFilterPanel is not null)
        {
            priorityFilterPanel.Visibility = ShowPriorityInRuntimeFilter ? Visibility.Visible : Visibility.Collapsed;
        }
        if (minimumPriorityBox is not null)
        {
            minimumPriorityBox.Text = MinimumPriority.ToString(System.Globalization.CultureInfo.InvariantCulture);
            minimumPriorityBox.LostFocus += PriorityFilterBox_LostFocus;
        }
        if (maximumPriorityBox is not null)
        {
            maximumPriorityBox.Text = MaximumPriority.ToString(System.Globalization.CultureInfo.InvariantCulture);
            maximumPriorityBox.LostFocus += PriorityFilterBox_LostFocus;
        }
        if (maximumCountPanel is not null)
        {
            maximumCountPanel.Visibility = ShowMaximumCountInRuntimeFilter ? Visibility.Visible : Visibility.Collapsed;
        }
        if (maximumCountBox is not null)
        {
            maximumCountBox.Text = MaximumCount.ToString(System.Globalization.CultureInfo.InvariantCulture);
            maximumCountBox.LostFocus += MaximumCountBox_LostFocus;
        }
        if (sortOrderPanel is not null)
        {
            sortOrderPanel.Visibility = ShowServerSortOrderInRuntimeFilter ? Visibility.Visible : Visibility.Collapsed;
        }
        if (sortOrderSelector is not null)
        {
            sortOrderSelector.ItemsSource = new[]
            {
                new FilterOption<ServerSortOrder>("时间降序", ServerSortOrder.DateDescending),
                new FilterOption<ServerSortOrder>("优先级降序", ServerSortOrder.PriorityDescending)
            };
            sortOrderSelector.SelectedValuePath = nameof(FilterOption<ServerSortOrder>.Value);
            sortOrderSelector.DisplayMemberPath = nameof(FilterOption<ServerSortOrder>.Label);
            sortOrderSelector.SelectedValue = ServerSortOrder;
            sortOrderSelector.SelectionChanged += SortOrderSelector_SelectionChanged;
        }
        if (statusTextBlock is not null)
        {
            statusTextBlock.Visibility = ShowStatusBar ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private static void OnRefreshIntervalChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
    {
        HMIAlarmViewer viewer = (HMIAlarmViewer)target;
        int interval = (int)args.NewValue;
        if (interval < 1)
        {
            viewer.SetCurrentValue(RefreshIntervalProperty, args.OldValue);
            return;
        }
        viewer.refreshTimer.Interval = TimeSpan.FromSeconds(interval);
    }

    private async void HMIAlarmViewer_Loaded(object sender, RoutedEventArgs args)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }
        lifetimeCancellationSource?.Cancel();
        lifetimeCancellationSource?.Dispose();
        CancellationTokenSource loadCancellationSource = new();
        lifetimeCancellationSource = loadCancellationSource;
        connector = RuntimeServices.GetRequiredService<IHMIConnector>();
        await LoadAlarmFilterMetadataAsync(loadCancellationSource.Token);
        if (loadCancellationSource.IsCancellationRequested || lifetimeCancellationSource != loadCancellationSource)
        {
            return;
        }
        DateTime today = DateTime.Today;
        if (startDatePicker?.SelectedDate is null)
        {
            startDatePicker?.SetCurrentValue(DatePicker.SelectedDateProperty, today.AddDays(-1));
        }
        if (endDatePicker?.SelectedDate is null)
        {
            endDatePicker?.SetCurrentValue(DatePicker.SelectedDateProperty, today);
        }
        refreshTimer.Start();
        _ = RefreshAlarmsAsync(lifetimeCancellationSource.Token);
    }

    private void HMIAlarmViewer_Unloaded(object sender, RoutedEventArgs args)
    {
        refreshTimer.Stop();
        lifetimeCancellationSource?.Cancel();
        lifetimeCancellationSource?.Dispose();
        lifetimeCancellationSource = null;
    }

    private void RefreshTimer_Tick(object? sender, EventArgs args)
    {
        if (lifetimeCancellationSource is { IsCancellationRequested: false } source)
        {
            _ = RefreshAlarmsAsync(source.Token);
        }
    }

    private async void RefreshButton_Click(object sender, RoutedEventArgs args)
    {
        if (lifetimeCancellationSource is { IsCancellationRequested: false } source)
        {
            await RefreshAlarmsAsync(source.Token);
        }
    }

    private async Task RefreshAlarmsAsync(CancellationToken cancellationToken)
    {
        if (isRefreshing || connector is null || startDatePicker?.SelectedDate is not DateTime startDate ||
            endDatePicker?.SelectedDate is not DateTime endDate)
        {
            if (startDatePicker?.SelectedDate is null || endDatePicker?.SelectedDate is null)
            {
                StatusText = "请选择有效的开始和结束日期。";
            }
            return;
        }

        DateTimeOffset startTime = new DateTimeOffset(startDate.Date);
        DateTimeOffset endTime = new DateTimeOffset(endDate.Date.AddDays(1).AddTicks(-1));
        if (startTime > endTime)
        {
            StatusText = "历史报警的开始日期不能晚于结束日期。";
            return;
        }

        isRefreshing = true;
        if (refreshButton is not null)
        {
            refreshButton.IsEnabled = false;
        }
        try
        {
            IReadOnlyList<AlarmHistoryEvent> result = await connector.GetAlarmHistoryAsync(
                ServerName,
                startTime,
                endTime,
                cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            queriedAlarms = result;
            UpdateFilteredAlarms();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Alarm history query failed: {0}",
                exception);
            StatusText = $"历史报警查询失败：{exception.GetBaseException().Message}";
        }
        finally
        {
            isRefreshing = false;
            if (refreshButton is not null)
            {
                refreshButton.IsEnabled = true;
            }
        }
    }

    private string BuildStatusText(int visibleCount, int filteredCount, int totalCount, int unacknowledgedCount)
    {
        List<string> notices = [$"历史报警：显示 {visibleCount} 条"];
        if (filteredCount != visibleCount)
        {
            notices.Add($"筛选后 {filteredCount} 条");
        }
        if (ShowTotalAlarmCount)
        {
            notices.Add($"查询总数 {totalCount} 条");
        }
        if (ShowUnacknowledgedAlarmCount)
        {
            notices.Add($"筛选后未确认 {unacknowledgedCount} 条");
        }
        notices.Add("确认状态来自事件记录，非当前活动状态");
        if (AlarmVisibility is AlarmStatusFilter.Active or AlarmStatusFilter.Gone or AlarmStatusFilter.ActiveOrNotAcknowledged)
        {
            notices.Add("活动/恢复状态筛选尚不支持");
        }
        if (FilterGroupsByUser)
        {
            notices.Add("用户组筛选尚不支持");
        }
        if (TextFilterColumn is not (FilterColumnType.None or FilterColumnType.Text or FilterColumnType.SignalName or FilterColumnType.Name or FilterColumnType.OpcItem))
        {
            notices.Add("所选文本筛选列尚不支持，筛选范围为点位和事件描述");
        }
        if (EnableNavigation || EnablePopup || !string.IsNullOrWhiteSpace(HornPath))
        {
            notices.Add("导航、详情弹窗和报警声音尚未迁移");
        }
        if (HasConfiguredSecurityPolicies())
        {
            notices.Add("报警查看器的细分安全策略尚未迁移");
        }
        if (Columns.Any(column => column.IsVisible && CreateColumnDefinition(column.Type) is null))
        {
            notices.Add("部分列类型尚不支持");
        }
        if (RowColorDefinitions.Count > 0 || PriorityColorDefinitions.Count > 0)
        {
            notices.Add("旧版报警状态颜色尚未迁移");
        }
        if (Offline)
        {
            notices.Add("离线查询模式尚未迁移");
        }
        if (ShowActiveAlarmCount)
        {
            notices.Add("当前活动报警计数尚未迁移，历史记录不能推断活动状态");
        }
        if (ShowMultilineTextValues)
        {
            notices.Add("多行文本显示设置尚未迁移");
        }
        return string.Join("；", notices);
    }

    private bool HasConfiguredSecurityPolicies() =>
        SecurityPolicies.Help.Count > 0 ||
        SecurityPolicies.Refresh.Count > 0 ||
        SecurityPolicies.Filter.Count > 0 ||
        SecurityPolicies.Acknowledge.Count > 0 ||
        SecurityPolicies.Comments.Count > 0 ||
        SecurityPolicies.ProcessingAndDisplay.Count > 0 ||
        SecurityPolicies.SoundToggle.Count > 0 ||
        SecurityPolicies.Navigate.Count > 0 ||
        SecurityPolicies.OnlineMode.Count > 0 ||
        SecurityPolicies.OfflineMode.Count > 0 ||
        SecurityPolicies.CustomQueryMode.Count > 0 ||
        SecurityPolicies.HttpLink.Count > 0;

    private void UpdateFilteredAlarms()
    {
        IEnumerable<AlarmHistoryEvent> filtered = queriedAlarms;
        filtered = filtered.Where(alarm =>
            MatchesPathFilter(alarm.AlarmGroupName, GroupsFilter) &&
            MatchesPathFilter(alarm.AlarmTypeName, TypesFilter));
        if (MinimumPriority >= 0)
        {
            filtered = filtered.Where(alarm => alarm.Severity >= MinimumPriority);
        }
        if (MaximumPriority >= 0)
        {
            filtered = filtered.Where(alarm => alarm.Severity <= MaximumPriority);
        }
        if (AlarmVisibility == AlarmStatusFilter.NotAcknowledged)
        {
            filtered = filtered.Where(alarm => !alarm.Acknowledged);
        }
        if (!string.IsNullOrWhiteSpace(textFilterBox?.Text))
        {
            string text = textFilterBox.Text.Trim();
            filtered = filtered.Where(alarm => MatchesTextFilter(alarm, text));
        }
        filtered = ServerSortOrder == ServerSortOrder.PriorityDescending
            ? filtered.OrderByDescending(alarm => alarm.Severity).ThenByDescending(alarm => alarm.Timestamp)
            : filtered.OrderByDescending(alarm => alarm.Timestamp);

        AlarmHistoryEvent[] filteredResults = filtered.ToArray();
        AlarmHistoryEvent[] snapshot = filteredResults.Take(Math.Max(1, MaximumCount)).ToArray();
        alarms.Clear();
        foreach (AlarmHistoryEvent alarm in snapshot)
        {
            alarms.Add(alarm);
        }
        StatusText = BuildStatusText(
            snapshot.Length,
            filteredResults.Length,
            queriedAlarms.Count,
            filteredResults.Count(alarm => !alarm.Acknowledged));
    }

    private bool MatchesTextFilter(AlarmHistoryEvent alarm, string text)
    {
        bool matchesChannel = alarm.ChannelName.Contains(text, StringComparison.CurrentCultureIgnoreCase);
        bool matchesMessage = alarm.Message.Contains(text, StringComparison.CurrentCultureIgnoreCase);
        return TextFilterColumn switch
        {
            FilterColumnType.Text => matchesMessage,
            FilterColumnType.SignalName or FilterColumnType.Name or FilterColumnType.OpcItem => matchesChannel,
            _ => matchesChannel || matchesMessage
        };
    }

    private static bool MatchesPathFilter(string value, string filter)
    {
        string[] patterns = filter.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return patterns.Any(pattern =>
            pattern == "*" ||
            string.Equals(value, pattern, StringComparison.OrdinalIgnoreCase) ||
            (pattern.EndsWith(".*", StringComparison.Ordinal) &&
             (string.Equals(value, pattern[..^2], StringComparison.OrdinalIgnoreCase) ||
              value.StartsWith(pattern[..^1], StringComparison.OrdinalIgnoreCase))));
    }

    private async Task LoadAlarmFilterMetadataAsync(CancellationToken cancellationToken)
    {
        if (connector is null || (groupFilterList is null && typeFilterList is null))
        {
            return;
        }

        try
        {
            AlarmFilterMetadata metadata = await connector.GetAlarmFilterMetadataAsync(ServerName, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (groupFilterList is not null)
            {
                isSynchronizingGroupFilter = true;
                groupFilterList.ItemsSource = metadata.GroupNames;
                SelectConfiguredFilters(groupFilterList, GroupsFilter);
                isSynchronizingGroupFilter = false;
            }
            if (typeFilterList is not null)
            {
                isSynchronizingTypeFilter = true;
                typeFilterList.ItemsSource = metadata.TypeNames;
                SelectConfiguredFilters(typeFilterList, TypesFilter);
                isSynchronizingTypeFilter = false;
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Alarm filter metadata query failed: {0}",
                exception);
            StatusText = $"报警组/类型加载失败：{exception.GetBaseException().Message}";
        }
        finally
        {
            isSynchronizingGroupFilter = false;
            isSynchronizingTypeFilter = false;
        }
    }

    private static void SelectConfiguredFilters(ListBox listBox, string filter)
    {
        listBox.SelectedItems.Clear();
        string[] configured = filter.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (configured.Contains("*", StringComparer.Ordinal))
        {
            foreach (object item in listBox.Items)
            {
                listBox.SelectedItems.Add(item);
            }
            return;
        }
        foreach (object item in listBox.Items)
        {
            if (item is string name && MatchesPathFilter(name, filter))
            {
                listBox.SelectedItems.Add(item);
            }
        }
    }

    private void GroupFilterList_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (isSynchronizingGroupFilter || groupFilterList is null)
        {
            return;
        }
        GroupsFilter = string.Join(";", groupFilterList.SelectedItems.Cast<string>());
        UpdateFilteredAlarms();
    }

    private void TypeFilterList_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (isSynchronizingTypeFilter || typeFilterList is null)
        {
            return;
        }
        TypesFilter = string.Join(";", typeFilterList.SelectedItems.Cast<string>());
        UpdateFilteredAlarms();
    }

    private void ConfigureColumns()
    {
        if (alarmGrid is null)
        {
            return;
        }

        alarmGrid.Columns.Clear();
        IEnumerable<(string Type, string Header, string Property)> definitions = Columns.Count > 0
            ? Columns.Where(column => column.IsVisible)
                .Select(column => CreateColumnDefinition(column.Type))
                .Where(definition => definition.HasValue)
                .Select(definition => definition!.Value)
            :
            [
                ("SystemTime", "时间", nameof(AlarmHistoryEvent.Timestamp)),
                ("Name", "点位", nameof(AlarmHistoryEvent.ChannelName)),
                ("Group", "报警组", nameof(AlarmHistoryEvent.AlarmGroupName)),
                ("Type", "报警类型", nameof(AlarmHistoryEvent.AlarmTypeName)),
                ("Text", "事件描述", nameof(AlarmHistoryEvent.Message)),
                ("Priority", "级别", nameof(AlarmHistoryEvent.Severity)),
                ("Acknowledged", "已确认", nameof(AlarmHistoryEvent.Acknowledged)),
                ("AcknowledgedAt", "确认时间", nameof(AlarmHistoryEvent.AcknowledgedAt))
            ];

        foreach ((string type, string header, string property) in definitions)
        {
            ColumnSettings? setting = Columns.FirstOrDefault(column => column.Type == type);
            var column = new GridViewDataColumn
            {
                Header = header,
                DataMemberBinding = new Binding(property),
                IsReadOnly = true,
                IsFilterable = setting?.IsFilterable ?? true
            };
            if (property is nameof(AlarmHistoryEvent.Timestamp) or nameof(AlarmHistoryEvent.AcknowledgedAt))
            {
                column.DataFormatString = ShowDateTimeMilliseconds ? "{0:yyyy-MM-dd HH:mm:ss.fff}" : "{0:yyyy-MM-dd HH:mm:ss}";
            }
            if (setting is { IsAutoSize: false, Width: > 0 })
            {
                column.Width = new GridViewLength(setting.Width);
            }
            alarmGrid.Columns.Add(column);
        }
        alarmGrid.ShowColumnHeaders = ShowHeader;
        ScrollViewer.SetVerticalScrollBarVisibility(alarmGrid, ShowScrollBars ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled);
        ScrollViewer.SetHorizontalScrollBarVisibility(alarmGrid, ShowScrollBars ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled);
    }

    private static (string Type, string Header, string Property)? CreateColumnDefinition(string type) =>
        type switch
        {
            "Active" or "SystemTime" => (type, "时间", nameof(AlarmHistoryEvent.Timestamp)),
            "Name" or "SignalName" or "OpcItem" => (type, "点位", nameof(AlarmHistoryEvent.ChannelName)),
            "Text" => (type, "事件描述", nameof(AlarmHistoryEvent.Message)),
            "Group" => (type, "报警组", nameof(AlarmHistoryEvent.AlarmGroupName)),
            "Type" => (type, "报警类型", nameof(AlarmHistoryEvent.AlarmTypeName)),
            "Priority" => (type, "级别", nameof(AlarmHistoryEvent.Severity)),
            "Acknowledged" => (type, "已确认", nameof(AlarmHistoryEvent.Acknowledged)),
            "Gone" => (type, "确认时间", nameof(AlarmHistoryEvent.AcknowledgedAt)),
            _ => null
        };

    private void TextFilterBox_TextChanged(object sender, TextChangedEventArgs args)
    {
        TextFilter = textFilterBox?.Text ?? string.Empty;
        UpdateFilteredAlarms();
    }

    private void AlarmVisibilitySelector_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (alarmVisibilitySelector?.SelectedValue is AlarmStatusFilter selection)
        {
            AlarmVisibility = selection;
            UpdateFilteredAlarms();
        }
    }

    private void SortOrderSelector_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (sortOrderSelector?.SelectedValue is ServerSortOrder selection)
        {
            ServerSortOrder = selection;
            UpdateFilteredAlarms();
        }
    }

    private void PriorityFilterBox_LostFocus(object sender, RoutedEventArgs args)
    {
        if (!TryParseOptionalInteger(minimumPriorityBox?.Text, out int minimum) ||
            !TryParseOptionalInteger(maximumPriorityBox?.Text, out int maximum))
        {
            StatusText = "优先级筛选请输入整数；使用 -1 表示不限制。";
            return;
        }
        if (minimum >= 0 && maximum >= 0 && minimum > maximum)
        {
            StatusText = "最低优先级不能大于最高优先级。";
            return;
        }

        MinimumPriority = minimum;
        MaximumPriority = maximum;
        UpdateFilteredAlarms();
    }

    private void MaximumCountBox_LostFocus(object sender, RoutedEventArgs args)
    {
        if (!int.TryParse(maximumCountBox?.Text, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture, out int maximumCount) ||
            maximumCount is < 1 or > 9999)
        {
            StatusText = "最大显示条数必须是 1 到 9999 之间的整数。";
            return;
        }

        MaximumCount = maximumCount;
        UpdateFilteredAlarms();
    }

    private static bool TryParseOptionalInteger(string? text, out int value) =>
        int.TryParse(text, System.Globalization.NumberStyles.Integer,
            System.Globalization.CultureInfo.InvariantCulture, out value) &&
        value >= -1;

    private sealed record FilterOption<T>(string Label, T Value);
}
