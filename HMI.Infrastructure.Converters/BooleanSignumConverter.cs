// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BooleanSignumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is bool)
			{
				return (bool)value ? 1 : -1;
			}
			return 0;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is int)
			{
				return (int)value == 1;
			}
			return false;
		}
	}
}
