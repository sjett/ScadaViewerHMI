// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class TextAlignmentsToSystemTextAlignmentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((value is TextAlignments) ? ((TextAlignments)value) : TextAlignments.Left)
			{
			case TextAlignments.Left:
				return 1;
			case TextAlignments.Center:
				return 0;
			case TextAlignments.Right:
				return 2;
			default:
				return 1;
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((value is TextAlignment) ? (int)((TextAlignment)value) : 1)
			{
			case 0:
				return TextAlignments.Center;
			case 1:
				return TextAlignments.Left;
			case 2:
				return TextAlignments.Right;
			default:
				return TextAlignments.Left;
			}
		}
	}
}
