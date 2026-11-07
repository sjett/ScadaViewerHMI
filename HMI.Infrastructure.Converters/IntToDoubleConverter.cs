// SPDX-License-Identifier: GPL-3.0-only
using System.Globalization;
using System.Windows.Data;

namespace HMI.Infrastructure.Converters
{
    [ValueConversion(typeof(double), typeof(int))]
    public class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            try
            {
                result = System.Convert.ToDouble(value, culture);
            }
            catch
            {
                result = value;
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            try
            {
                result = System.Convert.ToInt32(value, culture);
            }
            catch
            {
                result = value;
            }
            return result;
        }
    }
}
