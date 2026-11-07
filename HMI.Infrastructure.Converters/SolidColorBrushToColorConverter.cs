// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
namespace HMI.Infrastructure.Converters
{
	public class SolidColorBrushToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SolidColorBrush solidColorBrush = value as SolidColorBrush;
			return (solidColorBrush ?? Brushes.Transparent).Color;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Color))
			{
				return Brushes.Transparent;
			}
			return new SolidColorBrush((Color)value);
		}
	}
}
