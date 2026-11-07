// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace HMI.Runtime.Shared.Common.Converters
{
	[ContentProperty("ImageMap")]
	public class MessageBoxImageSelector : IValueConverter
	{
		public MessageBoxImageMappings ImageMap
		{
			get;
			set;
		}
		public MessageBoxImageSelector()
		{
			this.ImageMap = new MessageBoxImageMappings();
		}
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is MessageBoxType)
			{
				MessageBoxType messageBoxType = (MessageBoxType)value;
				return Enumerable.FirstOrDefault<ImageSource>(Enumerable.Select<MessageBoxImageMap, ImageSource>(Enumerable.Where<MessageBoxImageMap>(this.ImageMap, (MessageBoxImageMap x) => x.MessageBoxType == messageBoxType), (MessageBoxImageMap x) => x.Image));
			}
			return null;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
