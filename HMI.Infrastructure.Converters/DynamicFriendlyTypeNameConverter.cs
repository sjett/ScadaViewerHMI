// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class DynamicFriendlyTypeNameConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			string text = values[1] as string;
			Type type = values[0] as Type;
			if (type == null)
			{
				return null;
			}
			if (text != null)
			{
				return string.Format(text, type.Name);
			}
			return type.Name;
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
