// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
namespace HMI.Infrastructure.Converters
{
	public class ValidationErrorsToTextConverter : IValueConverter
	{
		
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IEnumerable<ValidationError> validationErrors = value as IEnumerable<ValidationError>;
			if (validationErrors == null)
			{
				return null;
			}
			IEnumerable<object> values = validationErrors.Select(validationError => validationError.ErrorContent);
			return string.Join(Environment.NewLine, values);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
