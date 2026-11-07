// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class DoubleToProcentOfDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double inputValue = (value is double) ? ((double)value) : double.NaN;
			int percentageScale = (parameter is int) ? ((int)parameter) : 100;
			if (!double.IsNaN(inputValue) && !double.IsInfinity(inputValue))
			{
				return (double)percentageScale * inputValue / 100.0;
			}
			return value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
