// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class ThicknessToStringConverter : QuadDoubleConverterBase, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Thickness))
			{
				return string.Empty;
			}
			Thickness thickness = (Thickness)value;
			Tuple<double, double, double, double> quad = new Tuple<double, double, double, double>(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
			return QuadDoubleToString(quad, ",", 0.0001);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string))
			{
				return default(Thickness);
			}
			return ThicknessFromString(value as string);
		}
		public Thickness ThicknessFromString(string s)
		{
			Tuple<double, double, double, double> tuple = QuadDoubleFromString(s);
			return new Thickness(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
		}
	}
}
