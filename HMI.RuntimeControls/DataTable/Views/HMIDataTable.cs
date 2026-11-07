// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using HMI.Runtime.Communication;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using HMI.RuntimeControls;

namespace HMI.RuntimeControls.DataTable.Views;

public enum DataTableButton
{
    TimeSelection,
    Settings,
    Save,
    Print,
    Load,
    Export
}

public sealed class ButtonSettings
{
    public string SecurityAuthorizationName { get; set; } = string.Empty;
    public Visibility Visibility { get; set; } = Visibility.Visible;
    public DenyAccessBehaviour SecurityDenyAccessBehaviour { get; set; } = DenyAccessBehaviour.Hide;
}

public sealed class HMIDataTableButtons
{
    public ButtonSettings TimeSelection { get; set; } = new();
    public ButtonSettings Settings { get; set; } = new();
    public ButtonSettings Print { get; set; } = new();
    public ButtonSettings Export { get; set; } = new();
    public ButtonSettings Save { get; set; } = new();
    public ButtonSettings Load { get; set; } = new();
}

public sealed class HMIDataTable : Control
{
    private readonly ObservableCollection<HistoricalDataRow> visibleRows = [];
    private readonly List<HistoricalDataRow> allRows = [];
    private readonly DispatcherTimer refreshTimer;
    private RadGridView? grid;
    private DatePicker? startDatePicker;
    private DatePicker? endDatePicker;
    private TextBox? startTimeTextBox;
    private TextBox? endTimeTextBox;
    private Button? refreshButton;
    private Button? copyButton;
    private Button? settingsButton;
    private Button? saveConfigurationButton;
    private Button? loadConfigurationButton;
    private Button? printButton;
    private Button? exportButton;
    private Button? previousPageButton;
    private Button? nextPageButton;
    private TextBlock? pageText;
    private TextBlock? statusText;
    private StackPanel? pagerPanel;
    private CancellationTokenSource? lifetimeCancellationSource;
    private IHMIConnector? connector;
    private int currentPage;
    private bool isRefreshing;
    private int pageSize = 100;
    private int rangeHours = 24;
    private int archiveBit;

    static HMIDataTable()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIDataTable),
            new FrameworkPropertyMetadata(typeof(HMIDataTable)));
    }

    public HMIDataTable()
    {
        refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
        refreshTimer.Tick += RefreshTimer_Tick;
        Loaded += HMIDataTable_Loaded;
        Unloaded += HMIDataTable_Unloaded;
    }

    public string ServerName { get; set; } = string.Empty;
    public string Signals { get; set; } = string.Empty;
    public string InitialConfiguration { get; set; } = string.Empty;
    public bool ClientSideConfiguration { get; set; }
    public bool ShowPager { get; set; } = true;
    public int PageSize
    {
        get => pageSize;
        set => pageSize = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }
    public int RangeHours
    {
        get => rangeHours;
        set => rangeHours = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }
    public int ArchiveBit
    {
        get => archiveBit;
        set => archiveBit = value is >= 0 and <= byte.MaxValue
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value));
    }
    public HMIDataTableButtons ButtonsConfiguration { get; set; } = new();
    public ObservableCollection<HistoricalDataRow> Rows => visibleRows;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (refreshButton is not null)
        {
            refreshButton.Click -= RefreshButton_Click;
        }
        if (copyButton is not null)
        {
            copyButton.Click -= CopyButton_Click;
        }
        if (settingsButton is not null)
        {
            settingsButton.Click -= SettingsButton_Click;
        }
        if (saveConfigurationButton is not null)
        {
            saveConfigurationButton.Click -= SaveConfigurationButton_Click;
        }
        if (loadConfigurationButton is not null)
        {
            loadConfigurationButton.Click -= LoadConfigurationButton_Click;
        }
        if (exportButton is not null)
        {
            exportButton.Click -= ExportButton_Click;
        }
        if (printButton is not null)
        {
            printButton.Click -= PrintButton_Click;
        }
        if (previousPageButton is not null)
        {
            previousPageButton.Click -= PreviousPageButton_Click;
        }
        if (nextPageButton is not null)
        {
            nextPageButton.Click -= NextPageButton_Click;
        }

        grid = GetTemplateChild("PART_DataGrid") as RadGridView;
        startDatePicker = GetTemplateChild("PART_StartDatePicker") as DatePicker;
        endDatePicker = GetTemplateChild("PART_EndDatePicker") as DatePicker;
        startTimeTextBox = GetTemplateChild("PART_StartTimeTextBox") as TextBox;
        endTimeTextBox = GetTemplateChild("PART_EndTimeTextBox") as TextBox;
        refreshButton = GetTemplateChild("PART_RefreshButton") as Button;
        copyButton = GetTemplateChild("PART_CopyButton") as Button;
        settingsButton = GetTemplateChild("PART_SettingsButton") as Button;
        saveConfigurationButton = GetTemplateChild("PART_SaveConfigurationButton") as Button;
        loadConfigurationButton = GetTemplateChild("PART_LoadConfigurationButton") as Button;
        printButton = GetTemplateChild("PART_PrintButton") as Button;
        exportButton = GetTemplateChild("PART_ExportButton") as Button;
        previousPageButton = GetTemplateChild("PART_PreviousPageButton") as Button;
        nextPageButton = GetTemplateChild("PART_NextPageButton") as Button;
        pagerPanel = GetTemplateChild("PART_PagerPanel") as StackPanel;
        pageText = GetTemplateChild("PART_PageText") as TextBlock;
        statusText = GetTemplateChild("PART_StatusText") as TextBlock;
        if (grid is not null)
        {
            grid.ItemsSource = visibleRows;
        }
        if (refreshButton is not null)
        {
            refreshButton.Click += RefreshButton_Click;
        }
        if (copyButton is not null)
        {
            copyButton.Click += CopyButton_Click;
        }
        if (settingsButton is not null)
        {
            settingsButton.Click += SettingsButton_Click;
        }
        if (saveConfigurationButton is not null)
        {
            saveConfigurationButton.Click += SaveConfigurationButton_Click;
        }
        if (loadConfigurationButton is not null)
        {
            loadConfigurationButton.Click += LoadConfigurationButton_Click;
        }
        if (exportButton is not null)
        {
            exportButton.Click += ExportButton_Click;
        }
        if (printButton is not null)
        {
            printButton.Click += PrintButton_Click;
        }
        if (previousPageButton is not null)
        {
            previousPageButton.Click += PreviousPageButton_Click;
        }
        if (nextPageButton is not null)
        {
            nextPageButton.Click += NextPageButton_Click;
        }

        SetButtonVisibility("PART_SettingsButton", ButtonsConfiguration.Settings);
        SetButtonVisibility("PART_TimeSelectionPanel", ButtonsConfiguration.TimeSelection);
        SetButtonVisibility("PART_ExportButton", ButtonsConfiguration.Export);
        SetButtonVisibility("PART_SaveConfigurationButton", ButtonsConfiguration.Save);
        SetButtonVisibility("PART_LoadConfigurationButton", ButtonsConfiguration.Load);
        SetButtonVisibility("PART_PrintButton", ButtonsConfiguration.Print);
        SetButtonVisibility("PART_CopyButton", new ButtonSettings());
        pagerPanel?.SetCurrentValue(VisibilityProperty, ShowPager ? Visibility.Visible : Visibility.Collapsed);
    }

    private async void HMIDataTable_Loaded(object sender, RoutedEventArgs args)
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
        ApplyButtonSecurity("PART_SettingsButton", ButtonsConfiguration.Settings);
        ApplyButtonSecurity("PART_TimeSelectionPanel", ButtonsConfiguration.TimeSelection);
        ApplyButtonSecurity("PART_ExportButton", ButtonsConfiguration.Export);
        ApplyButtonSecurity("PART_SaveConfigurationButton", ButtonsConfiguration.Save);
        ApplyButtonSecurity("PART_LoadConfigurationButton", ButtonsConfiguration.Load);
        ApplyButtonSecurity("PART_PrintButton", ButtonsConfiguration.Print);
        SetDefaultTimeRange();
        refreshTimer.Start();
        await RefreshDataAsync(source.Token);
    }

    private void SetDefaultTimeRange()
    {
        DateTime end = DateTime.Now;
        DateTime start = end.AddHours(-RangeHours);
        startDatePicker?.SetCurrentValue(DatePicker.SelectedDateProperty, start.Date);
        endDatePicker?.SetCurrentValue(DatePicker.SelectedDateProperty, end.Date);
        startTimeTextBox?.SetCurrentValue(TextBox.TextProperty, start.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
        endTimeTextBox?.SetCurrentValue(TextBox.TextProperty, end.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
    }

    private void HMIDataTable_Unloaded(object sender, RoutedEventArgs args)
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
            _ = RefreshDataAsync(source.Token);
        }
    }

    private async void RefreshButton_Click(object sender, RoutedEventArgs args)
    {
        if (lifetimeCancellationSource is { IsCancellationRequested: false } source)
        {
            await RefreshDataAsync(source.Token);
        }
    }

    private async Task RefreshDataAsync(CancellationToken cancellationToken)
    {
        if (isRefreshing || connector is null)
        {
            return;
        }
        if (!HistoricalTrendTimeRange.TryCreate(
                startDatePicker?.SelectedDate,
                startTimeTextBox?.Text,
                useUtc: false,
                out DateTimeOffset startTime) ||
            !HistoricalTrendTimeRange.TryCreate(
                endDatePicker?.SelectedDate,
                endTimeTextBox?.Text,
                useUtc: false,
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
            ConfigureColumns(trends.Select(trend => trend.SignalName).ToArray());
            BuildRows(trends);
            currentPage = 0;
            ShowCurrentPage();
            string legacyNotice = string.IsNullOrWhiteSpace(InitialConfiguration)
                ? string.Empty
                : $"；旧版命名配置“{InitialConfiguration}”需在设计器中转为 Signals 属性";
            SetStatus($"查询完成：{allRows.Count} 行，{trends.Count} 个通道；配置可保存/加载为本地 JSON 文件{legacyNotice}");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Historical data table query failed: {0}",
                exception);
            SetStatus($"历史数据表查询失败：{exception.GetBaseException().Message}");
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

    private void ConfigureColumns(IReadOnlyList<string> signalNames)
    {
        if (grid is null)
        {
            return;
        }

        grid.Columns.Clear();
        grid.Columns.Add(new GridViewDataColumn
        {
            Header = "序号",
            DataMemberBinding = new Binding(nameof(HistoricalDataRow.Index)),
            Width = new GridViewLength(60),
            IsReadOnly = true
        });
        grid.Columns.Add(new GridViewDataColumn
        {
            Header = "时间",
            DataMemberBinding = new Binding(nameof(HistoricalDataRow.Timestamp))
            {
                StringFormat = "yyyy-MM-dd HH:mm:ss.fff"
            },
            Width = new GridViewLength(190),
            IsReadOnly = true
        });
        for (int index = 0; index < signalNames.Count; index++)
        {
            grid.Columns.Add(new GridViewDataColumn
            {
                Header = signalNames[index],
                DataMemberBinding = new Binding($"Values[{index}]")
                {
                    StringFormat = "G"
                },
                IsReadOnly = true
            });
        }
    }

    private void BuildRows(IReadOnlyList<HistoricalTrendSeries> trends)
    {
        allRows.Clear();
        SortedSet<DateTime> timestamps = new(
            trends.SelectMany(trend => trend.Points).Select(point => point.Timestamp));
        Dictionary<string, Dictionary<DateTime, double>> valuesBySignal = new(StringComparer.OrdinalIgnoreCase);
        foreach (HistoricalTrendSeries trend in trends)
        {
            valuesBySignal[trend.SignalName] = trend.Points
                .GroupBy(point => point.Timestamp)
                .ToDictionary(group => group.Key, group => group.Last().Value);
        }

        int index = 1;
        foreach (DateTime timestamp in timestamps)
        {
            double?[] values = trends
                .Select(trend => valuesBySignal[trend.SignalName].TryGetValue(timestamp, out double value)
                    ? value
                    : (double?)null)
                .ToArray();
            allRows.Add(new HistoricalDataRow(index++, timestamp, values));
        }
    }

    private void ShowCurrentPage()
    {
        visibleRows.Clear();
        int pageCount = Math.Max(1, (int)Math.Ceiling(allRows.Count / (double)PageSize));
        currentPage = Math.Clamp(currentPage, 0, pageCount - 1);
        IEnumerable<HistoricalDataRow> page = ShowPager
            ? allRows.Skip(currentPage * PageSize).Take(PageSize)
            : allRows;
        foreach (HistoricalDataRow row in page)
        {
            visibleRows.Add(row);
        }

        if (pageText is not null)
        {
            pageText.Text = $"{currentPage + 1} / {pageCount}";
        }
        if (previousPageButton is not null)
        {
            previousPageButton.IsEnabled = currentPage > 0;
        }
        if (nextPageButton is not null)
        {
            nextPageButton.IsEnabled = currentPage < pageCount - 1;
        }
    }

    private void PreviousPageButton_Click(object sender, RoutedEventArgs args)
    {
        currentPage--;
        ShowCurrentPage();
    }

    private void NextPageButton_Click(object sender, RoutedEventArgs args)
    {
        currentPage++;
        ShowCurrentPage();
    }

    private void CopyButton_Click(object sender, RoutedEventArgs args)
    {
        if (visibleRows.Count == 0)
        {
            SetStatus("当前页没有可复制的数据。");
            return;
        }
        try
        {
            StringBuilder output = new();
            output.Append("序号\t时间");
            foreach (GridViewDataColumn column in grid!.Columns.OfType<GridViewDataColumn>().Skip(2))
            {
                output.Append('\t').Append(column.Header);
            }
            foreach (HistoricalDataRow row in visibleRows)
            {
                output.AppendLine();
                output.Append(row.Index).Append('\t')
                    .Append(row.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
                foreach (double? value in row.Values)
                {
                    output.Append('\t').Append(value?.ToString("G", CultureInfo.InvariantCulture) ?? string.Empty);
                }
            }
            Clipboard.SetText(output.ToString());
            SetStatus($"已复制当前页 {visibleRows.Count} 行。");
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Copying data table rows failed: {0}",
                exception);
            SetStatus($"复制失败：{exception.GetBaseException().Message}");
        }
    }

    private async void SettingsButton_Click(object sender, RoutedEventArgs args)
    {
        HMIDataTableSettingsWindow dialog = new(Signals, RangeHours, ArchiveBit)
        {
            Owner = Window.GetWindow(this)
        };
        if (dialog.ShowDialog() != true)
        {
            return;
        }

        Signals = dialog.Signals;
        RangeHours = dialog.RangeHours;
        ArchiveBit = dialog.ArchiveBit;
        SetDefaultTimeRange();

        if (lifetimeCancellationSource is { IsCancellationRequested: false } source)
        {
            await RefreshDataAsync(source.Token);
        }
    }

    private void SaveConfigurationButton_Click(object sender, RoutedEventArgs args)
    {
        SaveFileDialog dialog = new()
        {
            Filter = "数据表配置 (*.json)|*.json",
            DefaultExt = ".json",
            AddExtension = true,
            FileName = "historical-data-table.json"
        };
        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            DataTableUserConfiguration configuration = new(Signals, RangeHours, ArchiveBit);
            string json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(dialog.FileName, json, new UTF8Encoding(true));
            SetStatus($"配置已保存到 {dialog.FileName}。");
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Saving data table configuration failed: {0}",
                exception);
            SetStatus($"保存配置失败：{exception.GetBaseException().Message}");
        }
    }

    private async void LoadConfigurationButton_Click(object sender, RoutedEventArgs args)
    {
        OpenFileDialog dialog = new()
        {
            Filter = "数据表配置 (*.json)|*.json",
            CheckFileExists = true,
            Multiselect = false
        };
        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            string json = File.ReadAllText(dialog.FileName);
            DataTableUserConfiguration? configuration = JsonSerializer.Deserialize<DataTableUserConfiguration>(json);
            if (configuration is null ||
                configuration.RangeHours <= 0 ||
                configuration.ArchiveBit is < 0 or > byte.MaxValue)
            {
                throw new InvalidDataException("配置内容无效：RangeHours 必须大于 0，ArchiveBit 必须在 0 到 255 之间。");
            }

            Signals = configuration.Signals ?? string.Empty;
            RangeHours = configuration.RangeHours;
            ArchiveBit = configuration.ArchiveBit;
            SetDefaultTimeRange();
            SetStatus($"配置已从 {dialog.FileName} 加载。");
            if (lifetimeCancellationSource is { IsCancellationRequested: false } source)
            {
                await RefreshDataAsync(source.Token);
            }
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Loading data table configuration failed: {0}",
                exception);
            SetStatus($"加载配置失败：{exception.GetBaseException().Message}");
        }
    }

    private void ExportButton_Click(object sender, RoutedEventArgs args)
    {
        if (allRows.Count == 0 || grid is null)
        {
            SetStatus("没有可导出的数据。");
            return;
        }

        SaveFileDialog dialog = new()
        {
            Filter = "CSV 文件 (*.csv)|*.csv",
            DefaultExt = ".csv",
            AddExtension = true,
            FileName = "historical-data.csv"
        };
        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            using StreamWriter writer = new(dialog.FileName, false, new UTF8Encoding(true));
            writer.WriteLine(string.Join(",", grid.Columns.OfType<GridViewDataColumn>().Select(column => EscapeCsv(column.Header?.ToString() ?? string.Empty))));
            foreach (HistoricalDataRow row in allRows)
            {
                string[] fields =
                [
                    row.Index.ToString(CultureInfo.InvariantCulture),
                    row.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                    .. row.Values.Select(value => value?.ToString("G", CultureInfo.InvariantCulture) ?? string.Empty)
                ];
                writer.WriteLine(string.Join(",", fields.Select(EscapeCsv)));
            }
            SetStatus($"已导出 {allRows.Count} 行。");
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Exporting data table failed: {0}",
                exception);
            SetStatus($"导出失败：{exception.GetBaseException().Message}");
        }
    }

    private void PrintButton_Click(object sender, RoutedEventArgs args)
    {
        if (allRows.Count == 0 || grid is null)
        {
            SetStatus("没有可打印的数据。");
            return;
        }

        PrintDialog dialog = new();
        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            FlowDocument document = CreatePrintDocument(dialog);
            IDocumentPaginatorSource paginatorSource = document;
            DocumentPaginator paginator = paginatorSource.DocumentPaginator;
            paginator.PageSize = new Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight);
            dialog.PrintDocument(paginator, "HMI 历史数据表");
            SetStatus($"已发送 {allRows.Count} 行到打印机。");
        }
        catch (Exception exception)
        {
            RuntimeServices.GetRequiredService<IHMITrace>().OutputDebugText(
                HMIDebugLevel.Error,
                "Printing data table failed: {0}",
                exception);
            SetStatus($"打印失败：{exception.GetBaseException().Message}");
        }
    }

    private FlowDocument CreatePrintDocument(PrintDialog dialog)
    {
        FlowDocument document = new()
        {
            PageWidth = dialog.PrintableAreaWidth,
            PageHeight = dialog.PrintableAreaHeight,
            PagePadding = new Thickness(24),
            ColumnWidth = dialog.PrintableAreaWidth,
            FontFamily = new FontFamily("Segoe UI"),
            FontSize = 8
        };
        document.Blocks.Add(new Paragraph(new Run("历史数据表"))
        {
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(0, 0, 0, 4)
        });

        string start = FormatSelectedDateTime(startDatePicker?.SelectedDate, startTimeTextBox?.Text);
        string end = FormatSelectedDateTime(endDatePicker?.SelectedDate, endTimeTextBox?.Text);
        document.Blocks.Add(new Paragraph(new Run($"查询范围：{start} 至 {end}"))
        {
            Margin = new Thickness(0, 0, 0, 8)
        });

        Table table = new()
        {
            CellSpacing = 0,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(0.5)
        };
        int columnCount = grid!.Columns.Count;
        table.Columns.Add(new TableColumn { Width = new GridLength(36) });
        table.Columns.Add(new TableColumn { Width = new GridLength(125) });
        for (int index = 2; index < columnCount; index++)
        {
            table.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
        }

        TableRowGroup headerGroup = new();
        TableRow header = new();
        foreach (GridViewDataColumn column in grid.Columns.OfType<GridViewDataColumn>())
        {
            header.Cells.Add(CreatePrintCell(column.Header?.ToString() ?? string.Empty, true));
        }
        headerGroup.Rows.Add(header);
        table.RowGroups.Add(headerGroup);

        TableRowGroup dataGroup = new();
        foreach (HistoricalDataRow row in allRows)
        {
            TableRow tableRow = new();
            tableRow.Cells.Add(CreatePrintCell(row.Index.ToString(CultureInfo.InvariantCulture)));
            tableRow.Cells.Add(CreatePrintCell(
                row.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
            foreach (double? value in row.Values)
            {
                tableRow.Cells.Add(CreatePrintCell(value?.ToString("G", CultureInfo.InvariantCulture) ?? string.Empty));
            }
            dataGroup.Rows.Add(tableRow);
        }
        table.RowGroups.Add(dataGroup);
        document.Blocks.Add(table);
        return document;
    }

    private static TableCell CreatePrintCell(string text, bool isHeader = false)
    {
        Paragraph paragraph = new(new Run(text))
        {
            Margin = new Thickness(0),
            TextAlignment = isHeader ? TextAlignment.Center : TextAlignment.Left
        };
        return new TableCell(paragraph)
        {
            Padding = new Thickness(3, 2, 3, 2),
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(0.25),
            Background = isHeader ? Brushes.LightGray : Brushes.White,
            FontWeight = isHeader ? FontWeights.Bold : FontWeights.Normal
        };
    }

    private static string FormatSelectedDateTime(DateTime? date, string? time) =>
        date is DateTime selectedDate && TimeOnly.TryParse(time, CultureInfo.InvariantCulture, out TimeOnly selectedTime)
            ? selectedDate.Date.Add(selectedTime.ToTimeSpan()).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            : "未设置";

    private void SetButtonVisibility(string partName, ButtonSettings settings)
    {
        if (GetTemplateChild(partName) is UIElement element)
        {
            element.Visibility = settings.Visibility;
        }
    }

    private void ApplyButtonSecurity(string partName, ButtonSettings settings)
    {
        if (GetTemplateChild(partName) is not Control control ||
            string.IsNullOrWhiteSpace(settings.SecurityAuthorizationName))
        {
            return;
        }

        bool authorized = connector?.CheckProjectAuthorization(settings.SecurityAuthorizationName) == true;
        if (authorized)
        {
            return;
        }
        if (settings.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide)
        {
            control.Visibility = Visibility.Collapsed;
        }
        else
        {
            control.IsEnabled = false;
        }
    }

    private void SetStatus(string message)
    {
        if (statusText is not null)
        {
            statusText.Text = message;
        }
    }

    private static string EscapeCsv(string value) =>
        "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";

    private static string[] ParseSignals(string? signals) =>
        (signals ?? string.Empty)
            .Split([';', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private sealed record DataTableUserConfiguration(string? Signals, int RangeHours, int ArchiveBit);
}

public sealed record HistoricalDataRow(int Index, DateTime Timestamp, IReadOnlyList<double?> Values);
