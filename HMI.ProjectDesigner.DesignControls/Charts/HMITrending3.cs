// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.ProjectDesigner.DesignControls.Charts;

public class HMITrending3 : HMITrendingBase
{
    static HMITrending3()
    {
        System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMITrending3),
            new System.Windows.FrameworkPropertyMetadata(typeof(HMITrending3)));
    }
}
