// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Indicators
{
	public class HMIIndicator2 : HMIIndicatorBase
	{
		static HMIIndicator2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIIndicator2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIIndicator2)));
		}
	}
}
