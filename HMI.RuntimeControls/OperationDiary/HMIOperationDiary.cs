// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Communication;
using HMI.Runtime.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace HMI.RuntimeControls.OperationDiary;

public sealed class ColumnSettings
{
    public bool IsAutoSize { get; set; } = true;
    public double Width { get; set; } = 100;
    public string Type { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
}

public sealed class AlarmTypeDisplay
{
    public string SymbolicName { get; set; } = string.Empty;
    public string AlarmOnSymbolicName { get; set; } = string.Empty;
    public string AlarmOffSymbolicName { get; set; } = string.Empty;
    public string AlarmAcknowledgedSymbolicName { get; set; } = string.Empty;
    public string AlarmActivatedSymbolicName { get; set; } = string.Empty;
    public string AlarmDeactivatedSymbolicName { get; set; } = string.Empty;
    public System.Windows.Media.Brush? Color { get; set; }
}

public sealed class HMIOperationDiary : Control, INotifyPropertyChanged, IDisposable
{
    public static readonly DependencyProperty MaxEventsProperty = DependencyProperty.Register(
        nameof(MaxEvents), typeof(int), typeof(HMIOperationDiary), new PropertyMetadata(10, OnMaxEventsChanged));
    public static readonly DependencyProperty RefreshIntervalProperty = DependencyProperty.Register(
        nameof(RefreshInterval), typeof(int), typeof(HMIOperationDiary), new PropertyMetadata(5, OnRefreshIntervalChanged));
    public static readonly DependencyProperty GroupByDateProperty = DependencyProperty.Register(
        nameof(GroupByDate), typeof(bool), typeof(HMIOperationDiary), new PropertyMetadata(false, OnGroupByDateChanged));
    public static readonly DependencyProperty ClientsideConfigurationProperty = DependencyProperty.Register(
        nameof(ClientsideConfiguration), typeof(bool), typeof(HMIOperationDiary), new PropertyMetadata(false));
    public static readonly DependencyProperty ConfigurationNamespaceProperty = DependencyProperty.Register(
        nameof(ConfigurationNamespace), typeof(string), typeof(HMIOperationDiary), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty InitialConfigurationProperty = DependencyProperty.Register(
        nameof(InitialConfiguration), typeof(string), typeof(HMIOperationDiary), new PropertyMetadata(string.Empty));

    private readonly DispatcherTimer _timer;
    private readonly ObservableCollection<OperationEvent> _events = new();
    private readonly ObservableCollection<ColumnSettings> _columns = new();
    private readonly ObservableCollection<AlarmTypeDisplay> _alarmTypesInformation = new();
    private readonly CancellationTokenSource _disposeTokenSource = new();
    private IHMIConnector? _connector;
    private bool _refreshInProgress;
    private bool _disposed;
    private string _statusMessage = "Waiting for event data.";
    private DateTime _startDate = DateTime.Today.AddDays(-1);
    private DateTime _endDate = DateTime.Today;

    static HMIOperationDiary() => DefaultStyleKeyProperty.OverrideMetadata(
        typeof(HMIOperationDiary), new FrameworkPropertyMetadata(typeof(HMIOperationDiary)));

    public HMIOperationDiary()
    {
        _timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(5) };
        _timer.Tick += OnTimerTick;
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        _columns.CollectionChanged += OnColumnsChanged;
        ApplyGrouping();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<OperationEvent> Events => _events;
    public ObservableCollection<ColumnSettings> Columns => _columns;
    public ObservableCollection<AlarmTypeDisplay> AlarmTypesInformation => _alarmTypesInformation;
    public int MaxEvents { get => (int)GetValue(MaxEventsProperty); set => SetValue(MaxEventsProperty, value); }
    public int RefreshInterval { get => (int)GetValue(RefreshIntervalProperty); set => SetValue(RefreshIntervalProperty, value); }
    public bool GroupByDate { get => (bool)GetValue(GroupByDateProperty); set => SetValue(GroupByDateProperty, value); }
    public bool ClientsideConfiguration { get => (bool)GetValue(ClientsideConfigurationProperty); set => SetValue(ClientsideConfigurationProperty, value); }
    public string ConfigurationNamespace { get => (string)GetValue(ConfigurationNamespaceProperty); set => SetValue(ConfigurationNamespaceProperty, value); }
    public string InitialConfiguration { get => (string)GetValue(InitialConfigurationProperty); set => SetValue(InitialConfigurationProperty, value); }
    public string StatusMessage { get => _statusMessage; private set { _statusMessage = value; OnPropertyChanged(); } }
    public DateTime StartDate { get => _startDate; set { if (_startDate != value) { _startDate = value.Date; OnPropertyChanged(); } } }
    public DateTime EndDate { get => _endDate; set { if (_endDate != value) { _endDate = value.Date; OnPropertyChanged(); } } }

    private static void OnRefreshIntervalChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
    {
        HMIOperationDiary control = (HMIOperationDiary)target;
        if (control.RefreshInterval is < 1 or > 3600)
        {
            throw new ArgumentOutOfRangeException(nameof(RefreshInterval), "RefreshInterval must be between 1 and 3600 seconds.");
        }
        control._timer.Interval = TimeSpan.FromSeconds(control.RefreshInterval);
    }

    private static void OnMaxEventsChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
    {
        int maxEvents = (int)args.NewValue;
        if (maxEvents is < 1 or > 10000)
        {
            throw new ArgumentOutOfRangeException(nameof(MaxEvents), "MaxEvents must be between 1 and 10000.");
        }
    }

    private static void OnGroupByDateChanged(DependencyObject target, DependencyPropertyChangedEventArgs args) =>
        ((HMIOperationDiary)target).ApplyGrouping();

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        _connector ??= RuntimeServices.GetRequiredService<IHMIConnector>();
        _timer.Start();
        _ = RefreshAsync();
    }

    private void OnUnloaded(object sender, RoutedEventArgs args) => _timer.Stop();

    private void OnTimerTick(object? sender, EventArgs args) => _ = RefreshAsync();

    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        if (_refreshInProgress || _disposed || !IsLoaded)
        {
            return;
        }
        if (StartDate > EndDate)
        {
            StatusMessage = "Start date must not be after end date.";
            return;
        }

        _refreshInProgress = true;
        StatusMessage = "Loading RapidSCADA events...";
        using CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, _disposeTokenSource.Token);
        try
        {
            _connector ??= RuntimeServices.GetRequiredService<IHMIConnector>();
            DateTimeOffset start = new(StartDate.Date);
            DateTimeOffset end = new(EndDate.Date.AddDays(1).AddTicks(-1));
            IReadOnlyList<OperationEvent> events = await _connector.GetOperationEventsAsync(
                string.Empty, start, end, MaxEvents, linkedTokenSource.Token);

            _events.Clear();
            foreach (OperationEvent operationEvent in events)
            {
                _events.Add(operationEvent);
            }
            StatusMessage = $"Loaded {_events.Count} event(s).";
        }
        catch (OperationCanceledException) when (linkedTokenSource.IsCancellationRequested)
        {
            if (!_disposed)
            {
                StatusMessage = "Event query was cancelled.";
            }
        }
        catch (Exception exception)
        {
            StatusMessage = $"Unable to load RapidSCADA events: {exception.Message}";
        }
        finally
        {
            _refreshInProgress = false;
        }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        ApplyColumns();
        if (GetTemplateChild("PART_RefreshButton") is Button refreshButton)
        {
            refreshButton.Click += OnRefreshButtonClick;
        }
        if (GetTemplateChild("PART_StartDatePicker") is DatePicker startPicker)
        {
            startPicker.SelectedDateChanged += (_, _) => StartDate = startPicker.SelectedDate ?? StartDate;
        }
        if (GetTemplateChild("PART_EndDatePicker") is DatePicker endPicker)
        {
            endPicker.SelectedDateChanged += (_, _) => EndDate = endPicker.SelectedDate ?? EndDate;
        }
    }

    private void OnRefreshButtonClick(object sender, RoutedEventArgs args) => _ = RefreshAsync();

    private void OnColumnsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args) =>
        ApplyColumns();

    private void ApplyColumns()
    {
        if (GetTemplateChild("PART_EventsGrid") is not DataGrid grid || _columns.Count == 0)
        {
            return;
        }

        foreach (DataGridColumn column in grid.Columns)
        {
            column.Visibility = Visibility.Collapsed;
        }

        int displayIndex = 0;
        foreach (ColumnSettings setting in _columns)
        {
            string? header = setting.Type switch
            {
                "Time" => "Time",
                "SignalName" => "Signal",
                "EventText" => "Event",
                "SignalOldValue" => "Old value",
                "SignalNewValue" => "New value",
                "AlarmPriority" => "Severity",
                "AlarmAcknowledgedText" => "Ack",
                _ => null
            };
            DataGridColumn? column = grid.Columns.FirstOrDefault(candidate => Equals(candidate.Header, header));
            if (column is null)
            {
                continue;
            }

            column.Visibility = setting.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            column.Width = setting.IsAutoSize ? DataGridLength.Auto : new DataGridLength(Math.Max(1, setting.Width));
            column.DisplayIndex = displayIndex++;
        }
    }

    private void ApplyGrouping()
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(_events);
        view.GroupDescriptions.Clear();
        if (GroupByDate)
        {
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(OperationEvent.Date)));
        }
        OnPropertyChanged(nameof(Events));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        _timer.Stop();
        _timer.Tick -= OnTimerTick;
        _columns.CollectionChanged -= OnColumnsChanged;
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }
}
