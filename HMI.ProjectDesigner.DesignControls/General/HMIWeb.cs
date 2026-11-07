// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Views;

namespace HMI.ProjectDesigner.DesignControls.General
{
    public class HMIWeb : CommonControl
    {
        static HMIWeb()
        {
            System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIWeb), new System.Windows.FrameworkPropertyMetadata(typeof(HMIWeb)));
        }
    }
}
