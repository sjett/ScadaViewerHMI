// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class CornerRadiusToStringConverter : QuadDoubleConverterBase, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is CornerRadius))
			{
				return string.Empty;
			}
			CornerRadius cornerRadius = (CornerRadius)value;
			Tuple<double, double, double, double> quad = new Tuple<double, double, double, double>(cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft);
			return QuadDoubleToString(quad, ",", 0.0001);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string))
			{
				return default(CornerRadius);
			}
			return CornerRadiusFromString(value as string);
		}
		public CornerRadius CornerRadiusFromString(string s)
		{
			Tuple<double, double, double, double> tuple = QuadDoubleFromString(s);
			return new CornerRadius(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
		}
	}
}
