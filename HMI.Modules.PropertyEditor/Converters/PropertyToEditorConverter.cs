// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using HMI.Domain.Models;
namespace HMI.Modules.PropertyEditor.Converters
{
	public class PropertyToEditorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IProperty property = value as IProperty;
			if (property == null)
			{
				return null;
			}
			return property.PropertyEditorBuilder.CreateEditor(property, property);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
