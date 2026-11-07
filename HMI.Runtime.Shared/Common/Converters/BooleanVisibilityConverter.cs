// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class BooleanVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				if (value.GetType() == typeof(bool))
				{
					return ((bool)value) ? 0 : 1;
				}
			}
			return 1;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new Exception();
		}
	}
}
