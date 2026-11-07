// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class ConditionalNumberConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Contract.Requires<ArgumentNullException>(values != null);
			Contract.Requires<ArgumentException>(values.Length == 2);
			object value = parameter;
			if (values[1] is bool && (bool)values[1])
			{
				value = values[0];
			}
			return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
