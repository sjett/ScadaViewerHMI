// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Common.Browsers.Converters
{
	public class GreaterThanValueToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IComparable comparable = value as IComparable;
			if (comparable != null && parameter != null)
			{
				if (parameter.GetType() != value.GetType())
				{
					parameter = System.Convert.ChangeType(parameter, parameter.GetType(), culture);
				}
				return (comparable.CompareTo(parameter) > 0) ? Visibility.Visible : Visibility.Collapsed;
			}
			return Visibility.Collapsed;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
