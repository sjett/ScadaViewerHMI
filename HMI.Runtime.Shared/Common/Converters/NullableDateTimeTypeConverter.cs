// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Globalization;
namespace HMI.Runtime.Shared.Common.Converters
{
	public class NullableDateTimeTypeConverter : TypeConverter
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
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return default(DateTime?);
			}
			object result;
			try
			{
				result = new DateTime?(DateTime.Parse(text, culture));
			}
			catch
			{
				try
				{
					result = new DateTime?(DateTime.ParseExact(text, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
				}
				catch
				{
					throw new InvalidCastException(text);
				}
			}
			return result;
		}
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			DateTime? dateTime = default(DateTime?);
			if (value is DateTime)
			{
				dateTime = new DateTime?((DateTime)value);
			}
			else
			{
				if (value is DateTime?)
				{
					dateTime = (DateTime?)value;
				}
			}
			if (dateTime.HasValue && dateTime.HasValue)
			{
				return dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
			}
			return string.Empty;
		}
	}
}
