// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Indicators
{
	public class HMIIndicatorDynamicColor : HMIIndicatorBase
	{
		static HMIIndicatorDynamicColor()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIIndicatorDynamicColor), new System.Windows.FrameworkPropertyMetadata(typeof(HMIIndicatorDynamicColor)));
		}
	}
}
