// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class NullableIntToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IEnumerable<bool> correctParameters = NullableIntToBoolConverter.GetCorrectParameters(parameter);
			return (value == null) ? Enumerable.First<bool>(correctParameters) : Enumerable.Last<bool>(correctParameters);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
			{
				return value;
			}
			IEnumerable<bool> correctParameters = NullableIntToBoolConverter.GetCorrectParameters(parameter);
			if (bool.Parse(value.ToString()) != Enumerable.First<bool>(correctParameters))
			{
				return 0;
			}
			return null;
		}
		private static IEnumerable<bool> GetCorrectParameters(object parameter)
		{
			string[] array = (parameter != null) ? parameter.ToString().ToLower().Split(new char[]
			{
				','
			}) : new string[0];
			if (array.Length < 2)
			{
				array = new string[]
				{
					"true",
					"false"
				};
			}
			return new bool[]
			{
				bool.Parse(Enumerable.First<string>(array).Trim()),
				bool.Parse(Enumerable.Last<string>(array).Trim())
			};
		}
	}
}
