// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HMI.ProjectDesigner.DesignControls.Alarms;
namespace HMI.ProjectDesigner.DesignControls.Converters
{
	public class AlarmCountDisplayModeToVisibilityConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object objB = System.Enum.Parse(typeof(AlarmCountDisplayMode), parameter.ToString());
			if (object.Equals(value, objB))
			{
				return System.Windows.Visibility.Visible;
			}
			return System.Windows.Visibility.Collapsed;
		}
		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
	}
}
