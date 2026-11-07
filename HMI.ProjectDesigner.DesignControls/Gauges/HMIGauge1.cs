// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Gauges
{
	public class HMIGauge1 : HMIGaugeBase
	{
		static HMIGauge1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIGauge1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIGauge1)));
		}
	}
}
