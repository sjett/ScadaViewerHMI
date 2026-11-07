// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Modules.Design.Views.Browsers
{
	public class NullItemStyleSelector : StyleSelector
	{
		public Style NullStyle
		{
			get;
			set;
		}
		public Style NonNullStyle
		{
			get;
			set;
		}
		public override Style SelectStyle(object item, DependencyObject container)
		{
			IBrowserItemViewModel browserItemViewModel = item as IBrowserItemViewModel;
			if (browserItemViewModel == null)
			{
				return null;
			}
			if (browserItemViewModel.ItemText == null)
			{
				return this.NullStyle;
			}
			return this.NonNullStyle;
		}
	}
}
