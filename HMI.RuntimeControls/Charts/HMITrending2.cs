// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HMI.Runtime.Communication;
using HMI.Runtime.Core;
using HMI.Runtime.Shared.Dialogs;
using HMI.RuntimeControls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace HMI.RuntimeControls.Charts;

public sealed record TrendChartPoint(DateTime Timestamp, double Value);

public enum TrendingTimeZone
{
    ClientTime,
    ServerTime,
    UTCTime
}

public class HMITrending2 : Control
{
    private readonly DispatcherTimer refreshTimer;
    private RadCartesianChart? chart;
    private DatePicker? startDatePicker;
    private DatePicker? endDatePicker;
    private TextBox? startTimeTextBox;
    private TextBox? endTimeTextBox;
    private Button? refreshButton;
    private TextBlock? statusText;
    private CancellationTokenSource? lifetimeCancellationSource;
    private IHMIConnector? connector;
    private bool isRefreshing;

    static HMITrending2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMITrending2),
            new FrameworkPropertyMetadata(typeof(HMITrending2)));
    }

    public HMITrending2()
    {
        refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
        refreshTimer.Tick += RefreshTimer_Tick;
        Loaded += HMITrending2_Loaded;
        Unloaded += HMITrending2_Unloaded;
    }

    public string ServerName { get; set; } = string.Empty;
    public string Signals { get; set; } = string.Empty;
    public int RangeHours { get; set; } = 24;
    public int ArchiveBit { get; set; }
    public bool ClientsideConfiguration { get; set; }
    public bool ClientSideConfiguration
    {
        get => ClientsideConfiguration;
        set => ClientsideConfiguration = value;
    }
    public string ConfigurationNamespace { get; set; } = string.Empty;
    public string InitialConfiguration { get; set; } = string.Empty;
    public DialogModality PopupModality { get; set; } = DialogModality.Modal;
    public TrendingTimeZone TimeZone { get; set; }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (refreshButton is not null)
        {
            refreshButton.Click -= RefreshButton_Click;
        }

        chart = GetTemplateChild("PART_Chart") as RadCartesianChart;
        startDatePicker = GetTemplateChild("PART_StartDatePicker") as DatePicker;
        endDatePicker = GetTemplateChild("PART_EndDatePicker") as DatePicker;
        startTimeTextBox = GetTemplateChild("PART_StartTimeTextBox") as TextBox;
        endTimeTextBox = GetTemplateChild("PART_EndTimeTextBox") as TextBox;
        refreshButton = GetTemplateChild("PART_RefreshButton") as Button;
        statusText = GetTemplateChild("PART_StatusText") as TextBlock;
        if (refreshButton is not null)
        {
            refreshButton.Click += RefreshButton_Click;
        }
    }

    private async void HMITrending2_Loaded(object sender, RoutedEventArgs args)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        lifetimeCancellationSource?.Cancel();
        lifetimeCancellationSource?.Dispose();
        CancellationTokenSource source = new();
        lifetimeCancellationSource = source;
        connector = RuntimeServices.GetRequiredService<IHMIConnector>();
        DateTime end = DateTime.Now;
        DateTime start = end.AddHours(-Math.Max(1, RangeHours));
        startDatePicker?.SetCurrentValue(DatePicker.SelectedDateProperty, start.Date);
        endDatePicker?.SetCurrentValue(DatePicker.SelectedDateProperty, end.Date);
        startTimeTextBox?.SetCurrentValue(TextBox.TextProperty, start.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
        endTimeTextBox?.SetCurrentValue(TextBox.TextProperty, end.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
        refreshTimer.Start();
        await RefreshChartAsync(source.Token);
    }

    private void HMITrending2_Unloaded(object sender, RoutedEventArgs args)
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
            _ = RefreshChartAsync(source.Token);
        }
    }

    private async void RefreshButton_Click(object sender, RoutedEventArgs args)
    {
        if (lifetimeCancellationSource is { IsCancellationRequested: false } source)
        {
            await RefreshChartAsync(source.Token);
        }
    }

    private async Task RefreshChartAsync(CancellationToken cancellationToken)
    {
        if (isRefreshing || connector is null || chart is null)
        {
            return;
        }
        bool useUtc = TimeZone != TrendingTimeZone.ClientTime;
        if (!HistoricalTrendTimeRange.TryCreate(
                startDatePicker?.SelectedDate,
                startTimeTextBox?.Text,
                useUtc,
                out DateTimeOffset startTime) ||
            !HistoricalTrendTimeRange.TryCreate(
                endDatePicker?.SelectedDate,
                endTimeTextBox?.Text,
                useUtc,
                out DateTimeOffset endTime) ||
            startTime > endTime)
        {
            SetStatus("请输入有效的开始和结束日期时间（时间格式为 HH:mm:ss），且开始时间不能晚于结束时间。");
            return;
        }

        string[] signals = ParseSignals(Signals);
        if (signals.Length == 0)
        {
            SetStatus("请在设计器属性 Signals 中填写 RapidSCADA 通道名称（以分号分隔）。");
            return;
        }

        isRefreshing = true;
        if (refreshButton is not null)
        {
            refreshButton.IsEnabled = false;
        }
        try
        {
            IReadOnlyList<HistoricalTrendSeries> trends = await connector.GetHistoricalTrendsAsync(
                ServerName,
                signals,
                startTime,
                endTime,
                ArchiveBit,
                cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            chart.Series.Clear();
            foreach (HistoricalTrendSeries trend in trends)
            {
                ObservableCollection<TrendChartPoint> points = new(trend.Points.Select(point =>
                    new TrendChartPoint(ConvertTimestamp(point.Timestamp), point.Value)));
                LineSeries series = new()
                {
                    DisplayName = trend.SignalName,
                    CategoryBinding = new PropertyNameDataPointBinding(nameof(TrendChartPoint.Timestamp)),
                    ValueBinding = new PropertyNameDataPointBinding(nameof(TrendChartPoint.Value)),
                    ItemsSource = points
                };
                chart.Series.Add(series);
            }

            string timezoneNotice = TimeZone == TrendingTimeZone.ServerTime
                ? "；服务器时区未配置，时间按 UTC 显示"
                : string.Empty;
            SetStatus($"已加载 {trends.Sum(trend => trend.Points.Count)} 个样本，{trends.Count} 条曲线{timezoneNotice}");
            if (!string.IsNullOrWhiteSpace(InitialConfiguration))
            {
                SetStatus($"已加载当前通道数据；旧版命名配置“{InitialConfiguration}”需在设计器转为 Signals 属性。{timezoneNotice}");
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Historical trend query failed: {0}",
                exception);
            SetStatus($"历史趋势查询失败：{exception.GetBaseException().Message}");
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

    private DateTime ConvertTimestamp(DateTime timestamp) =>
        TimeZone switch
        {
            TrendingTimeZone.ClientTime => DateTime.SpecifyKind(timestamp, DateTimeKind.Utc).ToLocalTime(),
            _ => DateTime.SpecifyKind(timestamp, DateTimeKind.Utc)
        };

    private void SetStatus(string message)
    {
        if (statusText is not null)
        {
            statusText.Text = message;
        }
    }

    internal static string[] ParseSignals(string? signals) =>
        (signals ?? string.Empty)
            .Split([';', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
}
