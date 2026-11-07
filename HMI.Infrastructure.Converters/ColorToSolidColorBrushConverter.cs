// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
namespace HMI.Infrastructure.Converters
{
	public class ColorToSolidColorBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Color)
			{
				return new SolidColorBrush((Color)value);
			}
			return Brushes.Transparent;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is SolidColorBrush)
			{
				return ((SolidColorBrush)value).Color;
			}
			return Colors.Transparent;
		}
	}
}
