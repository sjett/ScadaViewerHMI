// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class DoubleToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is double))
			{
				return string.Empty;
			}
			return ((double)value).ToString("g", CultureInfo.InvariantCulture);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double num = 0.0;
			string text = value as string;
			Match match = Regex.Match(text ?? string.Empty, "\\d*\\.?\\d+");
			if (match.Success)
			{
				double.TryParse(match.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out num);
			}
			return num;
		}
	}
}
