// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class StringNullOrEmptyFallbackConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string)
			{
				if (string.IsNullOrEmpty((string)value))
				{
					return parameter;
				}
				return value;
			}
			else
			{
				if (value == null)
				{
					return parameter;
				}
				string text = value.ToString();
				if (string.IsNullOrEmpty(text))
				{
					return parameter;
				}
				return text;
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
