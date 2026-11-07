// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
namespace HMI.Infrastructure.Converters
{
	public class StringToXamlConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			object result;
			try
			{
				result = XamlReader.Parse(text);
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
