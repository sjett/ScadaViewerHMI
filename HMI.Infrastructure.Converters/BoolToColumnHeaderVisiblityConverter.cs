// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	[ValueConversion(typeof(bool), typeof(DataGridHeadersVisibility))]
	public class BoolToColumnHeaderVisiblityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && (bool)value)
			{
				return DataGridHeadersVisibility.Column;
			}
			return DataGridHeadersVisibility.None;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((DataGridHeadersVisibility)value  != DataGridHeadersVisibility.None && ((DataGridHeadersVisibility)value & DataGridHeadersVisibility.Column) == DataGridHeadersVisibility.Column)
			{
				return true;
			}
			return false;
		}
	}
}
