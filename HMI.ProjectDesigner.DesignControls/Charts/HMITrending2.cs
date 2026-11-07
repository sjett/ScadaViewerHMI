// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Charts
{
	public class HMITrending2 : HMITrendingBase
	{
		static HMITrending2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITrending2), new System.Windows.FrameworkPropertyMetadata(typeof(HMITrending2)));
		}
	}
}
