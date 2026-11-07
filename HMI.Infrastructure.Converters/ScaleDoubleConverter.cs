// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class ScaleDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double d1 = 1.0;
			try
			{
				d1 = System.Convert.ToDouble(value, culture);
			}
			catch
			{
			}
			double d2 = 1.0;
			try
			{
				d2 = System.Convert.ToDouble(parameter, culture);
			}
			catch
			{
			}
			return d1 * d2;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
