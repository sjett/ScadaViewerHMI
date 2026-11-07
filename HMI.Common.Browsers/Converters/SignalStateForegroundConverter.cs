// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Common.Browsers.Converters
{
	public class SignalStateForegroundConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ISignalDefinitionViewModel signalDefinitionViewModel = value as ISignalDefinitionViewModel;
			if (signalDefinitionViewModel == null)
			{
				return Brushes.Black;
			}
			if (!signalDefinitionViewModel.IsActive)
			{
				if (!signalDefinitionViewModel.IsVChannel)
				{
					return Brushes.DarkGray;
				}
				return Brushes.LightBlue;
			}
			else
			{
				if (!signalDefinitionViewModel.IsVChannel)
				{
					return Brushes.Black;
				}
				return Brushes.DodgerBlue;
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
