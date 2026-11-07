// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class AverageValueStringConverter : IMultiValueConverter
	{
		
		private static Func<object, double> _convertToDouble;
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Contract.Requires<ArgumentNullException>(values != null);
			Contract.Requires<ArgumentException>(values.Length >= 2);
			if (_convertToDouble == null)
			{
                _convertToDouble = new Func<object, double>(ConvertToDouble);
			}
			return values.Average(_convertToDouble).ToString(CultureInfo.InvariantCulture);
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
		
		private static double ConvertToDouble(object value)
		{
			return (double)value;
		}
	}
}
