// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.ProjectDesigner.DesignControls.Charts;

public class HMIRealtimeChart : HMITrendingBase
{
    static HMIRealtimeChart()
    {
        System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIRealtimeChart),
            new System.Windows.FrameworkPropertyMetadata(typeof(HMIRealtimeChart)));
    }
}
