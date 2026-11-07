// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(bool), typeof(double))]
	public class BoolToMinOrNaNDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && (bool)value)
			{
				return double.NaN;
			}
			return 4.94065645841247E-324;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double && double.IsNaN((double)value))
			{
				return true;
			}
			return false;
		}
	}
}
