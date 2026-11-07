// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class VerticalAlignmentToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((VerticalAlignment)value  == VerticalAlignment.Top)
			{
				return false;
			}
			string text = ((VerticalAlignment)value).ToString();
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
			VerticalAlignment verticalAlignment;
			Enum.TryParse(alignmentName, true, out verticalAlignment);
			return verticalAlignment;
		}
	}
}
