// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Globalization;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class NullableInt32TypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType.Equals(typeof(string)) || base.CanConvertFrom(context, sourceType);
		}
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType.Equals(typeof(string)) || base.CanConvertTo(context, destinationType);
		}
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is int)
			{
				return (int?)value;
			}
			string text = value as string;
			if (value != null && text == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			return (!string.IsNullOrEmpty(text)) ? new int?(int.Parse(text, CultureInfo.InvariantCulture)) : default(int?);
		}
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
