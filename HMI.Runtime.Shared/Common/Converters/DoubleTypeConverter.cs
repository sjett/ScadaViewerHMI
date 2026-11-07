// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Globalization;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class DoubleTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
		{
			return sourceType.Equals(typeof(string)) || base.CanConvertFrom(context, sourceType);
		}
		public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
		{
			return destinationType!.Equals(typeof(string)) || base.CanConvertTo(context, destinationType);
		}
		public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
		{
			string? text = value as string;
			if (text == null && value != null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			object result;
			try
			{
				result = ((!string.IsNullOrEmpty(text)) ? double.Parse(text, CultureInfo.InvariantCulture) : 0.0);
			}
			catch
			{
				throw new InvalidCastException(value!.ToString());
			}
			return result;
		}
		public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object value, Type destinationType)
		{
			if (destinationType.Equals(typeof(string)))
			{
				return ((double)value).ToString(CultureInfo.InvariantCulture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
