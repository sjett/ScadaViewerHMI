// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.RuntimeControls.Indicators.Converters
{
	public class BooleanSignumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				if (value.GetType() == typeof(bool))
				{
					return ((bool)value) ? 1 : -1;
				}
			}
			return 0;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				if (value.GetType() == typeof(int))
				{
					return (int)value == 1;
				}
			}
			return false;
		}
	}
}
