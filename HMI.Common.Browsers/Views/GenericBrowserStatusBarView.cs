// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Common.Browsers.Views
{
    
    public class GenericBrowserStatusBarView : Control
    {
        static GenericBrowserStatusBarView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericBrowserStatusBarView), new FrameworkPropertyMetadata(typeof(GenericBrowserStatusBarView)));
        }
    }
}
