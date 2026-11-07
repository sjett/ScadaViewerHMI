// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class ConditionalVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Contract.Requires<ArgumentNullException>(values != null);
			Contract.Requires<ArgumentException>(values.Length == 2);
			object obj = parameter;
			if (values[1] is bool && (bool)values[1])
			{
				obj = values[0];
			}
			return obj.ToString() == "Visible" ? Visibility.Visible : Visibility.Collapsed;
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
