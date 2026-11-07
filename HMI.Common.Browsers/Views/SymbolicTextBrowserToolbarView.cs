// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Common.Browsers.Views
{
    
    public class SymbolicTextBrowserToolbarView : Control
    {
        static SymbolicTextBrowserToolbarView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SymbolicTextBrowserToolbarView), new FrameworkPropertyMetadata(typeof(SymbolicTextBrowserToolbarView)));
        }
    }
}
