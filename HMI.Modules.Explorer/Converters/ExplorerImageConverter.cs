// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Modules.Explorer.ViewModels;
namespace HMI.Modules.Explorer.Converters
{
	[ValueConversion(typeof(IExplorerViewModel), typeof(string))]
	public class ExplorerImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is IProjectViewModel)
			{
				return "/HMI.Modules.Explorer;component/Resources/Images/FolderClosed.png";
			}
			if (value is ITemplatePageViewModel)
			{
				return "/HMI.Modules.Explorer;component/Resources/Images/Document_Blank16.png";
			}
			if (value is IProjectPageViewModel)
			{
				return "/HMI.Modules.Explorer;component/Resources/Images/Document_Blank16.png";
			}
			if (value is INavigationTargetViewModel)
			{
				return "/HMI.Modules.Explorer;component/Resources/Images/NavigationNode16.png";
			}
			return null;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
