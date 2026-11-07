// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.ViewModels.Browsers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Modules.Design.Views.Browsers
{
	public class VerticalCollectionBrowserView : Control
	{
		static VerticalCollectionBrowserView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalCollectionBrowserView), new FrameworkPropertyMetadata(typeof(VerticalCollectionBrowserView)));
		}
        public VerticalCollectionBrowserView()
        {
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }
    }
}
