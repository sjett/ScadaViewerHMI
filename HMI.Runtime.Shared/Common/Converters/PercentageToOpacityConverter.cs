// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class PercentageToOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value == null || !(value is double)) ? double.NaN : (System.Convert.ToDouble(value) / 100.0);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value == null || !(value is double)) ? double.NaN : (System.Convert.ToDouble(value) * 100.0);
		}
	}
}
