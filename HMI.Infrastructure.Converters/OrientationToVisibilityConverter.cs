// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(Orientation), typeof(Visibility))]
	public class OrientationToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (Orientation)value  == Orientation.Vertical || !value.Equals(Orientation.Horizontal) ? Visibility.Collapsed : Visibility.Visible;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(value is Visibility) || !value.Equals(Visibility.Visible) ? Orientation.Vertical : Orientation.Horizontal;
		}
	}
}
