// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Common.Browsers.Views
{
    
    public class OKCancelRefreshPanelView : Control
    {
        static OKCancelRefreshPanelView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(OKCancelRefreshPanelView), new FrameworkPropertyMetadata(typeof(OKCancelRefreshPanelView)));
        }
    }
}
