// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class ValueToPercentConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Contract.Requires<ArgumentNullException>(values != null);
			Contract.Requires<ArgumentException>(values.Length == 3);
			double minimumValue = System.Convert.ToDouble(values[0], CultureInfo.InvariantCulture);
			double maximumValue = System.Convert.ToDouble(values[1], CultureInfo.InvariantCulture);
			double currentValue = System.Convert.ToDouble(values[2], CultureInfo.InvariantCulture);
			if (minimumValue <= maximumValue && currentValue <= maximumValue)
			{
				if (currentValue >= minimumValue)
				{
					if (maximumValue != minimumValue)
					{
						double percentage = currentValue - minimumValue / (maximumValue - minimumValue);
						if (parameter == null)
						{
							return percentage;
						}
						double offset = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
						return percentage + offset;
					}
				}
				return 0;
			}
			return 1;
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
