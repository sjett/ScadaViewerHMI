// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class ConditionToWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ConditionToVisibilityConverter conditionToVisibilityConverter = new ConditionToVisibilityConverter();
            return ((Visibility)conditionToVisibilityConverter.Convert(value, targetType, parameter, culture) == Visibility.Collapsed) ? GridLength.Auto : new GridLength(1.0, GridUnitType.Star);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
