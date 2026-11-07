// SPDX-License-Identifier: GPL-3.0-only
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using HMI.Runtime.Communication;
using HMI.Runtime.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;
using Telerik.Reporting.Processing;
using SystemDataTable = System.Data.DataTable;
using SystemDataColumn = System.Data.DataColumn;
using TelerikReport = Telerik.Reporting.Report;
using TelerikReportTable = Telerik.Reporting.Table;
using WpfControl = System.Windows.Controls.Control;
using TelerikTextBox = Telerik.Reporting.TextBox;

namespace HMI.RuntimeControls.Reporting;

public enum ReportDataType
{
    HistoricalChannelValues,
    OperationEvents,
    AlarmHistory
}

public sealed class ChannelSelection : INotifyPropertyChanged
{
    private bool _isSelected;

    public ChannelSelection(string name) => Name = name;

    public event PropertyChangedEventHandler? PropertyChanged;
    public string Name { get; }
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value)
            {
                return;
            }
            _isSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
        }
    }
}

public sealed class HMITelerikReportViewer : WpfControl, INotifyPropertyChanged, IDisposable
{
    public static readonly DependencyProperty ReportDefinitionsUrlProperty = DependencyProperty.Register(
        nameof(ReportDefinitionsUrl), typeof(string), typeof(HMITelerikReportViewer), new PropertyMetadata(string.Empty));

    private readonly ObservableCollection<ChannelSelection> _channels = new();
    private IHMIConnector? _connector;
    private ReportDataType _selectedDataType = ReportDataType.HistoricalChannelValues;
    private DateTime _startDate = DateTime.Today;
    private DateTime _endDate = DateTime.Today;
    private string _statusMessage = "Ready.";
    private bool _isBusy;
    private bool _disposed;

    static HMITelerikReportViewer() => DefaultStyleKeyProperty.OverrideMetadata(
        typeof(HMITelerikReportViewer), new FrameworkPropertyMetadata(typeof(HMITelerikReportViewer)));

    public HMITelerikReportViewer()
    {
        Loaded += OnLoaded;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<ChannelSelection> Channels => _channels;
    public Array ReportTypes => Enum.GetValues<ReportDataType>();
    public ReportDataType SelectedDataType
    {
        get => _selectedDataType;
        set
        {
            if (_selectedDataType != value)
            {
                _selectedDataType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ChannelListVisibility));
            }
        }
    }
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (_startDate != value.Date)
            {
                _startDate = value.Date;
                OnPropertyChanged();
            }
        }
    }
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (_endDate != value.Date)
            {
                _endDate = value.Date;
                OnPropertyChanged();
            }
        }
    }
    public string StatusMessage { get => _statusMessage; private set { _statusMessage = value; OnPropertyChanged(); } }
    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            _isBusy = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanExport));
        }
    }
    public bool CanExport => !IsBusy;
    public Visibility ChannelListVisibility =>
        SelectedDataType == ReportDataType.HistoricalChannelValues ? Visibility.Visible : Visibility.Collapsed;
    public string ReportDefinitionsUrl
    {
        get => (string)GetValue(ReportDefinitionsUrlProperty);
        set => SetValue(ReportDefinitionsUrlProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (GetTemplateChild("PART_ExportPdfButton") is Button pdfButton)
        {
            pdfButton.Click += OnExportPdfClick;
        }
        if (GetTemplateChild("PART_ExportExcelButton") is Button excelButton)
        {
            excelButton.Click += OnExportExcelClick;
        }
    }

    private async void OnLoaded(object sender, RoutedEventArgs args)
    {
        try
        {
            _connector ??= RuntimeServices.GetRequiredService<IHMIConnector>();
            IReadOnlyList<string> signalNames = await _connector.GetSignalNamesAsync(string.Empty);
            _channels.Clear();
            foreach (string signalName in signalNames)
            {
                _channels.Add(new ChannelSelection(signalName));
            }
            StatusMessage = $"Loaded {signalNames.Count} RapidSCADA channel(s).";
        }
        catch (Exception exception)
        {
            StatusMessage = $"Unable to load RapidSCADA channels: {exception.Message}";
        }
    }

    private void OnExportPdfClick(object sender, RoutedEventArgs args) => _ = ExportAsync("PDF");
    private void OnExportExcelClick(object sender, RoutedEventArgs args) => _ = ExportAsync("XLSX");

    private async Task ExportAsync(string format)
    {
        if (IsBusy)
        {
            return;
        }
        if (StartDate > EndDate)
        {
            StatusMessage = "Start date must not be after end date.";
            return;
        }

        SaveFileDialog dialog = new()
        {
            AddExtension = true,
            DefaultExt = format == "PDF" ? ".pdf" : ".xlsx",
            Filter = format == "PDF" ? "PDF document (*.pdf)|*.pdf" : "Excel workbook (*.xlsx)|*.xlsx",
            FileName = $"RapidSCADA_{SelectedDataType}_{StartDate:yyyyMMdd}_{EndDate:yyyyMMdd}"
        };
        if (dialog.ShowDialog() != true)
        {
            StatusMessage = "Export cancelled.";
            return;
        }

        IsBusy = true;
        StatusMessage = "Querying RapidSCADA data...";
        try
        {
            SystemDataTable data = await LoadReportDataAsync();
            StatusMessage = $"Generating {format} report ({data.Rows.Count} row(s))...";
            await Task.Run(() =>
            {
                if (format == "PDF")
                {
                    WritePdf(data, dialog.FileName, SelectedDataType);
                }
                else
                {
                    WriteExcel(data, dialog.FileName);
                }
            });
            StatusMessage = $"Report saved to {dialog.FileName}";
        }
        catch (Exception exception)
        {
            StatusMessage = $"Report generation failed: {exception.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task<SystemDataTable> LoadReportDataAsync()
    {
        _connector ??= RuntimeServices.GetRequiredService<IHMIConnector>();
        DateTimeOffset start = new(StartDate.Date);
        DateTimeOffset end = new(EndDate.Date.AddDays(1).AddTicks(-1));
        SystemDataTable table = new();

        switch (SelectedDataType)
        {
            case ReportDataType.HistoricalChannelValues:
            {
                string[] signalNames = _channels.Where(channel => channel.IsSelected).Select(channel => channel.Name).ToArray();
                if (signalNames.Length == 0)
                {
                    throw new InvalidOperationException("Select at least one channel for a historical values report.");
                }

                table.Columns.Add("Timestamp", typeof(DateTime));
                table.Columns.Add("SignalName", typeof(string));
                table.Columns.Add("Value", typeof(double));
                table.Columns.Add("Status", typeof(int));
                IReadOnlyList<HistoricalTrendSeries> series = await _connector.GetHistoricalTrendsAsync(
                    string.Empty, signalNames, start, end);
                foreach (HistoricalTrendSeries trend in series)
                {
                    foreach (HistoricalTrendPoint point in trend.Points)
                    {
                        table.Rows.Add(point.Timestamp, trend.SignalName, point.Value, point.Status);
                    }
                }
                break;
            }
            case ReportDataType.OperationEvents:
            {
                table.Columns.Add("Timestamp", typeof(DateTime));
                table.Columns.Add("SignalName", typeof(string));
                table.Columns.Add("Text", typeof(string));
                table.Columns.Add("PreviousValue", typeof(double));
                table.Columns.Add("Value", typeof(double));
                table.Columns.Add("Severity", typeof(int));
                table.Columns.Add("Acknowledged", typeof(bool));
                IReadOnlyList<OperationEvent> events = await _connector.GetOperationEventsAsync(
                    string.Empty, start, end, 10000);
                foreach (OperationEvent item in events)
                {
                    table.Rows.Add(item.Timestamp, item.SignalName, item.Text,
                        item.PreviousValue.HasValue ? item.PreviousValue.Value : DBNull.Value,
                        item.Value.HasValue ? item.Value.Value : DBNull.Value,
                        item.Severity, item.Acknowledged);
                }
                break;
            }
            case ReportDataType.AlarmHistory:
            {
                table.Columns.Add("Timestamp", typeof(DateTime));
                table.Columns.Add("SignalName", typeof(string));
                table.Columns.Add("Message", typeof(string));
                table.Columns.Add("Severity", typeof(int));
                table.Columns.Add("Acknowledged", typeof(bool));
                table.Columns.Add("AcknowledgedAt", typeof(DateTime));
                table.Columns.Add("AlarmGroup", typeof(string));
                table.Columns.Add("AlarmType", typeof(string));
                IReadOnlyList<AlarmHistoryEvent> events = await _connector.GetAlarmHistoryAsync(
                    string.Empty, start, end);
                foreach (AlarmHistoryEvent item in events)
                {
                    table.Rows.Add(item.Timestamp, item.ChannelName, item.Message, item.Severity,
                        item.Acknowledged,
                        item.AcknowledgedAt.HasValue ? item.AcknowledgedAt.Value : DBNull.Value,
                        item.AlarmGroupName, item.AlarmTypeName);
                }
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(SelectedDataType));
        }

        return table;
    }

    private static void WritePdf(SystemDataTable data, string filePath, ReportDataType reportType)
    {
        TelerikReport report = new()
        {
            Name = "RapidScadaReport",
            DocumentName = reportType.ToString(),
            Width = Unit.Cm(26)
        };
        report.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
        report.PageSettings.Landscape = true;
        report.PageSettings.Margins = new MarginsU(Unit.Cm(1), Unit.Cm(1), Unit.Cm(1), Unit.Cm(1));

        PageHeaderSection header = new() { Height = Unit.Cm(1.2) };
        TelerikTextBox title = new()
        {
            Name = "ReportTitle",
            Value = $"{reportType} - {DateTime.Now:yyyy-MM-dd HH:mm}",
            Location = new PointU(Unit.Cm(0), Unit.Cm(0)),
            Size = new SizeU(Unit.Cm(26), Unit.Cm(1))
        };
        header.Items.Add(title);

        Telerik.Reporting.DetailSection detail = new() { Height = Unit.Cm(1) };
        TelerikReportTable table = BuildTable(data);
        detail.Items.Add(table);
        report.Items.AddRange([header, detail]);

        InstanceReportSource source = new() { ReportDocument = report };
        RenderingResult result = new ReportProcessor().RenderReport("PDF", source, new System.Collections.Hashtable());
        if (result.HasErrors)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Errors.Select(error => error.Message)));
        }
        File.WriteAllBytes(filePath, result.DocumentBytes);
    }

    private static TelerikReportTable BuildTable(SystemDataTable data)
    {
        TelerikReportTable table = new() { Name = "ReportTable", DataSource = data };
        TableGroup detailGroup = new();
        detailGroup.Groupings.Add(new Grouping(null));
        table.RowGroups.Add(detailGroup);
        table.Body.Rows.Add(new TableBodyRow(Unit.Cm(0.55)));

        for (int index = 0; index < data.Columns.Count; index++)
        {
            SystemDataColumn dataColumn = data.Columns[index];
            string itemName = $"Column{index}";
            TableBodyColumn bodyColumn = new(Unit.Cm(26d / Math.Max(1, data.Columns.Count)));
            table.Body.Columns.Add(bodyColumn);

            TelerikTextBox headerText = new()
            {
                Name = $"{itemName}Header",
                Value = dataColumn.ColumnName,
                Size = new SizeU(bodyColumn.Width, Unit.Cm(0.55))
            };
            TableGroup columnGroup = new() { Name = itemName, ReportItem = headerText };
            table.ColumnGroups.Add(columnGroup);

            TelerikTextBox valueText = new()
            {
                Name = $"{itemName}Value",
                Value = $"= Fields.{dataColumn.ColumnName}",
                Size = new SizeU(bodyColumn.Width, Unit.Cm(0.55))
            };
            table.Body.SetCellContent(0, index, valueText);
        }

        return table;
    }

    private static void WriteExcel(SystemDataTable data, string filePath)
    {
        using SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
        WorkbookPart workbookPart = document.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();
        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        SheetData sheetData = new();

        Row header = new();
        foreach (SystemDataColumn column in data.Columns)
        {
            header.Append(CreateTextCell(column.ColumnName));
        }
        sheetData.Append(header);

        foreach (DataRow dataRow in data.Rows)
        {
            Row row = new();
            foreach (SystemDataColumn column in data.Columns)
            {
                object value = dataRow[column];
                row.Append(value switch
                {
                    DBNull => new Cell(),
                    bool booleanValue => new Cell
                    {
                        DataType = CellValues.Boolean,
                        CellValue = new CellValue(booleanValue ? "1" : "0")
                    },
                    byte or short or int or long or float or double or decimal =>
                        new Cell
                        {
                            DataType = CellValues.Number,
                            CellValue = new CellValue(Convert.ToString(value, CultureInfo.InvariantCulture))
                        },
                    DateTime dateTime => CreateTextCell(dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)),
                    _ => CreateTextCell(Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty)
                });
            }
            sheetData.Append(row);
        }

        worksheetPart.Worksheet = new Worksheet(sheetData);
        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
        sheets.Append(new Sheet
        {
            Id = workbookPart.GetIdOfPart(worksheetPart),
            SheetId = 1,
            Name = "RapidSCADA"
        });
        workbookPart.Workbook.Save();
    }

    private static Cell CreateTextCell(string value) => new()
    {
        DataType = CellValues.InlineString,
        InlineString = new InlineString(new Text(value))
    };

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        Loaded -= OnLoaded;
        _disposed = true;
    }
}
