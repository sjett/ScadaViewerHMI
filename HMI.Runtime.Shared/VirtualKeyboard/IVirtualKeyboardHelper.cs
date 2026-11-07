// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    public interface IVirtualKeyboardHelper
    {
        void CenterBottomControl(FrameworkElement element, FrameworkElement parent, double zoomFactor);
        Transform CreateRenderTransform(GeneralTransform transformToVisual);
    }
}

