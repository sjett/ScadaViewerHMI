// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
namespace HMI.Infrastructure.Converters
{
	public class ImageSourceToBitmapImageConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Contract.Requires<ArgumentNullException>(values != null);
			Contract.Requires<ArgumentException>(values != null && values.Length == 2);
			if (values != null && values[0] != null && values[1] != null && values[0] != DependencyProperty.UnsetValue)
			{
				if (values[1] != DependencyProperty.UnsetValue)
				{
					BitmapImage bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					string uriString = Path.Combine(values[0].ToString(), values[1].ToString());
					bitmapImage.UriSource = new Uri(uriString);
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.EndInit();
					bitmapImage.Freeze();
					return bitmapImage;
				}
			}
			return null;
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
