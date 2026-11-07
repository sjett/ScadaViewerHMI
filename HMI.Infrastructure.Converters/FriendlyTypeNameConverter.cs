// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class FriendlyTypeNameConverter : IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			string text = parameter as string;
			Type type = value as Type;
			if (type == null)
			{
				type = value.GetType();
			}
			if (text != null)
			{
				return string.Format(text, type.Name);
			}
			return type.Name;
		}
		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
