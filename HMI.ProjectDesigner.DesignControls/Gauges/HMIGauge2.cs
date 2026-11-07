// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Gauges
{
	public class HMIGauge2 : HMIGaugeBase
	{
		static HMIGauge2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIGauge2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIGauge2)));
		}
	}
}
