// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class TimeSpanToDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			TimeSpan? timeSpan = value as TimeSpan?;
			return !timeSpan.HasValue ? 0.0 : timeSpan.Value.TotalMilliseconds;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double? num = value as double?;
			return !num.HasValue ? TimeSpan.Zero : TimeSpan.FromMilliseconds(num.Value);
		}
	}
}
