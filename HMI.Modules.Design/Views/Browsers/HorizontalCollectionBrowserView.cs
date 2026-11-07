// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Modules.Design.Views.Browsers
{
	[ExcludeFromCodeCoverage]
	public class HorizontalCollectionBrowserView : Control
	{
		static HorizontalCollectionBrowserView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalCollectionBrowserView), new FrameworkPropertyMetadata(typeof(HorizontalCollectionBrowserView)));
		}
	}
}
