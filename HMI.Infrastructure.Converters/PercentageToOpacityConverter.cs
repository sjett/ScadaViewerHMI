// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(double), typeof(double))]
	public class PercentageToOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? double.NaN : System.Convert.ToDouble(value) / 100.0;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? double.NaN : System.Convert.ToDouble(value) * 100.0;
		}
	}
}
