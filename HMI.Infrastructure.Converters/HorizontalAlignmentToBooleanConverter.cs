// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class HorizontalAlignmentToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((HorizontalAlignment)value  == HorizontalAlignment.Left)
			{
				return false;
			}
			string text = ((HorizontalAlignment)value).ToString();
			string alignmentName = parameter as string;
			if (string.IsNullOrWhiteSpace(alignmentName))
			{
				return false;
			}
			return text.Equals(alignmentName, StringComparison.InvariantCultureIgnoreCase);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string alignmentName = parameter as string;
			HorizontalAlignment horizontalAlignment;
			Enum.TryParse(alignmentName, true, out horizontalAlignment);
			return horizontalAlignment;
		}
	}
}
