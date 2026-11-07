// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Modules.Design.Views.Browsers
{
	[ExcludeFromCodeCoverage]
	public class CollectionBrowserButtonsView : Control
	{
		static CollectionBrowserButtonsView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionBrowserButtonsView), new FrameworkPropertyMetadata(typeof(CollectionBrowserButtonsView)));
		}
	}
}
