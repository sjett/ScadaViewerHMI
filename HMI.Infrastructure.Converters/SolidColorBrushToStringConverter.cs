// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
namespace HMI.Infrastructure.Converters
{
	public class SolidColorBrushToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			BrushConverter brushConverter = new BrushConverter();
			return brushConverter.ConvertToInvariantString(value);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			BrushConverter brushConverter = new BrushConverter();
			if (brushConverter.IsValid(value))
			{
				return brushConverter.ConvertFromInvariantString((string)value);
			}
			return Brushes.Transparent;
		}
	}
}
