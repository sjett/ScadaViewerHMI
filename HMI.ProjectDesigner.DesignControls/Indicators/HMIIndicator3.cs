// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Indicators
{
	public class HMIIndicator3 : HMIIndicatorBase
	{
		static HMIIndicator3()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIIndicator3), new System.Windows.FrameworkPropertyMetadata(typeof(HMIIndicator3)));
		}
	}
}
