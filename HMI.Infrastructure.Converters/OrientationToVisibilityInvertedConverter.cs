// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(Visibility), typeof(Orientation))]
	public class OrientationToVisibilityInvertedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (Orientation)value  == Orientation.Horizontal || !value.Equals(Orientation.Vertical) ? Visibility.Collapsed : Visibility.Visible;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(value is Visibility) || !value.Equals(Visibility.Visible) ? Orientation.Horizontal : Orientation.Vertical;
		}
	}
}
