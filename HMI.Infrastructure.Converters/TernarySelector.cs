// SPDX-License-Identifier: GPL-3.0-only
using System.Diagnostics;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class TernarySelector : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Contract.Requires<ArgumentNullException>(values != null);
			Contract.Requires<ArgumentException>(values.Length == 3);
			object result;
			try
			{
				if (Equals(values[0], DependencyProperty.UnsetValue))
				{
					result = DependencyProperty.UnsetValue;
				}
				else
				{
					result = (bool)values[0] ? values[2] : values[1];
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
