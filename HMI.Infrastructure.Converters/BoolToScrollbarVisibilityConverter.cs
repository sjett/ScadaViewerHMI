// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(bool), typeof(ScrollBarVisibility))]
	public class BoolToScrollbarVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && (bool)value)
			{
				return ScrollBarVisibility.Auto;
			}
			return ScrollBarVisibility.Hidden;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((ScrollBarVisibility)value  != ScrollBarVisibility.Disabled && ((ScrollBarVisibility)value == ScrollBarVisibility.Auto || (ScrollBarVisibility)value == ScrollBarVisibility.Visible))
			{
				return true;
			}
			return false;
		}
	}
}
