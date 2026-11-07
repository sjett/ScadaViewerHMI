// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using HMI.Runtime.Communication;
using HMI.Runtime.Core;
using HMI.RuntimeControls.Alarms.Common;
using Telerik.Windows.Controls;

namespace HMI.RuntimeControls.Alarms;

public enum AlarmVisibility
{
    ActiveOrNotAcknowledged,
    NotAcknowledged = 2,
    Active = 4,
    All = 8
}

public class HMIAlarmOnline : Control
{
    private static readonly TimeSpan QueryWindow = TimeSpan.FromHours(24);
    private readonly ObservableCollection<UnacknowledgedEvent> alarms = [];
    private readonly HashSet<Guid> previousAlarmIds = [];
    private readonly DispatcherTimer refreshTimer;
    private RadGridView? alarmGrid;
    private Button? refreshButton;
    private Button? acknowledgeButton;
    private MediaElement? hornMediaElement;
    private CancellationTokenSource? lifetimeCancellationSource;
    private IHMIConnector? connector;
    private bool hasInitialSnapshot;
    private bool isRefreshing;
    private bool isAcknowledging;
    private Guid? playingAlarmId;

    public static readonly DependencyProperty ServerNameProperty =
        DependencyProperty.Register(nameof(ServerName), typeof(string), typeof(HMIAlarmOnline), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty RefreshIntervalProperty =
        DependencyProperty.Register(nameof(RefreshInterval), typeof(int), typeof(HMIAlarmOnline), new PropertyMetadata(5, OnRefreshIntervalChanged));
    public static readonly DependencyProperty MaxAlarmRowsProperty =
        DependencyProperty.Register(nameof(MaxAlarmRows), typeof(int), typeof(HMIAlarmOnline), new PropertyMetadata(100));
    public static readonly DependencyProperty HornPathProperty =
        DependencyProperty.Register(nameof(HornPath), typeof(string), typeof(HMIAlarmOnline), new PropertyMetadata(string.Empty, OnHornPathChanged));
    public static readonly DependencyProperty LoopProperty =
        DependencyProperty.Register(nameof(Loop), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(false));
    public static readonly DependencyProperty ShowDateTimeMillisecondsProperty =
        DependencyProperty.Register(nameof(ShowDateTimeMilliseconds), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(false));
    public static readonly DependencyProperty ShowHeaderProperty =
        DependencyProperty.Register(nameof(ShowHeader), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(true));
    public static readonly DependencyProperty ShowScrollBarsProperty =
        DependencyProperty.Register(nameof(ShowScrollBars), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(true));
    public static readonly DependencyProperty FilterProperty =
        DependencyProperty.Register(nameof(Filter), typeof(string), typeof(HMIAlarmOnline), new PropertyMetadata(nameof(AlarmVisibility.All)));
    public static readonly DependencyProperty MinimumPriorityProperty =
        DependencyProperty.Register(nameof(MinimumPriority), typeof(int), typeof(HMIAlarmOnline), new PropertyMetadata(-1));
    public static readonly DependencyProperty MaximumPriorityProperty =
        DependencyProperty.Register(nameof(MaximumPriority), typeof(int), typeof(HMIAlarmOnline), new PropertyMetadata(-1));
    public static readonly DependencyProperty ServerSortOrderProperty =
        DependencyProperty.Register(nameof(ServerSortOrder), typeof(string), typeof(HMIAlarmOnline), new PropertyMetadata("DateDescending"));
    public static readonly DependencyProperty ActiveAlarmBackgroundProperty =
        DependencyProperty.Register(nameof(ActiveAlarmBackground), typeof(Brush), typeof(HMIAlarmOnline), new PropertyMetadata(Brushes.Tomato));
    public static readonly DependencyProperty ActiveAlarmForegroundProperty =
        DependencyProperty.Register(nameof(ActiveAlarmForeground), typeof(Brush), typeof(HMIAlarmOnline), new PropertyMetadata(Brushes.Black));
    public static readonly DependencyProperty InactiveAlarmBackgroundProperty =
        DependencyProperty.Register(nameof(InactiveAlarmBackground), typeof(Brush), typeof(HMIAlarmOnline), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 204, 102))));
    public static readonly DependencyProperty InactiveAlarmForegroundProperty =
        DependencyProperty.Register(nameof(InactiveAlarmForeground), typeof(Brush), typeof(HMIAlarmOnline), new PropertyMetadata(Brushes.Black));
    public static readonly DependencyProperty AcknowledgedAlarmBackgroundProperty =
        DependencyProperty.Register(nameof(AcknowledgedAlarmBackground), typeof(Brush), typeof(HMIAlarmOnline), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 153))));
    public static readonly DependencyProperty AcknowledgedAlarmForegroundProperty =
        DependencyProperty.Register(nameof(AcknowledgedAlarmForeground), typeof(Brush), typeof(HMIAlarmOnline), new PropertyMetadata(Brushes.Black));
    public static readonly DependencyProperty AcknowledgedAndGoneAlarmBackgroundProperty =
        DependencyProperty.Register(nameof(AcknowledgedAndGoneAlarmBackground), typeof(Brush), typeof(HMIAlarmOnline), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(154, 205, 50))));
    public static readonly DependencyProperty AcknowledgedAndGoneAlarmForegroundProperty =
        DependencyProperty.Register(nameof(AcknowledgedAndGoneAlarmForeground), typeof(Brush), typeof(HMIAlarmOnline), new PropertyMetadata(Brushes.Black));
    public static readonly DependencyProperty FilterAlarmGroupsByUserProperty =
        DependencyProperty.Register(nameof(FilterAlarmGroupsByUser), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(false));
    public static readonly DependencyProperty EnablePopupProperty =
        DependencyProperty.Register(nameof(EnablePopup), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(true));
    public static readonly DependencyProperty WriteSecureProperty =
        DependencyProperty.Register(nameof(WriteSecure), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(false));
    public static readonly DependencyProperty UseColumnWidthsPropertiesProperty =
        DependencyProperty.Register(nameof(UseColumnWidthsProperties), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(false));
    public static readonly DependencyProperty AutoSizeColumnsProperty =
        DependencyProperty.Register(nameof(AutoSizeColumns), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(true));
    public static readonly DependencyProperty UseColumnOrderPropertiesProperty =
        DependencyProperty.Register(nameof(UseColumnOrderProperties), typeof(bool), typeof(HMIAlarmOnline), new PropertyMetadata(false));
    public static readonly DependencyProperty ActiveWidthProperty = RegisterDouble("ActiveWidth", 140);
    public static readonly DependencyProperty SystemTimeWidthProperty = RegisterDouble("SystemTimeWidth", 0);
    public static readonly DependencyProperty GoneWidthProperty = RegisterDouble("GoneWidth", 140);
    public static readonly DependencyProperty AcknowledgedWidthProperty = RegisterDouble("AcknowledgedWidth", 140);
    public static readonly DependencyProperty AcknowledgedWithCommentWidthProperty = RegisterDouble("AcknowledgedWithCommentWidth", 0);
    public static readonly DependencyProperty TextWidthProperty = RegisterDouble("TextWidth", 140);
    public static readonly DependencyProperty GroupWidthProperty = RegisterDouble("GroupWidth", 140);
    public static readonly DependencyProperty TypeWidthProperty = RegisterDouble("TypeWidth", 140);
    public static readonly DependencyProperty SignalNameWidthProperty = RegisterDouble("SignalNameWidth", 200);
    public static readonly DependencyProperty OpcItemWidthProperty = RegisterDouble("OpcItemWidth", 140);
    public static readonly DependencyProperty PriorityWidthProperty = RegisterDouble("PriorityWidth", 65);
    public static readonly DependencyProperty PriorityColorWidthProperty = RegisterDouble("PriorityColorWidth", 0);
    public static readonly DependencyProperty ActiveOrderProperty = RegisterInt("ActiveOrder", 2);
    public static readonly DependencyProperty SystemTimeOrderProperty = RegisterInt("SystemTimeOrder", 3);
    public static readonly DependencyProperty GoneOrderProperty = RegisterInt("GoneOrder", 4);
    public static readonly DependencyProperty AcknowledgedOrderProperty = RegisterInt("AcknowledgedOrder", 5);
    public static readonly DependencyProperty AcknowledgedWithCommentOrderProperty = RegisterInt("AcknowledgedWithCommentOrder", 6);
    public static readonly DependencyProperty TextOrderProperty = RegisterInt("TextOrder", 7);
    public static readonly DependencyProperty GroupOrderProperty = RegisterInt("GroupOrder", 8);
    public static readonly DependencyProperty TypeOrderProperty = RegisterInt("TypeOrder", 9);
    public static readonly DependencyProperty SignalNameOrderProperty = RegisterInt("SignalNameOrder", 10);
    public static readonly DependencyProperty OpcItemOrderProperty = RegisterInt("OpcItemOrder", 11);
    public static readonly DependencyProperty PriorityOrderProperty = RegisterInt("PriorityOrder", 12);
    public static readonly DependencyProperty PriorityColorOrderProperty = RegisterInt("PriorityColorOrder", 1);
    public static readonly DependencyProperty StatusTextProperty =
        DependencyProperty.Register(nameof(StatusText), typeof(string), typeof(HMIAlarmOnline), new PropertyMetadata("最近24小时未确认事件（不代表当前活动状态）"));

    static HMIAlarmOnline()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmOnline), new FrameworkPropertyMetadata(typeof(HMIAlarmOnline)));
    }

    public HMIAlarmOnline()
    {
        refreshTimer = new DispatcherTimer(DispatcherPriority.Background)
        {
            Interval = TimeSpan.FromSeconds(RefreshInterval)
        };
        refreshTimer.Tick += RefreshTimer_Tick;
        Loaded += HMIAlarmOnline_Loaded;
        Unloaded += HMIAlarmOnline_Unloaded;
    }

    [Category("HMI")]
    public string ServerName { get => (string)GetValue(ServerNameProperty); set => SetValue(ServerNameProperty, value); }
    [Category("HMI")]
    public int RefreshInterval { get => (int)GetValue(RefreshIntervalProperty); set => SetValue(RefreshIntervalProperty, value); }
    [Category("HMI")]
    public int MaxAlarmRows { get => (int)GetValue(MaxAlarmRowsProperty); set => SetValue(MaxAlarmRowsProperty, value); }
    [Category("HMI")]
    public string HornPath { get => (string)GetValue(HornPathProperty); set => SetValue(HornPathProperty, value); }
    [Category("HMI")]
    public bool Loop { get => (bool)GetValue(LoopProperty); set => SetValue(LoopProperty, value); }
    [Category("HMI")]
    public bool ShowDateTimeMilliseconds { get => (bool)GetValue(ShowDateTimeMillisecondsProperty); set => SetValue(ShowDateTimeMillisecondsProperty, value); }
    [Category("HMI")]
    public bool ShowHeader { get => (bool)GetValue(ShowHeaderProperty); set => SetValue(ShowHeaderProperty, value); }
    [Category("HMI")]
    public bool ShowScrollBars { get => (bool)GetValue(ShowScrollBarsProperty); set => SetValue(ShowScrollBarsProperty, value); }
    [Category("HMI")]
    public string Filter { get => (string)GetValue(FilterProperty); set => SetValue(FilterProperty, value); }
    [Category("HMI")]
    public int MinimumPriority { get => (int)GetValue(MinimumPriorityProperty); set => SetValue(MinimumPriorityProperty, value); }
    [Category("HMI")]
    public int MaximumPriority { get => (int)GetValue(MaximumPriorityProperty); set => SetValue(MaximumPriorityProperty, value); }
    [Category("HMI")]
    public string ServerSortOrder { get => (string)GetValue(ServerSortOrderProperty); set => SetValue(ServerSortOrderProperty, value); }
    [Category("HMI")]
    public Brush ActiveAlarmBackground { get => (Brush)GetValue(ActiveAlarmBackgroundProperty); set => SetValue(ActiveAlarmBackgroundProperty, value); }
    [Category("HMI")]
    public Brush ActiveAlarmForeground { get => (Brush)GetValue(ActiveAlarmForegroundProperty); set => SetValue(ActiveAlarmForegroundProperty, value); }
    [Category("HMI")]
    public Brush InactiveAlarmBackground { get => (Brush)GetValue(InactiveAlarmBackgroundProperty); set => SetValue(InactiveAlarmBackgroundProperty, value); }
    [Category("HMI")]
    public Brush InactiveAlarmForeground { get => (Brush)GetValue(InactiveAlarmForegroundProperty); set => SetValue(InactiveAlarmForegroundProperty, value); }
    [Category("HMI")]
    public Brush AcknowledgedAlarmBackground { get => (Brush)GetValue(AcknowledgedAlarmBackgroundProperty); set => SetValue(AcknowledgedAlarmBackgroundProperty, value); }
    [Category("HMI")]
    public Brush AcknowledgedAlarmForeground { get => (Brush)GetValue(AcknowledgedAlarmForegroundProperty); set => SetValue(AcknowledgedAlarmForegroundProperty, value); }
    [Category("HMI")]
    public Brush AcknowledgedAndGoneAlarmBackground { get => (Brush)GetValue(AcknowledgedAndGoneAlarmBackgroundProperty); set => SetValue(AcknowledgedAndGoneAlarmBackgroundProperty, value); }
    [Category("HMI")]
    public Brush AcknowledgedAndGoneAlarmForeground { get => (Brush)GetValue(AcknowledgedAndGoneAlarmForegroundProperty); set => SetValue(AcknowledgedAndGoneAlarmForegroundProperty, value); }
    [Category("HMI")]
    public bool FilterAlarmGroupsByUser { get => (bool)GetValue(FilterAlarmGroupsByUserProperty); set => SetValue(FilterAlarmGroupsByUserProperty, value); }
    [Category("HMI")]
    public bool EnablePopup { get => (bool)GetValue(EnablePopupProperty); set => SetValue(EnablePopupProperty, value); }
    [Category("HMI")]
    public bool WriteSecure { get => (bool)GetValue(WriteSecureProperty); set => SetValue(WriteSecureProperty, value); }
    [Category("HMI")]
    public bool UseColumnWidthsProperties { get => (bool)GetValue(UseColumnWidthsPropertiesProperty); set => SetValue(UseColumnWidthsPropertiesProperty, value); }
    [Category("HMI")]
    public bool AutoSizeColumns { get => (bool)GetValue(AutoSizeColumnsProperty); set => SetValue(AutoSizeColumnsProperty, value); }
    [Category("HMI")]
    public bool UseColumnOrderProperties { get => (bool)GetValue(UseColumnOrderPropertiesProperty); set => SetValue(UseColumnOrderPropertiesProperty, value); }
    public double ActiveWidth { get => (double)GetValue(ActiveWidthProperty); set => SetValue(ActiveWidthProperty, value); }
    public double SystemTimeWidth { get => (double)GetValue(SystemTimeWidthProperty); set => SetValue(SystemTimeWidthProperty, value); }
    public double GoneWidth { get => (double)GetValue(GoneWidthProperty); set => SetValue(GoneWidthProperty, value); }
    public double AcknowledgedWidth { get => (double)GetValue(AcknowledgedWidthProperty); set => SetValue(AcknowledgedWidthProperty, value); }
    public double AcknowledgedWithCommentWidth { get => (double)GetValue(AcknowledgedWithCommentWidthProperty); set => SetValue(AcknowledgedWithCommentWidthProperty, value); }
    public double TextWidth { get => (double)GetValue(TextWidthProperty); set => SetValue(TextWidthProperty, value); }
    public double GroupWidth { get => (double)GetValue(GroupWidthProperty); set => SetValue(GroupWidthProperty, value); }
    public double TypeWidth { get => (double)GetValue(TypeWidthProperty); set => SetValue(TypeWidthProperty, value); }
    public double SignalNameWidth { get => (double)GetValue(SignalNameWidthProperty); set => SetValue(SignalNameWidthProperty, value); }
    public double OpcItemWidth { get => (double)GetValue(OpcItemWidthProperty); set => SetValue(OpcItemWidthProperty, value); }
    public double PriorityWidth { get => (double)GetValue(PriorityWidthProperty); set => SetValue(PriorityWidthProperty, value); }
    public double PriorityColorWidth { get => (double)GetValue(PriorityColorWidthProperty); set => SetValue(PriorityColorWidthProperty, value); }
    public int ActiveOrder { get => (int)GetValue(ActiveOrderProperty); set => SetValue(ActiveOrderProperty, value); }
    public int SystemTimeOrder { get => (int)GetValue(SystemTimeOrderProperty); set => SetValue(SystemTimeOrderProperty, value); }
    public int GoneOrder { get => (int)GetValue(GoneOrderProperty); set => SetValue(GoneOrderProperty, value); }
    public int AcknowledgedOrder { get => (int)GetValue(AcknowledgedOrderProperty); set => SetValue(AcknowledgedOrderProperty, value); }
    public int AcknowledgedWithCommentOrder { get => (int)GetValue(AcknowledgedWithCommentOrderProperty); set => SetValue(AcknowledgedWithCommentOrderProperty, value); }
    public int TextOrder { get => (int)GetValue(TextOrderProperty); set => SetValue(TextOrderProperty, value); }
    public int GroupOrder { get => (int)GetValue(GroupOrderProperty); set => SetValue(GroupOrderProperty, value); }
    public int TypeOrder { get => (int)GetValue(TypeOrderProperty); set => SetValue(TypeOrderProperty, value); }
    public int SignalNameOrder { get => (int)GetValue(SignalNameOrderProperty); set => SetValue(SignalNameOrderProperty, value); }
    public int OpcItemOrder { get => (int)GetValue(OpcItemOrderProperty); set => SetValue(OpcItemOrderProperty, value); }
    public int PriorityOrder { get => (int)GetValue(PriorityOrderProperty); set => SetValue(PriorityOrderProperty, value); }
    public int PriorityColorOrder { get => (int)GetValue(PriorityColorOrderProperty); set => SetValue(PriorityColorOrderProperty, value); }
    [Category("HMI")]
    public ObservableCollection<UnacknowledgedEvent> Alarms => alarms;
    [Category("HMI")]
    public ObservableCollection<ColumnSettings> Columns { get; } = [];
    [Category("HMI")]
    public ObservableCollection<RowColor> RowColorDefinitions { get; } = [];
    [Category("HMI")]
    public ObservableCollection<PriorityColor> PriorityColorDefinitions { get; } = [];
    [Category("HMI")]
    public ObservableCollection<AlarmGroupFilter> GroupFilters { get; } = [];
    [Category("HMI")]
    public ObservableCollection<AlarmTypeFilter> TypeFilters { get; } = [];
    public string StatusText { get => (string)GetValue(StatusTextProperty); private set => SetValue(StatusTextProperty, value); }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (alarmGrid is not null)
        {
            alarmGrid.SelectionChanged -= AlarmGrid_SelectionChanged;
        }
        if (refreshButton is not null)
        {
            refreshButton.Click -= RefreshButton_Click;
        }
        if (acknowledgeButton is not null)
        {
            acknowledgeButton.Click -= AcknowledgeButton_Click;
        }
        if (hornMediaElement is not null)
        {
            hornMediaElement.MediaEnded -= HornMediaElement_MediaEnded;
            hornMediaElement.MediaFailed -= HornMediaElement_MediaFailed;
        }

        alarmGrid = GetTemplateChild("PART_AlarmGrid") as RadGridView;
        refreshButton = GetTemplateChild("PART_RefreshButton") as Button;
        acknowledgeButton = GetTemplateChild("PART_AcknowledgeButton") as Button;
        hornMediaElement = GetTemplateChild("PART_HornMediaElement") as MediaElement;
        if (alarmGrid is not null)
        {
            alarmGrid.ItemsSource = alarms;
            alarmGrid.SelectionChanged += AlarmGrid_SelectionChanged;
            ConfigureColumns();
        }
        if (refreshButton is not null)
        {
            refreshButton.Click += RefreshButton_Click;
        }
        if (acknowledgeButton is not null)
        {
            acknowledgeButton.Click += AcknowledgeButton_Click;
        }
        if (hornMediaElement is not null)
        {
            hornMediaElement.MediaEnded += HornMediaElement_MediaEnded;
            hornMediaElement.MediaFailed += HornMediaElement_MediaFailed;
            hornMediaElement.LoadedBehavior = MediaState.Manual;
            hornMediaElement.UnloadedBehavior = MediaState.Stop;
        }
        UpdateActionButtons();
    }

    private static DependencyProperty RegisterDouble(string name, double value) =>
        DependencyProperty.Register(name, typeof(double), typeof(HMIAlarmOnline), new PropertyMetadata(value));

    private static DependencyProperty RegisterInt(string name, int value) =>
        DependencyProperty.Register(name, typeof(int), typeof(HMIAlarmOnline), new PropertyMetadata(value));

    private static void OnRefreshIntervalChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
    {
        HMIAlarmOnline control = (HMIAlarmOnline)target;
        if ((int)args.NewValue < 1)
        {
            control.SetCurrentValue(RefreshIntervalProperty, args.OldValue);
            return;
        }
        control.refreshTimer.Interval = TimeSpan.FromSeconds((int)args.NewValue);
    }

    private static void OnHornPathChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
    {
        HMIAlarmOnline control = (HMIAlarmOnline)target;
        control.hornMediaElement?.Stop();
        control.hornMediaElement?.Close();
        control.hornMediaElement?.SetCurrentValue(MediaElement.SourceProperty, null);
        control.playingAlarmId = null;
    }

    private void HMIAlarmOnline_Loaded(object sender, RoutedEventArgs args)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }
        lifetimeCancellationSource?.Cancel();
        lifetimeCancellationSource?.Dispose();
        lifetimeCancellationSource = new CancellationTokenSource();
        connector = RuntimeServices.GetRequiredService<IHMIConnector>();
        refreshTimer.Start();
        _ = RefreshAlarmsAsync(lifetimeCancellationSource.Token);
    }

    private void HMIAlarmOnline_Unloaded(object sender, RoutedEventArgs args)
    {
        refreshTimer.Stop();
        lifetimeCancellationSource?.Cancel();
        hornMediaElement?.Stop();
        playingAlarmId = null;
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
        if (isRefreshing || connector is null)
        {
            return;
        }

        isRefreshing = true;
        if (refreshButton is not null)
        {
            refreshButton.IsEnabled = false;
        }
        try
        {
            DateTimeOffset endTime = DateTimeOffset.UtcNow;
            IReadOnlyList<UnacknowledgedEvent> result = await connector.GetUnacknowledgedEventsAsync(
                ServerName,
                endTime - QueryWindow,
                endTime,
                cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<UnacknowledgedEvent> filtered = result;
            if (MinimumPriority >= 0)
            {
                filtered = filtered.Where(alarm => alarm.Severity >= MinimumPriority);
            }
            if (MaximumPriority >= 0)
            {
                filtered = filtered.Where(alarm => alarm.Severity <= MaximumPriority);
            }
            filtered = ServerSortOrder == "PriorityDescending"
                ? filtered.OrderByDescending(alarm => alarm.Severity).ThenByDescending(alarm => alarm.Timestamp)
                : filtered.OrderByDescending(alarm => alarm.Timestamp);
            UnacknowledgedEvent[] snapshot = filtered.Take(Math.Max(1, MaxAlarmRows)).ToArray();

            HashSet<Guid> currentIds = snapshot.Select(alarm => alarm.Id).ToHashSet();
            Guid? newAlarmId = hasInitialSnapshot
                ? snapshot.Select(alarm => alarm.Id).FirstOrDefault(id => !previousAlarmIds.Contains(id))
                : null;
            bool hasNewAlarm = newAlarmId.HasValue && newAlarmId.Value != Guid.Empty;
            alarms.Clear();
            foreach (UnacknowledgedEvent alarm in snapshot)
            {
                alarms.Add(alarm);
            }
            previousAlarmIds.Clear();
            previousAlarmIds.UnionWith(currentIds);
            hasInitialSnapshot = true;

            if (playingAlarmId.HasValue && !currentIds.Contains(playingAlarmId.Value))
            {
                hornMediaElement?.Stop();
                playingAlarmId = null;
            }
            if (hasNewAlarm)
            {
                PlayAlarmSound(newAlarmId!.Value);
            }

            StatusText = BuildStatusText(snapshot.Length);
            UpdateActionButtons();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Alarm event query failed: {0}",
                exception);
            StatusText = $"报警事件查询失败：{exception.Message}";
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

    private string BuildStatusText(int count)
    {
        List<string> notices = [$"最近24小时未确认事件：{count} 条（不代表当前活动状态）"];
        if (Filter is "Active" or "ActiveOrNotAcknowledged")
        {
            notices.Add("活动状态筛选尚不支持");
        }
        if (GroupFilters.Count > 0 || TypeFilters.Count > 0 || FilterAlarmGroupsByUser)
        {
            notices.Add("组/类型筛选尚不支持");
        }
        if (Columns.Any(column => column.IsVisible && CreateColumnDefinition(column.Type) is null))
        {
            notices.Add("部分列类型尚不支持");
        }
        if (RowColorDefinitions.Count > 0 || PriorityColorDefinitions.Count > 0 || EnablePopup)
        {
            notices.Add("旧版状态色和详情弹窗尚未迁移");
        }
        return string.Join("；", notices);
    }

    private void ConfigureColumns()
    {
        if (alarmGrid is null)
        {
            return;
        }

        alarmGrid.Columns.Clear();
        IEnumerable<(string Type, string Header, string Property)> defaults =
        [
            ("Active", "时间", nameof(UnacknowledgedEvent.Timestamp)),
            ("Name", "点位", nameof(UnacknowledgedEvent.ChannelName)),
            ("Text", "事件描述", nameof(UnacknowledgedEvent.Message)),
            ("Priority", "级别", nameof(UnacknowledgedEvent.Severity))
        ];
        IEnumerable<(string Type, string Header, string Property)> columns = Columns.Count == 0
            ? defaults
            : Columns.Where(column => column.IsVisible)
                .Select(column => CreateColumnDefinition(column.Type))
                .Where(column => column.HasValue)
                .Select(column => column!.Value);
        List<(string Type, string Header, string Property)> configuredColumns = columns.ToList();
        if (configuredColumns.Count == 0)
        {
            configuredColumns = defaults.ToList();
        }

        if (UseColumnOrderProperties)
        {
            configuredColumns = configuredColumns
                .OrderBy(column => GetColumnOrder(column.Type))
                .ToList();
        }

        foreach ((string type, string header, string property) in configuredColumns)
        {
            Binding binding = new(property);
            if (property == nameof(UnacknowledgedEvent.Timestamp))
            {
                binding.StringFormat = ShowDateTimeMilliseconds ? "yyyy-MM-dd HH:mm:ss.fff" : "yyyy-MM-dd HH:mm:ss";
            }
            ColumnSettings? columnSettings = Columns.FirstOrDefault(column => column.Type == type);
            GridViewLength width = columnSettings is not null
                ? columnSettings.IsAutoSize || columnSettings.Width <= 0
                    ? GridViewLength.Auto
                    : new GridViewLength(columnSettings.Width)
                : GetColumnWidth(type);
            alarmGrid.Columns.Add(new GridViewDataColumn
            {
                Header = header,
                DataMemberBinding = binding,
                IsFilterable = columnSettings?.IsFilterable ?? true,
                Width = width
            });
        }
        alarmGrid.ShowColumnHeaders = ShowHeader;
        ScrollViewer.SetVerticalScrollBarVisibility(
            alarmGrid,
            ShowScrollBars ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled);
        ScrollViewer.SetHorizontalScrollBarVisibility(
            alarmGrid,
            ShowScrollBars ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled);
    }

    private static (string Type, string Header, string Property)? CreateColumnDefinition(string type) =>
        type switch
        {
            "Active" or "SystemTime" => (type, "时间", nameof(UnacknowledgedEvent.Timestamp)),
            "Name" or "SignalName" or "OpcItem" => (type, "点位", nameof(UnacknowledgedEvent.ChannelName)),
            "Text" => (type, "事件描述", nameof(UnacknowledgedEvent.Message)),
            "Priority" => (type, "级别", nameof(UnacknowledgedEvent.Severity)),
            _ => null
        };

    private int GetColumnOrder(string type) =>
        type switch
        {
            "Active" => ActiveOrder,
            "SystemTime" => SystemTimeOrder,
            "Text" => TextOrder,
            "Name" or "SignalName" => SignalNameOrder,
            "OpcItem" => OpcItemOrder,
            "Priority" => PriorityOrder,
            _ => int.MaxValue
        };

    private GridViewLength GetColumnWidth(string type)
    {
        if (!UseColumnWidthsProperties || AutoSizeColumns)
        {
            return GridViewLength.Auto;
        }

        double width = type switch
        {
            "Active" => ActiveWidth,
            "SystemTime" => SystemTimeWidth,
            "Gone" => GoneWidth,
            "Acknowledged" => AcknowledgedWidth,
            "Text" => TextWidth,
            "Name" or "SignalName" => SignalNameWidth,
            "OpcItem" => OpcItemWidth,
            "Priority" => PriorityWidth,
            _ => 0
        };
        return width > 0 ? new GridViewLength(width) : GridViewLength.Auto;
    }

    private void PlayAlarmSound(Guid alarmId)
    {
        if (hornMediaElement is null || string.IsNullOrWhiteSpace(HornPath))
        {
            return;
        }
        try
        {
            Uri soundUri = ResolveSoundUri(HornPath);
            if (hornMediaElement.Source != soundUri)
            {
                hornMediaElement.Stop();
                hornMediaElement.Source = soundUri;
            }
            hornMediaElement.Position = TimeSpan.Zero;
            playingAlarmId = alarmId;
            hornMediaElement.Play();
        }
        catch (Exception exception)
        {
            ReportSoundFailure(exception);
        }
    }

    private static Uri ResolveSoundUri(string path)
    {
        bool isUncPath = path.StartsWith(@"\\", StringComparison.Ordinal) || path.StartsWith("//", StringComparison.Ordinal);
        bool isDrivePath = path.Length >= 3 &&
            char.IsLetter(path[0]) &&
            path[1] == ':' &&
            (path[2] == '\\' || path[2] == '/');
        if (isUncPath || isDrivePath)
        {
            return new Uri(Path.GetFullPath(path));
        }
        if (path.StartsWith('\\') || path.StartsWith('/'))
        {
            string rootedRelativeFile = path.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
            return new Uri(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, rootedRelativeFile)));
        }

        const string siteOfOriginPrefix = "pack://siteoforigin:,,,/";
        if (path.StartsWith(siteOfOriginPrefix, StringComparison.OrdinalIgnoreCase))
        {
            string relativePath = Uri.UnescapeDataString(path[siteOfOriginPrefix.Length..]);
            return new Uri(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, relativePath)));
        }

        if (Uri.TryCreate(path, UriKind.Absolute, out Uri? absoluteUri))
        {
            if (absoluteUri.IsFile)
            {
                return absoluteUri;
            }
        }

        return new Uri(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, path)));
    }

    private void ReportSoundFailure(Exception exception)
    {
        StatusText = $"报警事件已更新，但音频播放失败：{exception.Message}";
        RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
            HMIDebugLevel.Error,
            "Alarm sound playback failed: {0}",
            exception);
    }

    private void HornMediaElement_MediaEnded(object sender, RoutedEventArgs args)
    {
        if (Loop && playingAlarmId.HasValue && alarms.Any(alarm => alarm.Id == playingAlarmId.Value))
        {
            hornMediaElement!.Position = TimeSpan.Zero;
            hornMediaElement.Play();
        }
    }

    private void HornMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs args)
    {
        ReportSoundFailure(args.ErrorException);
    }

    private void AlarmGrid_SelectionChanged(object sender, SelectionChangeEventArgs args) =>
        UpdateActionButtons();

    private void UpdateActionButtons()
    {
        if (acknowledgeButton is not null)
        {
            acknowledgeButton.IsEnabled = !isAcknowledging && alarmGrid?.SelectedItems.Count > 0;
        }
    }

    private void AcknowledgeButton_Click(object sender, RoutedEventArgs args)
    {
        if (WriteSecure)
        {
            StatusText = "当前运行时尚未实现单独授权确认，请关闭 WriteSecure 或使用已登录会话。";
            return;
        }
        UnacknowledgedEvent[] selectedAlarms = alarmGrid?.SelectedItems
            .OfType<UnacknowledgedEvent>()
            .ToArray() ?? [];
        if (selectedAlarms.Length == 0)
        {
            StatusText = "请先选择要确认的未确认事件。";
            return;
        }
        if (connector is null)
        {
            StatusText = "报警连接尚未初始化。";
            return;
        }

        isAcknowledging = true;
        UpdateActionButtons();
        try
        {
            connector.AcknowledgeAlarms(
                ServerName,
                selectedAlarms.Select(alarm => alarm.Id).ToArray(),
                string.Empty,
                (error, results) => Dispatcher.BeginInvoke(() =>
                {
                    isAcknowledging = false;
                    UpdateActionButtons();
                    if (error is not null || results.Length == 0 || results[0] != 0)
                    {
                        Exception failure = error ?? new InvalidOperationException("The server did not confirm the selected events.");
                        RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                            HMIDebugLevel.Error,
                            "Acknowledging alarm events failed: {0}",
                            failure);
                        StatusText = $"确认报警事件失败：{failure.GetBaseException().Message}";
                        return;
                    }

                    StatusText = $"已提交 {selectedAlarms.Length} 条事件确认。";
                    if (lifetimeCancellationSource is { IsCancellationRequested: false } source)
                    {
                        _ = RefreshAlarmsAsync(source.Token);
                    }
                }));
        }
        catch (Exception exception)
        {
            isAcknowledging = false;
            UpdateActionButtons();
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Acknowledging alarm events failed: {0}",
                exception);
            StatusText = $"确认报警事件失败：{exception.GetBaseException().Message}";
        }
    }
}
