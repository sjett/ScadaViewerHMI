// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class ScaleThicknessConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double num = 1.0;
			try
			{
				num = System.Convert.ToDouble(value, culture);
			}
			catch
			{
				num = 1.0;
			}
			string text = (string)parameter;
			ThicknessConverter thicknessConverter = new ThicknessConverter();
			Thickness thickness = (Thickness)thicknessConverter.ConvertFromString(null, CultureInfo.InvariantCulture, text);
			Thickness thickness2 = new Thickness(thickness.Left * num, thickness.Top * num, thickness.Right * num, thickness.Bottom * num);
			return thickness2;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
