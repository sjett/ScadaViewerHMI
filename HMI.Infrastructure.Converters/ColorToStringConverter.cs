// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
namespace HMI.Infrastructure.Converters
{
	public class ColorToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ColorConverter colorConverter = new ColorConverter();
			return colorConverter.ConvertToInvariantString(value);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ColorConverter colorConverter = new ColorConverter();
			return colorConverter.ConvertFromInvariantString((string)value);
		}
	}
}
