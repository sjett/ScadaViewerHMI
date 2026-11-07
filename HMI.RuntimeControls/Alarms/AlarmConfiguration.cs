// SPDX-License-Identifier: GPL-3.0-only
using System.Windows.Media;

namespace HMI.RuntimeControls.Alarms
{

public sealed class ColumnSettings
{
    public bool IsAutoSize { get; set; } = true;
    public double Width { get; set; } = 50;
    public string Type { get; set; } = "Text";
    public bool IsVisible { get; set; } = true;
    public bool IsFilterable { get; set; } = true;
}

public sealed class RowColor
{
    public Brush ActiveAlarmBackground { get; set; } = Brushes.Tomato;
    public Brush ActiveAlarmForeground { get; set; } = Brushes.Black;
    public Brush InactiveAlarmBackground { get; set; } = new SolidColorBrush(Color.FromRgb(0, 204, 102));
    public Brush InactiveAlarmForeground { get; set; } = Brushes.Black;
    public Brush AcknowledgedAlarmBackground { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 153));
    public Brush AcknowledgedAlarmForeground { get; set; } = Brushes.Black;
    public Brush AcknowledgedAndGoneAlarmBackground { get; set; } = new SolidColorBrush(Color.FromRgb(154, 205, 50));
    public Brush AcknowledgedAndGoneAlarmForeground { get; set; } = Brushes.Black;
    public string AlarmGroup { get; set; } = string.Empty;
    public string AlarmType { get; set; } = string.Empty;
    public int? Priority { get; set; }
    public string HornPath { get; set; } = string.Empty;
    public bool Loop { get; set; }
}

public sealed class PriorityColor
{
    public Brush AcknowledgedAlarmColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 153));
    public Brush AcknowledgedAndGoneAlarmColor { get; set; } = new SolidColorBrush(Color.FromRgb(154, 205, 50));
    public Brush ActiveAlarmColor { get; set; } = Brushes.Tomato;
    public Brush InactiveAlarmColor { get; set; } = new SolidColorBrush(Color.FromRgb(0, 204, 102));
    public int Priority { get; set; }
}
}

namespace HMI.RuntimeControls.Alarms.Common
{
    public sealed class AlarmGroupFilter
    {
        public string GroupName { get; set; } = string.Empty;
    }

    public sealed class AlarmTypeFilter
    {
        public string TypeName { get; set; } = string.Empty;
    }
}
