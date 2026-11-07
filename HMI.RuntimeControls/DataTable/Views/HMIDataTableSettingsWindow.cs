// SPDX-License-Identifier: GPL-3.0-only
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace HMI.RuntimeControls.DataTable.Views;

internal sealed class HMIDataTableSettingsWindow : Window
{
    private readonly TextBox signalsTextBox;
    private readonly TextBox rangeHoursTextBox;
    private readonly TextBox archiveBitTextBox;

    public HMIDataTableSettingsWindow(string signals, int rangeHours, int archiveBit)
    {
        Title = "数据表设置";
        Width = 480;
        Height = 360;
        MinWidth = 420;
        MinHeight = 320;
        ResizeMode = ResizeMode.CanResizeWithGrip;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;

        Grid layout = new();
        layout.Margin = new Thickness(12);
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        TextBlock instructions = new()
        {
            Text = "每行填写一个 RapidSCADA 通道名称，也可使用分号分隔。",
            Margin = new Thickness(0, 0, 0, 6),
            TextWrapping = TextWrapping.Wrap
        };
        Grid.SetRow(instructions, 0);
        layout.Children.Add(instructions);

        signalsTextBox = new TextBox
        {
            Text = (signals ?? string.Empty).Replace(';', '\n'),
            AcceptsReturn = true,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            TextWrapping = TextWrapping.Wrap,
            MinHeight = 100,
            Margin = new Thickness(0, 0, 0, 8)
        };
        Grid.SetRow(signalsTextBox, 1);
        layout.Children.Add(signalsTextBox);

        rangeHoursTextBox = AddNumericSetting(layout, 2, "默认范围（小时）", rangeHours);
        archiveBitTextBox = AddNumericSetting(layout, 3, "ArchiveBit（0–255）", archiveBit);

        StackPanel buttons = new()
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 12, 0, 0)
        };
        Button cancelButton = new()
        {
            Content = "取消",
            IsCancel = true,
            MinWidth = 80,
            Margin = new Thickness(0, 0, 8, 0)
        };
        Button applyButton = new()
        {
            Content = "应用",
            IsDefault = true,
            MinWidth = 80
        };
        applyButton.Click += ApplyButton_Click;
        buttons.Children.Add(cancelButton);
        buttons.Children.Add(applyButton);
        Grid.SetRow(buttons, 4);
        layout.Children.Add(buttons);

        Content = layout;
    }

    public string Signals { get; private set; } = string.Empty;
    public int RangeHours { get; private set; }
    public int ArchiveBit { get; private set; }

    private static TextBox AddNumericSetting(Grid layout, int row, string label, int value)
    {
        Grid setting = new();
        setting.Margin = new Thickness(0, 4, 0, 0);
        setting.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        setting.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

        TextBlock text = new()
        {
            Text = label,
            VerticalAlignment = VerticalAlignment.Center
        };
        TextBox input = new()
        {
            Text = value.ToString(CultureInfo.InvariantCulture),
            VerticalContentAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(input, 1);
        setting.Children.Add(text);
        setting.Children.Add(input);
        Grid.SetRow(setting, row);
        layout.Children.Add(setting);
        return input;
    }

    private void ApplyButton_Click(object sender, RoutedEventArgs args)
    {
        if (!int.TryParse(rangeHoursTextBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int rangeHours) ||
            rangeHours <= 0)
        {
            MessageBox.Show(this, "默认范围必须是大于 0 的整数。", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (!int.TryParse(archiveBitTextBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int archiveBit) ||
            archiveBit is < 0 or > byte.MaxValue)
        {
            MessageBox.Show(this, "ArchiveBit 必须是 0 到 255 之间的整数。", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Signals = string.Join(
            ";",
            signalsTextBox.Text
                .Split([';', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase));
        RangeHours = rangeHours;
        ArchiveBit = archiveBit;
        DialogResult = true;
    }
}
