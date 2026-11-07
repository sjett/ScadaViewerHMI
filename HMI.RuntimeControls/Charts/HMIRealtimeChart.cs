// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace HMI.RuntimeControls.Charts;

public enum RealtimeChartType
{
    Bar,
    Column,
    Line,
    Pie
}

public sealed record RealtimeChartSample(DateTime Timestamp, double Value);

public sealed class RealtimeChartSignal : DependencyObject
{
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(double),
        typeof(RealtimeChartSignal),
        new PropertyMetadata(0d));

    public string Name { get; init; } = string.Empty;
    public ObservableCollection<RealtimeChartSample> Samples { get; } = [];

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public void AddValue(double value, DateTime timestamp)
    {
        Value = value;
        Samples.Add(new RealtimeChartSample(timestamp, value));
        while (Samples.Count > 30)
        {
            Samples.RemoveAt(0);
        }
    }
}

public sealed class HMIRealtimeChart : Control
{
    private readonly ObservableCollection<RealtimeChartSignal> signalData = [];
    private readonly Dictionary<string, RealtimeChartSignal> signalsByName =
        new(StringComparer.OrdinalIgnoreCase);
    private ContentControl? chartHost;
    private TextBlock? statusText;
    private IHMIConnector? connector;
    private OnSignalChangedDelegate? signalChangedHandler;
    private readonly List<string> subscribedSignalNames = [];
    private bool isAuthorized;
    private bool isLoaded;

    static HMIRealtimeChart()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIRealtimeChart),
            new FrameworkPropertyMetadata(typeof(HMIRealtimeChart)));
    }

    public HMIRealtimeChart()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public string ServerName { get; set; } = string.Empty;
    public string SignalPrefix { get; set; } = string.Empty;
    public string SignalsNames { get; set; } = string.Empty;
    public string SignalsDescriptions { get; set; } = string.Empty;
    public string SecurityAuthorizationName { get; set; } = string.Empty;
    public DenyAccessBehaviour SecurityDenyAccessBehaviour { get; set; } = DenyAccessBehaviour.Hide;
    public RealtimeChartType ChartType { get; set; } = RealtimeChartType.Bar;
    public double Interval { get; set; } = 5;
    public double Minimum { get; set; }
    public double Maximum { get; set; } = 100;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        chartHost = GetTemplateChild("PART_ChartHost") as ContentControl;
        statusText = GetTemplateChild("PART_StatusText") as TextBlock;
        if (chartHost is not null && !DesignerProperties.GetIsInDesignMode(this))
        {
            chartHost.Content = CreateChart();
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        if (DesignerProperties.GetIsInDesignMode(this) || isLoaded)
        {
            return;
        }

        isLoaded = true;
        if (Minimum >= Maximum)
        {
            SetStatus("实时图表配置无效：Maximum 必须大于 Minimum。");
            return;
        }
        if (Interval <= 0 || double.IsNaN(Interval) || double.IsInfinity(Interval))
        {
            SetStatus("实时图表配置无效：Interval 必须是大于 0 的有限数值。");
            return;
        }

        connector = RuntimeServices.GetRequiredService<IHMIConnector>();
        connector.UserContextChanged += OnUserContextChanged;
        UpdateAuthorization();
    }

    private void OnUnloaded(object sender, RoutedEventArgs args)
    {
        isLoaded = false;
        UnsubscribeSignals();
        if (connector is not null)
        {
            connector.UserContextChanged -= OnUserContextChanged;
        }

        connector = null;
        signalChangedHandler = null;
        signalsByName.Clear();
        signalData.Clear();
    }

    private void OnSignalChanged(string signalName, object value)
    {
        if (!signalsByName.TryGetValue(signalName, out RealtimeChartSignal? signal))
        {
            return;
        }

        try
        {
            double numericValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            if (!double.IsFinite(numericValue))
            {
                return;
            }

            signal.AddValue(numericValue, DateTime.Now);
        }
        catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Warning,
                "Ignoring non-numeric real-time chart value for '{0}': {1}",
                signalName,
                exception.Message);
        }
    }

    private void OnUserContextChanged(string userName)
    {
        Dispatcher.BeginInvoke(UpdateAuthorization);
    }

    private void UpdateAuthorization()
    {
        if (!isLoaded)
        {
            return;
        }

        if (connector is null || string.IsNullOrWhiteSpace(SecurityAuthorizationName))
        {
            isAuthorized = true;
        }
        else
        {
            isAuthorized = connector.CheckProjectAuthorization(SecurityAuthorizationName);
        }

        if (SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide)
        {
            SetCurrentValue(VisibilityProperty, isAuthorized ? Visibility.Visible : Visibility.Collapsed);
        }
        else
        {
            SetCurrentValue(IsEnabledProperty, isAuthorized);
        }

        if (isAuthorized)
        {
            SubscribeSignals();
        }
        else
        {
            UnsubscribeSignals();
            SetStatus("当前用户无权查看此实时图表。");
        }
    }

    private void SubscribeSignals()
    {
        if (connector is null || subscribedSignalNames.Count > 0)
        {
            return;
        }

        string[] names = ParseSignalNames();
        if (names.Length == 0)
        {
            SetStatus("请填写 SignalsNames（以分号分隔 RapidSCADA 通道名称）。");
            return;
        }

        if (signalsByName.Count == 0)
        {
            string[] descriptions = ParseSignalNames(SignalsDescriptions);
            for (int index = 0; index < names.Length; index++)
            {
                string signalName = SignalPrefix + names[index];
                RealtimeChartSignal signal = new()
                {
                    Name = index < descriptions.Length && !string.IsNullOrWhiteSpace(descriptions[index])
                        ? descriptions[index]
                        : names[index]
                };
                signalData.Add(signal);
                signalsByName.Add(signalName, signal);
            }

            chartHost ??= GetTemplateChild("PART_ChartHost") as ContentControl;
            if (chartHost is not null)
            {
                chartHost.Content = CreateChart();
            }
        }

        signalChangedHandler ??= OnSignalChanged;
        foreach (string signalName in signalsByName.Keys)
        {
            connector.RegisterSignalChangedHandler(ServerName, signalName, signalChangedHandler);
            subscribedSignalNames.Add(signalName);
        }

        SetStatus($"已订阅 {subscribedSignalNames.Count} 个实时通道。");
    }

    private void UnsubscribeSignals()
    {
        if (connector is not null && signalChangedHandler is not null)
        {
            foreach (string signalName in subscribedSignalNames)
            {
                connector.UnregisterSignalChangedHandler(ServerName, signalName, signalChangedHandler);
            }
        }

        subscribedSignalNames.Clear();
    }

    private FrameworkElement CreateChart() =>
        ChartType switch
        {
            RealtimeChartType.Bar => CreateCartesianChart(bar: true),
            RealtimeChartType.Column => CreateCartesianChart(bar: false),
            RealtimeChartType.Line => CreateLineChart(),
            RealtimeChartType.Pie => CreatePieChart(),
            _ => throw new ArgumentOutOfRangeException(nameof(ChartType), ChartType, "Unsupported real-time chart type.")
        };

    private RadCartesianChart CreateCartesianChart(bool bar)
    {
        RadCartesianChart chart = new() { Palette = ChartPalettes.Windows11 };
        CategoricalAxis categoryAxis = new();
        LinearAxis valueAxis = new() { Minimum = Minimum, Maximum = Maximum, MajorStep = Interval };
        if (bar)
        {
            chart.VerticalAxis = categoryAxis;
            chart.HorizontalAxis = valueAxis;
            chart.Series.Add(new BarSeries
            {
                CategoryBinding = new PropertyNameDataPointBinding(nameof(RealtimeChartSignal.Name)),
                ValueBinding = new PropertyNameDataPointBinding(nameof(RealtimeChartSignal.Value)),
                ItemsSource = signalData
            });
        }
        else
        {
            chart.HorizontalAxis = categoryAxis;
            chart.VerticalAxis = valueAxis;
            chart.Series.Add(new BarSeries
            {
                CategoryBinding = new PropertyNameDataPointBinding(nameof(RealtimeChartSignal.Name)),
                ValueBinding = new PropertyNameDataPointBinding(nameof(RealtimeChartSignal.Value)),
                ItemsSource = signalData
            });
        }

        return chart;
    }

    private RadCartesianChart CreateLineChart()
    {
        RadCartesianChart chart = new()
        {
            Palette = ChartPalettes.Windows11,
            HorizontalAxis = new DateTimeContinuousAxis { LabelFormat = "HH:mm:ss" },
            VerticalAxis = new LinearAxis { Minimum = Minimum, Maximum = Maximum, MajorStep = Interval }
        };

        foreach (RealtimeChartSignal signal in signalData)
        {
            chart.Series.Add(new LineSeries
            {
                DisplayName = signal.Name,
                CategoryBinding = new PropertyNameDataPointBinding(nameof(RealtimeChartSample.Timestamp)),
                ValueBinding = new PropertyNameDataPointBinding(nameof(RealtimeChartSample.Value)),
                ItemsSource = signal.Samples
            });
        }

        return chart;
    }

    private RadPieChart CreatePieChart()
    {
        RadPieChart chart = new() { Palette = ChartPalettes.Windows11 };
        chart.Series.Add(new PieSeries
        {
            ValueBinding = new PropertyNameDataPointBinding(nameof(RealtimeChartSignal.Value)),
            ItemsSource = signalData
        });
        return chart;
    }

    private string[] ParseSignalNames() => ParseSignalNames(SignalsNames);

    private static string[] ParseSignalNames(string? names) =>
        (names ?? string.Empty)
            .Split([';', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private void SetStatus(string message)
    {
        if (statusText is not null)
        {
            statusText.Text = message;
        }
    }
}
