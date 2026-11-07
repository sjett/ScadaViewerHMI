// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Common.Browsers.Views
{
    
    public class OKCancelPanelView : Control
    {
        static OKCancelPanelView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(OKCancelPanelView), new FrameworkPropertyMetadata(typeof(OKCancelPanelView)));
        }
    }
}
