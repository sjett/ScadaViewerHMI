// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class NullableIntToGridLengthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int? num = value as int?;
			string text = parameter as string;
			if (num.HasValue)
			{
				return new GridLength((double)num.Value);
			}
			return (string.IsNullOrWhiteSpace(text) || !(text.ToLower() == "auto")) ? new GridLength(1.0, GridUnitType.Star) : GridLength.Auto;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
