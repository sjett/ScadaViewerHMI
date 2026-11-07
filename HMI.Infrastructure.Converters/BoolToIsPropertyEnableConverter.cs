// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(bool), typeof(bool))]
	public class BoolToIsPropertyEnableConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(parameter is bool))
			{
				return true;
			}
			if (value is bool)
			{
				return (bool)value && (bool)parameter;
			}
			return (bool)parameter;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value;
		}
	}
}
