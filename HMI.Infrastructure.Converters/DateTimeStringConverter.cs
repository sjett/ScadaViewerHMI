// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(DateTime), typeof(string))]
	public class DateTimeStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is DateTime)
			{
				return ((DateTime)value).ToString(culture);
			}
			return string.Empty;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return DateTime.Now;
			}
			try
			{
				return DateTime.Parse(text, culture);
			}
			catch
			{
			}
			return DateTime.Now;
		}
	}
}
